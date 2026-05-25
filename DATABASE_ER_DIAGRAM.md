# ER-диаграмма базы данных

База приложения хранится в локальном SQLite-файле `learn_csharp_app.db3`.
Таблицы создаются в `Services/DatabaseService.cs` через `sqlite-net-pcl`.

```mermaid
erDiagram
    LESSON {
        int Id PK "Справочник в коде, не таблица SQLite"
        string Title
        string Description
        string Content
        string Difficulty
    }

    STARTED_LESSON {
        int LessonId PK "Логическая ссылка на Lesson.Id"
        DateTime StartedAt
    }

    COMPLETED_LESSON {
        int LessonId PK "Логическая ссылка на Lesson.Id"
        DateTime CompletedAt
    }

    QUIZ_RESULT {
        int Id PK "AutoIncrement"
        int LessonId "Логическая ссылка на Lesson.Id"
        string LessonTitle
        int Score
        int TotalQuestions
        double Percent
        DateTime CompletedAt
    }

    USER_PROGRESS {
        int Id PK "Singleton row, Id = 1"
        int CompletedLessons
        int TotalLessons
        int TestsTaken
        int BestQuizScore
        int BestQuizTotalQuestions
        double OverallProgress
        DateTime UpdatedAt
    }

    LESSON ||--o| STARTED_LESSON : "started"
    LESSON ||--o| COMPLETED_LESSON : "completed"
    LESSON ||--o| QUIZ_RESULT : "latest quiz result"
    COMPLETED_LESSON }o--|| USER_PROGRESS : "recalculates"
    QUIZ_RESULT }o--|| USER_PROGRESS : "recalculates"
```

## Фактические таблицы SQLite

В SQLite создаются только эти таблицы:

- `StartedLesson`
- `CompletedLesson`
- `QuizResult`
- `UserProgress`

`Lesson`, `QuizQuestion`, `QuizAnswerOption` и `LessonProgress` не создаются как таблицы базы данных. Они используются как модели данных в коде: уроки и вопросы тестов описаны статически в сервисах, а `LessonProgress` собирается на лету для отображения прогресса.

## Важные замечания

- В моделях нет атрибутов внешних ключей, поэтому связи с `Lesson.Id` являются логическими, а не ограничениями SQLite.
- Для `QuizResult` сервис перед вставкой удаляет старый результат по `LessonId`, поэтому фактически хранится последний результат теста для урока.
- `UserProgress` является агрегированной строкой с `Id = 1` и пересчитывается из завершённых уроков и результатов тестов.
