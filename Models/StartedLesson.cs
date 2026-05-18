using SQLite;

namespace LearnCSharpApp.Models;

public class StartedLesson
{
    [PrimaryKey]
    public int LessonId { get; set; }

    public DateTime StartedAt { get; set; }
}
