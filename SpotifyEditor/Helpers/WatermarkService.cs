using System.Windows;

namespace SpotifyEditor.Helpers
{
    public static class WatermarkService
    {
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.RegisterAttached(
                "Placeholder",
                typeof(string),
                typeof(WatermarkService),
                new PropertyMetadata(string.Empty));

        public static void SetPlaceholder(DependencyObject element, string value)
            => element.SetValue(PlaceholderProperty, value);

        public static string GetPlaceholder(DependencyObject element)
            => (string)element.GetValue(PlaceholderProperty);
    }
}
