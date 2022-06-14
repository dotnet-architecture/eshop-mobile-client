using System;
using Microsoft.Maui.Controls.Shapes;

namespace eShopOnContainers.Views
{
	[ContentProperty(nameof(Content))]
	public class BadgeView : Grid
	{
		private ContentPresenter _badgeContent;
		private Border _border;
		private RoundRectangle _borderShape;
		private Label _badgeIndicator;

		public static BindableProperty ContentProperty =
            BindableProperty.Create(nameof(Content), typeof(View), typeof(BadgeView), default(View),
				propertyChanged: OnLayoutPropertyChanged);

        public View Content
		{
            get => (View)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(BadgeView), default(string),
				propertyChanged: OnLayoutPropertyChanged);

        public string Text
		{
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static BindableProperty TextColorProperty =
			BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(BadgeView), default(Color),
				propertyChanged: OnLayoutPropertyChanged);

        public Color TextColor
		{
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static BindableProperty FontSizeProperty =
			BindableProperty.Create(nameof(FontSize), typeof(double), typeof(BadgeView), 10.0d,
				propertyChanged: OnLayoutPropertyChanged);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty BadgeColorProperty =
			BindableProperty.Create(nameof(BadgeColor), typeof(Color), typeof(BadgeView), default(Color),
				propertyChanged: OnLayoutPropertyChanged);

        public Color  BadgeColor
		{
            get => (Color)GetValue( BadgeColorProperty);
            set => SetValue( BadgeColorProperty, value);
        }

        public static BindableProperty InsetProperty =
			BindableProperty.Create(nameof(Inset), typeof(double), typeof(BadgeView), 4.0d,
				propertyChanged: OnLayoutPropertyChanged);

        public double Inset
        {
            get => (double)GetValue(InsetProperty);
            set => SetValue(InsetProperty, value);
        }

        static void OnLayoutPropertyChanged(BindableObject bindable, object oldValue, object newValue) =>
			(bindable as BadgeView)?.UpdateLayout();

		public BadgeView()
		{
			this.ColumnDefinitions =
				new ColumnDefinitionCollection
				{
					new ColumnDefinition(GridLength.Auto),
				};

			this.RowDefinitions =
				new RowDefinitionCollection
				{
					new RowDefinition(GridLength.Auto),
				};

			_badgeContent =
				new ContentPresenter
				{
					ZIndex = 0
				};

			_badgeIndicator =
				new Label
				{
					Padding = 4,
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
				};

			_borderShape = new RoundRectangle();

			_border =
				new Border
				{
					StrokeShape = _borderShape,
					Content = _badgeIndicator,
					HorizontalOptions = LayoutOptions.End,
					VerticalOptions = LayoutOptions.Start,
					ZIndex = 1,
				};

			this.Children.Add(_badgeContent);
			this.Children.Add(_border);

			UpdateLayout();
		}

		protected override void OnHandlerChanging(HandlerChangingEventArgs args)
		{
			base.OnHandlerChanging(args);

			_border.SizeChanged -= BadgeIndicatorSizeChanged;

			if (args.NewHandler is not null)
			{
				_border.SizeChanged += BadgeIndicatorSizeChanged;
			}
		}

        private void BadgeIndicatorSizeChanged(object sender, EventArgs e)
        {
			_border.MinimumWidthRequest = _border.Height;
			_borderShape.CornerRadius = _border.Height * .5f;
        }

		private void UpdateLayout()
		{
			BatchBegin();
			_badgeContent.BatchBegin();
			_border.BatchBegin();
			_badgeIndicator.BatchBegin();

			this.Padding = Inset;

			_badgeContent.Content = Content;

			_border.TranslationY = -Inset;
			_border.TranslationX = Inset;
			_border.BackgroundColor = BadgeColor;

			_badgeIndicator.Text = Text;
			_badgeIndicator.TextColor = TextColor;
			_badgeIndicator.FontSize = FontSize;

			_badgeContent.BatchCommit();
			_border.BatchCommit();
			_badgeIndicator.BatchCommit();
			BatchCommit();
		}
	}
}

