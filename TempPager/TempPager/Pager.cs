using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace TempPager
{
    public class Pager : Control
    {
        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(nameof(CurrentPage), typeof(int), typeof(Pager), new PropertyMetadata(1, OnCurrentPageChanged));

        public static readonly DependencyProperty NextButtonStyleProperty = DependencyProperty.Register(nameof(NextButtonStyle), typeof(Style), typeof(Pager), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty PagerItemStyleProperty = DependencyProperty.Register(nameof(PagerItemStyle), typeof(Style), typeof(Pager), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty PreviousButtonStyleProperty = DependencyProperty.Register(nameof(PreviousButtonStyle), typeof(Style), typeof(Pager), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty TotalPageProperty = DependencyProperty.Register(nameof(TotalPage), typeof(int), typeof(Pager), new PropertyMetadata(1, OnTotalPageChanged));

        private ListView _listView;

        private Button _nextButton;

        private Button _previousButton;

        public Pager()
        {
            DefaultStyleKey = typeof(Pager);
        }

        public event EventHandler<int> PageChanged;

        public int CurrentPage
        {
            get
            {
                return (int)GetValue(CurrentPageProperty);
            }
            set
            {
                SetValue(CurrentPageProperty, value);
            }
        }

        public Style NextButtonStyle
        {
            get
            {
                return (Style)GetValue(NextButtonStyleProperty);
            }
            set
            {
                SetValue(NextButtonStyleProperty, value);
            }
        }

        public Style PagerItemStyle
        {
            get
            {
                return (Style)GetValue(PagerItemStyleProperty);
            }
            set
            {
                SetValue(PagerItemStyleProperty, value);
            }
        }

        public Style PreviousButtonStyle
        {
            get
            {
                return (Style)GetValue(PreviousButtonStyleProperty);
            }
            set
            {
                SetValue(PreviousButtonStyleProperty, value);
            }
        }

        public int TotalPage
        {
            get
            {
                return (int)GetValue(TotalPageProperty);
            }
            set
            {
                SetValue(TotalPageProperty, value);
            }
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _listView = (ListView)GetTemplateChild("PART_ListView");
            _listView.SelectionChanged += ListView_SelectionChanged;

            _previousButton = (Button)GetTemplateChild("PART_PreviousButton");
            _previousButton.Click += PreviousButton_Click;
            _nextButton = (Button)GetTemplateChild("PART_NextButton");
            _nextButton.Click += NextButton_Click;

            UpdateView();
        }

        private static void OnCurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (Pager)d;

            obj.UpdateView();
            obj.PageChanged?.Invoke(obj, obj.CurrentPage);
        }

        private static void OnTotalPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (Pager)d;

            obj.UpdateView();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_listView.SelectedItem != null)
            {
                CurrentPage = (int)_listView.SelectedItem;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPage)
            {
                CurrentPage++;
            }
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        private void UpdateView()
        {
            if (_listView != null)
            {
                _listView.ItemsSource = Enumerable.Range(1, TotalPage);
                _listView.SelectedItem = CurrentPage;
            }
            if (_previousButton != null)
            {
                _previousButton.IsEnabled = CurrentPage > 1;
            }
            if (_nextButton != null)
            {
                _nextButton.IsEnabled = CurrentPage < TotalPage;
            }
        }
    }
}