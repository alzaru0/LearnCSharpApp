using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class LessonsPage : ContentPage
{
    private readonly LessonsViewModel _viewModel;

    public LessonsPage(LessonsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.AnimatePageEntranceAsync();
        await _viewModel.LoadAsync();
    }
}
