using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnCSharpApp.Models;

public partial class QuizAnswerOption : ObservableObject
{
    public int QuestionId { get; set; }

    public int Index { get; set; }

    public string Text { get; set; } = string.Empty;

    [ObservableProperty]
    private bool isSelected;
}
