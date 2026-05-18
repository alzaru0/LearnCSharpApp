using SQLite;

namespace LearnCSharpApp.Models;

public class CompletedLesson
{
    [PrimaryKey]
    public int LessonId { get; set; }

    public DateTime CompletedAt { get; set; }
}
