namespace LearnCSharpApp.Models;

public class Lesson
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Difficulty { get; set; } = string.Empty;

    public bool IsStarted { get; set; }

    public bool IsCompleted { get; set; }

    public string StatusText => IsCompleted
        ? "Завершен"
        : IsStarted
            ? "В процессе"
            : "Не изучен";
}
