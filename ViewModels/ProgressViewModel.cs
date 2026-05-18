using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnCSharpApp.Services;

namespace LearnCSharpApp.ViewModels;

public partial class ProgressViewModel : BaseViewModel
{
    private readonly IProgressService _progressService;
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private int completedLessons;

    [ObservableProperty]
    private int totalLessons;

    [ObservableProperty]
    private double progressValue;

    [ObservableProperty]
    private string progressPercentText = "0%";

    [ObservableProperty]
    private string averageQuizScoreText = "Нет данных";

    [ObservableProperty]
    private int testsTaken;

    [ObservableProperty]
    private string completedLessonsText = "0/0";

    public ProgressViewModel(IProgressService progressService, DatabaseService databaseService)
    {
        _progressService = progressService;
        _databaseService = databaseService;
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
            await RefreshProgressAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task CompleteNextLessonAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _progressService.CompleteNextLessonAsync();
            await RefreshProgressAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ResetProgressAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            await _progressService.ResetProgressAsync();
            await RefreshProgressAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RefreshProgressAsync()
    {
        var lessons = await _progressService.GetLessonsAsync();
        var quizResults = await _databaseService.GetQuizResultsAsync();

        TotalLessons = lessons.Count;
        CompletedLessons = lessons.Count(lesson => lesson.IsCompleted);
        ProgressValue = TotalLessons == 0 ? 0 : (double)CompletedLessons / TotalLessons;
        ProgressPercentText = $"{ProgressValue:P0}";
        CompletedLessonsText = $"{CompletedLessons}/{TotalLessons}";
        TestsTaken = quizResults.Count;
        AverageQuizScoreText = quizResults.Count == 0
            ? "Нет данных"
            : $"{quizResults.Average(result => result.Percent):P0}";
    }
}
