using SQLite;

namespace LearnCSharpApp.Models;

public class UserProgress
{
    [PrimaryKey]
    public int Id { get; set; } = 1;

    public int CompletedLessons { get; set; }

    public int TotalLessons { get; set; }

    public int TestsTaken { get; set; }

    public int BestQuizScore { get; set; }

    public int BestQuizTotalQuestions { get; set; }

    public double OverallProgress { get; set; }

    public DateTime UpdatedAt { get; set; }
}
