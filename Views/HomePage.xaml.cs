using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
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
