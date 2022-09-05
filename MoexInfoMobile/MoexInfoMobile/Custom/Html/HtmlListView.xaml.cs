using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Контейнер элементов списка.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public abstract partial class HtmlListView : ContentView
    {
        public HtmlListView()
        {
            InitializeComponent();
        }



        /// <summary>Стиль текста списка.</summary>
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        /// <summary>Привязка: стиль текста списка.</summary>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(HtmlListView), null);



        /// <summary>Стиль маркера списка.</summary>
        public Style MarkerStyle
        {
            get { return (Style)GetValue(MarkerStyleProperty); }
            set { SetValue(MarkerStyleProperty, value); }
        }

        /// <summary>Привязка: стиль маркера списка.</summary>
        public static readonly BindableProperty MarkerStyleProperty =
            BindableProperty.Create(nameof(MarkerStyle), typeof(Style), typeof(HtmlListView), null);



        /// <summary>
        /// Инициализирует список, на основе массива строк списка.
        /// </summary>
        /// <param name="values">Строки списка.</param>
        public abstract void InitializeList(string[] values);


        /// <summary>
        /// Создает маркер списка.
        /// </summary>
        /// <returns>Маркер списка.</returns>
        protected abstract View CreateMarker();


        /// <summary>
        /// Добавляет новый элемент в список.
        /// </summary>
        /// <param name="marker">Маркер.</param>
        /// <param name="text">Текст.</param>
        protected void AddRow(View marker, string text)
        {
            // Контейнер элемента.
            var rowContainer = new Grid();
            rowContainer.Style = rowContainerStyle;

            // Добавление маркара в контейнер.
            Grid.SetColumn(marker, 0);
            rowContainer.Children.Add(marker);

            // Текст элемента списка.
            var label = new Label
            {
                Text = text,
                BindingContext = this
            };

            label.SetBinding(StyleProperty, nameof(TextStyle));

            // Добавление текста в контейнер.
            Grid.SetColumn(label, 1);
            rowContainer.Children.Add(label);
            _list.Children.Add(rowContainer);
        }
    }
}