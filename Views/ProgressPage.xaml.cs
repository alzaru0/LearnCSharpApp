using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class ProgressPage : ContentPage
{
    private readonly ProgressViewModel _viewModel;

    public ProgressPage(ProgressViewModel viewModel)
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
