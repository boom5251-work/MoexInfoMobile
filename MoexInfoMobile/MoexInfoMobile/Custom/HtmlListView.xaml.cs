using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract partial class HtmlListView : ContentView
    {
        public HtmlListView()
        {
            InitializeComponent();
        }



        // Стиль текста списка.
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(HtmlListView), null);


        // Стиль маркера списка.
        public Style MarkerStyle
        {
            get { return (Style)GetValue(MarkerStyleProperty); }
            set { SetValue(MarkerStyleProperty, value); }
        }

        public static readonly BindableProperty MarkerStyleProperty =
            BindableProperty.Create(nameof(MarkerStyle), typeof(Style), typeof(HtmlListView), null);



        // Метод инициализирует список.
        public abstract void InitializeList(string[] values);

        // Метод создает маркер.
        protected abstract View CreateMarker();


        // Метод добавляет новых элемент в список.
        protected void AddRow(View marker, string text)
        {
            /// Контейнер элемента.
            Grid rowContainer = new Grid();
            rowContainer.Style = rowContainerStyle;

            /// Добавление маркара в контейнер.
            Grid.SetColumn(marker, 0);
            rowContainer.Children.Add(marker);

            /// Текст элемента списка.
            Label label = new Label
            {
                Text = text,
                BindingContext = this
            };

            label.SetBinding(StyleProperty, nameof(TextStyle));

            /// Добавление текста в контейнер.
            Grid.SetColumn(label, 1);
            rowContainer.Children.Add(label);
            _list.Children.Add(rowContainer);
        }
    }
}