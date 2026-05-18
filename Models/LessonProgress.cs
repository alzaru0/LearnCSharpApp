using SQLite;

namespace LearnCSharpApp.Models;

public class LessonProgress
{
    [PrimaryKey]
    public string LessonKey { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Order { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime UpdatedAt { get; set; }
}
