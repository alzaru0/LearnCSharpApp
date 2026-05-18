using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnCSharpApp.Models;

public partial class QuizLessonFilter : ObservableObject
{
    public int LessonId { get; set; }

    public string Title { get; set; } = string.Empty;

    public int QuestionCount { get; set; }

    [ObservableProperty]
    private bool isSelected;
}
