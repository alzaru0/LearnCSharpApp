using LearnCSharpApp.ViewModels;

namespace LearnCSharpApp.Views;

public partial class SettingsPage : ContentPage
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await this.AnimatePageEntranceAsync();
    }
}
