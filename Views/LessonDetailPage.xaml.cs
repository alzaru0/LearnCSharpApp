using LearnCSharpApp.Models;
using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class LessonDetailPage : ContentPage, IQueryAttributable
{
    private readonly LessonDetailViewModel _viewModel;

    public LessonDetailPage(LessonDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    public async void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue(nameof(Lesson), out var value) && value is Lesson selectedLesson)
        {
            await _viewModel.LoadLessonAsync(selectedLesson);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.AnimatePageEntranceAsync();
        await _viewModel.RefreshLastQuizResultAsync();
    }
}
