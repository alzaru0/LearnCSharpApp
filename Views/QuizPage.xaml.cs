using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class QuizPage : ContentPage, IQueryAttributable
{
    private readonly QuizViewModel _viewModel;

    public QuizPage(QuizViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public void SelectLesson(int lessonId)
    {
        _viewModel.SelectLessonById(lessonId);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.AnimatePageEntranceAsync();
        await _viewModel.LoadAsync();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("lessonId", out var value))
        {
            return;
        }

        if (int.TryParse(value?.ToString(), out var lessonId))
        {
            SelectLesson(lessonId);
        }
    }
}
