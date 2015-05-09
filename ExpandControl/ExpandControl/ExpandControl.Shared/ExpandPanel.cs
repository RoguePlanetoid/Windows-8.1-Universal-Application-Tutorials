﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace ExpandControl
{
    [TemplateVisualState(Name = "Collapsed", GroupName = "ViewStates")]
    [TemplateVisualState(Name = "Expanded", GroupName = "ViewStates")]
    [TemplatePart(Name = "Content", Type = typeof(FrameworkElement))]
    [TemplatePart(Name = "ExpandCollapseButton", Type = typeof(ToggleButton))]
    public class ExpandPanel : ContentControl
    {
        private bool _useTransitions = true;
        private VisualState _collapsedState;
        private ToggleButton toggleExpander;
        private FrameworkElement contentElement;

        public static readonly DependencyProperty HeaderContentProperty =
        DependencyProperty.Register("HeaderContent", typeof(object),
        typeof(ExpandPanel), null);

        public static readonly DependencyProperty IsExpandedProperty =
        DependencyProperty.Register("IsExpanded", typeof(bool),
        typeof(ExpandPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty CornerRadiusProperty =
        DependencyProperty.Register("CornerRadius", typeof(CornerRadius),
        typeof(ExpandPanel), null);

        public object HeaderContent
        {
            get { return GetValue(HeaderContentProperty); }
            set { SetValue(HeaderContentProperty, value); }
        }

        public bool IsExpanded
        {
            get { return (bool)GetValue(IsExpandedProperty); }
            set { SetValue(IsExpandedProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        private void changeVisualState(bool useTransitions)
        {
            if (IsExpanded)
            {
                if (contentElement != null)
                {
                    contentElement.Visibility = Visibility.Visible;
                }
                VisualStateManager.GoToState(this, "Expanded", useTransitions);
            }
            else
            {
                VisualStateManager.GoToState(this, "Collapsed", useTransitions);
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if (_collapsedState == null)
                {
                    if (contentElement != null)
                    {
                        contentElement.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        public ExpandPanel()
        {
            DefaultStyleKey = typeof(ExpandPanel);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            toggleExpander = (ToggleButton)GetTemplateChild("ExpandCollapseButton");
            if (toggleExpander != null)
            {
                toggleExpander.Click += (object sender, RoutedEventArgs e) =>
                {
                    IsExpanded = !IsExpanded;
                    toggleExpander.IsChecked = IsExpanded;
                    changeVisualState(_useTransitions);
                };
            }
            contentElement = (FrameworkElement)GetTemplateChild("Content");
            if (contentElement != null)
            {
                _collapsedState = (VisualState)GetTemplateChild("Collapsed");
                if ((_collapsedState != null) && (_collapsedState.Storyboard != null))
                {
                    _collapsedState.Storyboard.Completed += (object sender, object e) =>
                    {
                        contentElement.Visibility = Visibility.Collapsed;
                    };
                }
            }
            changeVisualState(false);
        }
    }
}
