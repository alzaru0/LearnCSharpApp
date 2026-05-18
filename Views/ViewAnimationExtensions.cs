namespace LearnCSharpApp.Views;

public static class ViewAnimationExtensions
{
    public static async Task AnimatePageEntranceAsync(this VisualElement page)
    {
        // Легкая анимация делает переходы между вкладками менее резкими на Android.
        page.Opacity = 0;
        page.TranslationY = 18;

        await Task.WhenAll(
            page.FadeToAsync(1, 260, Easing.CubicOut),
            page.TranslateToAsync(0, 0, 260, Easing.CubicOut));
    }
}
