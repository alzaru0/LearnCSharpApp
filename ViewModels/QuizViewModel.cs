using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnCSharpApp.Models;
using LearnCSharpApp.Services;

namespace LearnCSharpApp.ViewModels;

public partial class QuizViewModel : BaseViewModel
{
    private readonly IQuizService _quizService;
    private readonly DatabaseService _databaseService;
    private readonly ILessonService _lessonService;
    private readonly List<QuizQuestion> _allQuestions = [];
    private int? pendingLessonId;

    [ObservableProperty]
    private string selectedLessonTitle = string.Empty;

    [ObservableProperty]
    private int score;

    [ObservableProperty]
    private int totalQuestions;

    [ObservableProperty]
    private bool hasResult;

    [ObservableProperty]
    private string resultText = "Выберите ответы и нажмите «Завершить тест».";

    [ObservableProperty]
    private string emptyMessage = "Откройте тест из нужного урока.";

    public ObservableCollection<QuizLessonFilter> LessonFilters { get; } = [];

    public ObservableCollection<QuizQuestion> Questions { get; } = [];

    public bool HasSelectedLesson => !string.IsNullOrWhiteSpace(SelectedLessonTitle);

    public bool CanSubmitQuiz => HasSelectedLesson && Questions.Count > 0 && !HasResult;

    public bool CanShowResultActions => HasResult;

    public double ResultProgress => TotalQuestions == 0 ? 0 : (double)Score / TotalQuestions;

    public string ResultPercentText => $"{ResultProgress:P0}";

    public string ResultFeedbackText => ResultProgress switch
    {
        >= 0.9 => "Отличный результат! Тема хорошо закреплена.",
        >= 0.7 => "Хороший результат. Осталось повторить пару деталей.",
        >= 0.5 => "Неплохо, но лучше еще раз пройти материал урока.",
        _ => "Стоит вернуться к уроку и попробовать тест снова."
    };

    [ObservableProperty]
    private bool isResultPopupVisible;

    public QuizViewModel(IQuizService quizService, DatabaseService databaseService, ILessonService lessonService)
    {
        _quizService = quizService;
        _databaseService = databaseService;
        _lessonService = lessonService;
    }

    public async Task LoadAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;

            var questions = await _quizService.GetQuestionsAsync();
            _allQuestions.Clear();
            _allQuestions.AddRange(questions);

            BuildLessonFilters();
            SelectInitialLesson();
        }
        finally
        {
            IsBusy = false;
        }
    }

    partial void OnSelectedLessonTitleChanged(string value)
    {
        OnPropertyChanged(nameof(HasSelectedLesson));
        OnPropertyChanged(nameof(CanSubmitQuiz));
        ApplyLessonFilter();
    }

    public void SelectLessonById(int lessonId)
    {
        pendingLessonId = lessonId;

        if (_allQuestions.Count > 0)
        {
            SelectLessonFilter(LessonFilters.FirstOrDefault(filter => filter.LessonId == lessonId));
            pendingLessonId = null;
        }
    }

    [RelayCommand]
    private void SelectLessonFilter(QuizLessonFilter? filter)
    {
        if (filter is null)
        {
            return;
        }

        foreach (var item in LessonFilters)
        {
            item.IsSelected = item == filter;
        }

        if (SelectedLessonTitle == filter.Title)
        {
            ApplyLessonFilter();
            return;
        }

        SelectedLessonTitle = filter.Title;
    }

    [RelayCommand]
    private void SelectAnswer(QuizAnswerOption option)
    {
        if (HasResult)
        {
            return;
        }

        var question = Questions.FirstOrDefault(item => item.Id == option.QuestionId);
        question?.SelectAnswer(option.Index);
    }

    [RelayCommand]
    private async Task SubmitQuizAsync()
    {
        if (!HasSelectedLesson || Questions.Count == 0)
        {
            return;
        }

        TotalQuestions = Questions.Count;
        Score = Questions.Count(question => question.IsCorrect);
        HasResult = true;
        IsResultPopupVisible = true;
        ResultText = $"Ваш результат: {Score} из {TotalQuestions}";

        // Повторная сдача обновляет результат урока, чтобы статистика считала уникальные тесты.
        var firstQuestion = Questions.FirstOrDefault();
        var lessonId = firstQuestion?.LessonId ?? 0;

        if (lessonId > 0)
        {
            await _lessonService.MarkLessonCompletedAsync(lessonId);
        }

        await _databaseService.SaveQuizResultAsync(new QuizResult
        {
            LessonId = lessonId,
            LessonTitle = SelectedLessonTitle,
            Score = Score,
            TotalQuestions = TotalQuestions
        });

        var totalLessons = _allQuestions.Select(question => question.LessonId).Distinct().Count();
        await _databaseService.RecalculateUserProgressAsync(totalLessons);

    }

    [RelayCommand]
    private void ResetQuiz()
    {
        foreach (var question in Questions)
        {
            question.SelectAnswer(-1);
        }

        Score = 0;
        HasResult = false;
        IsResultPopupVisible = false;
        ResultText = "Выберите ответы и нажмите «Завершить тест».";
    }

    [RelayCommand]
    private void CloseResultPopup()
    {
        IsResultPopupVisible = false;
    }

    [RelayCommand]
    private async Task ReturnToLessonAsync()
    {
        await Shell.Current.Navigation.PopModalAsync();
    }

    private void ApplyLessonFilter()
    {
        var filteredQuestions = _allQuestions
            .Where(question => question.LessonTitle == SelectedLessonTitle)
            .ToList();

        Questions.Clear();
        for (var index = 0; index < filteredQuestions.Count; index++)
        {
            var question = filteredQuestions[index];
            question.DisplayNumber = index + 1;
            Questions.Add(question);
        }

        TotalQuestions = Questions.Count;
        HasResult = false;
        IsResultPopupVisible = false;
        ResultText = HasSelectedLesson
            ? "Выберите ответы и нажмите «Завершить тест»."
            : "Тест доступен только из выбранного урока.";
        EmptyMessage = HasSelectedLesson
            ? "Для этого урока пока нет вопросов."
            : "Откройте тест из нужного урока.";
        OnPropertyChanged(nameof(CanSubmitQuiz));
    }

    private void BuildLessonFilters()
    {
        LessonFilters.Clear();

        foreach (var group in _allQuestions.GroupBy(question => new { question.LessonId, question.LessonTitle }))
        {
            LessonFilters.Add(new QuizLessonFilter
            {
                LessonId = group.Key.LessonId,
                Title = group.Key.LessonTitle,
                QuestionCount = group.Count()
            });
        }
    }

    private void SelectInitialLesson()
    {
        var lessonId = pendingLessonId;
        var selectedFilter = lessonId is null
            ? LessonFilters.FirstOrDefault(filter => filter.Title == SelectedLessonTitle)
            : LessonFilters.FirstOrDefault(filter => filter.LessonId == lessonId.Value);

        SelectLessonFilter(selectedFilter);
        pendingLessonId = null;
    }

    partial void OnHasResultChanged(bool value)
    {
        OnPropertyChanged(nameof(CanSubmitQuiz));
        OnPropertyChanged(nameof(CanShowResultActions));
    }

    partial void OnScoreChanged(int value)
    {
        OnPropertyChanged(nameof(ResultProgress));
        OnPropertyChanged(nameof(ResultPercentText));
        OnPropertyChanged(nameof(ResultFeedbackText));
    }

    partial void OnTotalQuestionsChanged(int value)
    {
        OnPropertyChanged(nameof(ResultProgress));
        OnPropertyChanged(nameof(ResultPercentText));
        OnPropertyChanged(nameof(ResultFeedbackText));
    }
}
