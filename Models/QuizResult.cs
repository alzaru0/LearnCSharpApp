using SQLite;

namespace LearnCSharpApp.Models;

public class QuizResult
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public int LessonId { get; set; }

    public string LessonTitle { get; set; } = string.Empty;

    public int Score { get; set; }

    public int TotalQuestions { get; set; }

    public double Percent { get; set; }

    public DateTime CompletedAt { get; set; }
}
