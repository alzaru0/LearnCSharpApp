using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearnCSharpApp.Models;
using LearnCSharpApp.Services;
using LearnCSharpApp.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LearnCSharpApp.ViewModels;

public partial class LessonDetailViewModel : BaseViewModel
{
    private readonly ILessonService _lessonService;
    private readonly DatabaseService _databaseService;
    private readonly IServiceProvider _serviceProvider;
    private Lesson? lesson;

    [ObservableProperty]
    private string lessonTitle = string.Empty;

    [ObservableProperty]
    private string description = string.Empty;

    [ObservableProperty]
    private string lessonContent = string.Empty;

    [ObservableProperty]
    private int lessonId;

    [ObservableProperty]
    private bool isCompleted;

    [ObservableProperty]
    private bool isStarted;

    [ObservableProperty]
    private string completionMessage = string.Empty;

    [ObservableProperty]
    private string lastQuizPercentText = string.Empty;

    [ObservableProperty]
    private bool hasLastQuizResult;

    [ObservableProperty]
    private FormattedString highlightedCode = new();

    public LessonDetailViewModel(
        ILessonService lessonService,
        DatabaseService databaseService,
        IServiceProvider serviceProvider)
    {
        _lessonService = lessonService;
        _databaseService = databaseService;
        _serviceProvider = serviceProvider;
    }

    public string OrderText => $"Урок {LessonId}";

    public string StatusText => IsCompleted
        ? "Завершен"
        : IsStarted
            ? "В процессе"
            : "Не изучен";

    public bool HasCompletionMessage => !string.IsNullOrWhiteSpace(CompletionMessage);

    public async Task LoadLessonAsync(Lesson selectedLesson)
    {
        // Shell передает выбранный урок целиком, поэтому экран не дублирует поиск по Id.
        lesson = selectedLesson;
        LessonId = selectedLesson.Id;
        LessonTitle = selectedLesson.Title;
        Description = selectedLesson.Description;
        LessonContent = selectedLesson.Content;
        IsStarted = selectedLesson.IsStarted;
        IsCompleted = selectedLesson.IsCompleted;
        HighlightedCode = CreateHighlightedCode(GetCodeExample(selectedLesson.Id));
        CompletionMessage = string.Empty;
        await RefreshLastQuizResultAsync();

        if (!selectedLesson.IsCompleted && !selectedLesson.IsStarted)
        {
            await _lessonService.MarkLessonStartedAsync(selectedLesson.Id);
            selectedLesson.IsStarted = true;
            IsStarted = true;
        }
    }

    public async Task RefreshLastQuizResultAsync()
    {
        if (LessonId <= 0)
        {
            return;
        }

        var quizResults = await _databaseService.GetQuizResultsAsync();
        var lastResult = quizResults.FirstOrDefault(result => result.LessonId == LessonId);

        HasLastQuizResult = lastResult is not null;
        LastQuizPercentText = lastResult is null
            ? string.Empty
            : $"{lastResult.Percent:P0}";
    }

    [RelayCommand]
    private async Task StartQuizAsync()
    {
        var quizPage = _serviceProvider.GetRequiredService<QuizPage>();
        quizPage.SelectLesson(LessonId);

        await Shell.Current.Navigation.PushModalAsync(quizPage);
    }

    [RelayCommand]
    private async Task MarkAsCompletedAsync()
    {
        if (lesson is null)
        {
            return;
        }

        await _lessonService.MarkLessonCompletedAsync(lesson.Id);

        lesson.IsCompleted = true;
        IsCompleted = true;
        CompletionMessage = "Урок отмечен как изученный.";
    }

    partial void OnLessonIdChanged(int value)
    {
        OnPropertyChanged(nameof(OrderText));
    }

    partial void OnIsCompletedChanged(bool value)
    {
        OnPropertyChanged(nameof(StatusText));
    }

    partial void OnIsStartedChanged(bool value)
    {
        OnPropertyChanged(nameof(StatusText));
    }

    partial void OnCompletionMessageChanged(string value)
    {
        OnPropertyChanged(nameof(HasCompletionMessage));
    }

    private static string GetCodeExample(int id)
    {
        return id switch
        {
            1 => "string name = \"Aliya\";\nint age = 18;\nbool isStudent = true;\nConsole.WriteLine($\"{name}, {age}\");",
            2 => "int score = 85;\n\nif (score >= 90)\n{\n    Console.WriteLine(\"Отлично\");\n}\nelse\n{\n    Console.WriteLine(\"Продолжайте практику\");\n}",
            3 => "for (int i = 1; i <= 5; i++)\n{\n    Console.WriteLine($\"Шаг {i}\");\n}",
            4 => "static int Sum(int a, int b)\n{\n    return a + b;\n}\n\nint result = Sum(4, 6);",
            5 => "int[] marks = { 8, 9, 10 };\n\nforeach (int mark in marks)\n{\n    Console.WriteLine(mark);\n}",
            6 => "class Student\n{\n    public string Name { get; set; } = string.Empty;\n\n    public void SayHello()\n    {\n        Console.WriteLine($\"Привет, {Name}!\");\n    }\n}",
            _ => "Console.WriteLine(\"Learn C#\");"
        };
    }

    private static FormattedString CreateHighlightedCode(string code)
    {
        var formatted = new FormattedString();

        // Простая подсветка достаточна для демонстрации C#-примеров в курсовой.
        var keywords = new HashSet<string>
        {
            "bool", "class", "else", "foreach", "for", "if", "in", "int", "public", "return", "static", "string", "void"
        };

        foreach (var line in code.Split('\n'))
        {
            foreach (var token in SplitCodeLine(line))
            {
                formatted.Spans.Add(new Span
                {
                    Text = token,
                    TextColor = GetTokenColor(token, keywords)
                });
            }

            formatted.Spans.Add(new Span { Text = Environment.NewLine, TextColor = Color.FromArgb("#E5E7EB") });
        }

        return formatted;
    }

    private static IEnumerable<string> SplitCodeLine(string line)
    {
        var current = string.Empty;

        foreach (var character in line)
        {
            if (char.IsLetterOrDigit(character) || character == '_')
            {
                current += character;
                continue;
            }

            if (current.Length > 0)
            {
                yield return current;
                current = string.Empty;
            }

            yield return character.ToString();
        }

        if (current.Length > 0)
        {
            yield return current;
        }
    }

    private static Color GetTokenColor(string token, HashSet<string> keywords)
    {
        if (keywords.Contains(token))
        {
            return Color.FromArgb("#60A5FA");
        }

        if (token.StartsWith("\"", StringComparison.Ordinal) || token.EndsWith("\"", StringComparison.Ordinal))
        {
            return Color.FromArgb("#FBBF24");
        }

        if (char.IsDigit(token.FirstOrDefault()))
        {
            return Color.FromArgb("#A78BFA");
        }

        return Color.FromArgb("#E5E7EB");
    }
}
