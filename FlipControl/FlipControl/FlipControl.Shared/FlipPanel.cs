using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace FlipControl
{
    [TemplateVisualState(Name = "Normal", GroupName = "ViewStates")]
    [TemplateVisualState(Name = "Flipped", GroupName = "ViewStates")]
    [TemplatePart(Name = "FlipButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "FlipButtonAlternative", Type = typeof(ToggleButton))]
    public class FlipPanel : Control
    {
        public static readonly DependencyProperty FrontContentProperty =
        DependencyProperty.Register("FrontContent", typeof(object),
        typeof(FlipPanel), null);

        public static readonly DependencyProperty BackContentProperty =
        DependencyProperty.Register("BackContent", typeof(object),
        typeof(FlipPanel), null);

        public static readonly DependencyProperty IsFlippedProperty =
        DependencyProperty.Register("IsFlipped", typeof(bool),
        typeof(FlipPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(FlipPanel), null);

        public object FrontContent
        {
            get { return GetValue(FrontContentProperty); }
            set { SetValue(FrontContentProperty, value); }
        }

        public object BackContent
        {
            get { return GetValue(BackContentProperty); }
            set { SetValue(BackContentProperty, value); }
        }

        public bool IsFlipped
        {
            get { return (bool)GetValue(IsFlippedProperty); }
            set { SetValue(IsFlippedProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void changeVisualState(bool useTransitions)
        {
            if (IsFlipped)
            {
                VisualStateManager.GoToState(this, "Normal", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Flipped", useTransitions);
            }
        }

        public FlipPanel()
        {
            DefaultStyleKey = typeof(FlipPanel);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ToggleButton flipButton = (ToggleButton)GetTemplateChild("FlipButton");
            if (flipButton != null)
            {
                flipButton.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    changeVisualState(true);
                };
            }
            ToggleButton flipButtonAlt = (ToggleButton)GetTemplateChild("FlipButtonAlternative");
            if (flipButtonAlt != null)
            {
                flipButtonAlt.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsFlipped = !IsFlipped;
                    changeVisualState(true);
                };
            }
            changeVisualState(false);
        }
    }
}
