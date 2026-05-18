using CommunityToolkit.Mvvm.ComponentModel;

namespace LearnCSharpApp.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private bool isBusy;
}
