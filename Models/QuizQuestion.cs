using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnCSharpApp.Models;

public partial class QuizQuestion : ObservableObject
{
    [ObservableProperty]
    private int selectedAnswerIndex = -1;

    public int Id { get; set; }

    public int DisplayNumber { get; set; }

    public int LessonId { get; set; }

    public string LessonTitle { get; set; } = string.Empty;

    public string QuestionText { get; set; } = string.Empty;

    public int CorrectAnswerIndex { get; set; }

    public ObservableCollection<QuizAnswerOption> Options { get; } = [];

    public bool IsAnswered => SelectedAnswerIndex >= 0;

    public bool IsCorrect => SelectedAnswerIndex == CorrectAnswerIndex;

    public string ResultText => !IsAnswered
        ? "Ответ не выбран"
        : IsCorrect
            ? "Верно"
            : "Неверно";

    public void SelectAnswer(int answerIndex)
    {
        SelectedAnswerIndex = answerIndex;

        foreach (var option in Options)
        {
            option.IsSelected = option.Index == answerIndex;
        }

        OnPropertyChanged(nameof(IsAnswered));
        OnPropertyChanged(nameof(IsCorrect));
        OnPropertyChanged(nameof(ResultText));
    }

    partial void OnSelectedAnswerIndexChanged(int value)
    {
        OnPropertyChanged(nameof(IsAnswered));
        OnPropertyChanged(nameof(IsCorrect));
        OnPropertyChanged(nameof(ResultText));
    }
}
