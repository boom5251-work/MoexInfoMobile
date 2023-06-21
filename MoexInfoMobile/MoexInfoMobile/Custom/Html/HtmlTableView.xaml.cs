using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий таблицу.<br />
    /// Логика взаимодействия с HtmlTableView.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HtmlTableView : ContentView
    {
        /// <summary>
        /// Привязка: стиль ячейки таблицы.
        /// </summary>
        public static readonly BindableProperty CellStyleProperty =
            BindableProperty.Create(nameof(CellStyle), typeof(Style), typeof(HtmlTableView), null);

        /// <summary>
        /// Привязка: стиль пустой ячейки таблицы.
        /// </summary>
        public static readonly BindableProperty EmptyCellStyleProperty =
            BindableProperty.Create(nameof(EmptyCellStyle), typeof(Style), typeof(HtmlTableView), null);

        /// <summary>
        /// Привязка: стиль текста.
        /// </summary>
        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(HtmlTableView), null);

        /// <summary>
        /// Привязка: стиль жирного текста.
        /// </summary>
        public static readonly BindableProperty TextStyleBoldProperty =
            BindableProperty.Create(nameof(TextStyleBold), typeof(Style), typeof(HtmlTableView), null);



        /// <summary>
        /// Инициализирует таблицу и добавляет содержимое.
        /// </summary>
        /// <param name="cellsContent">Содержимое ячеек таблицы.</param>
        public HtmlTableView(View[,] cellsContent)
        {
            InitializeComponent();

            CellsContent = cellsContent;
            EnsureInitialization();
        }



        /// <summary>
        /// Количество строк таблицы.
        /// </summary>
        public int RowsCount => CellsContent.GetLength(0);

        /// <summary>
        /// Количество колонок таблицы.
        /// </summary>
        public int ColumnsCount => CellsContent.GetLength(1);

        /// <summary>
        /// Содержимое ячеек таблицы.
        /// </summary>
        public View[,] CellsContent { get; private set; }


        /// <summary>
        /// Стиль ячеек таблицы.
        /// </summary>
        public Style CellStyle
        {
            get => (Style)GetValue(CellStyleProperty);
            set => SetValue(CellStyleProperty, value);
        }

        /// <summary>
        /// Стиль пустых ячеек таблицы.
        /// </summary>
        public Style EmptyCellStyle
        {
            get => (Style)GetValue(EmptyCellStyleProperty);
            set => SetValue(EmptyCellStyleProperty, value);
        }

        /// <summary>
        /// Стиль текстовых элементов в ячейках таблицы.
        /// </summary>
        public Style TextStyle
        {
            get => (Style)GetValue(TextStyleProperty); 
            set => SetValue(TextStyleProperty, value);
        }

        /// <summary>
        /// Стиль заголовков в ячейках таблицы.
        /// </summary>
        public Style TextStyleBold
        {
            get => (Style)GetValue(TextStyleBoldProperty);
            set => SetValue(TextStyleBoldProperty, value);
        }



        /// <summary>
        /// Обеспечивает инициализацию таблици.
        /// </summary>
        private void EnsureInitialization()
        {
            // Если в таблица содержит не более трех столбцов, то она отображается.
            if (ColumnsCount <= 3)
                // Отображение таблицы.
                InitializeDefaultStyle();
            else
                // Отображение блока загрузки.
                CreateDownloadBlock();
        }


        /// <summary>
        /// Cоздает таблицу со стилем по умолчанию.
        /// </summary>
        private void InitializeDefaultStyle()
        {
            // Перебор содержимого ячеек.
            for (int i = 0; i < RowsCount; i++)
            {
                var tableRow = CreateTableRow();

                for (int j = 0; j < ColumnsCount; j++)
                {
                    var cell = CreateTableCell(CellsContent[i, j]);

                    // Установка колонки.
                    Grid.SetColumn(cell, j);

                    // Добавление ячейки в cтроку.
                    tableRow.Children.Add(cell);
                }

                // Добавление строки в таблицу.
                _table.Children.Add(tableRow);
            }
        }


        /// <summary>
        /// Добавляет блок для скачивания большой таблицы.
        /// </summary>
        private void CreateDownloadBlock()
        {
            // TODO: Добавить логику отображения.
        }


        /// <summary>
        /// Устанавливает связи для стилей элементов.
        /// </summary>
        /// <param name="view">Элмент представления.</param>
        private void SetStyle(View view)
        {
            view.BindingContext = this;

            // Присвение стилей текстовому элементу.
            if (view is Label)
            {
                if (((Label)view).FontAttributes == FontAttributes.Bold)
                    view.SetBinding(StyleProperty, nameof(TextStyleBold));
                else
                    view.SetBinding(StyleProperty, nameof(TextStyle));
            }
            // Присвение стилей ячейке таблицы.
            else if (view is HtmlTableCell)
            {
                if (((HtmlTableCell)view).HasChildren)
                    view.SetBinding(StyleProperty, nameof(CellStyle));
                else
                    view.SetBinding(StyleProperty, nameof(EmptyCellStyle));
            }
        }


        /// <summary>
        /// Создает строку с разделением на колонки.
        /// </summary>
        /// <returns>Строка таблицы.</returns>
        private Grid CreateTableRow()
        {
            var columnDefinitions = new ColumnDefinitionCollection();

            for (int j = 0; j < ColumnsCount; j++)
                columnDefinitions.Add(new ColumnDefinition());

            // Создание строки с колонками.
            return new Grid
            {
                ColumnDefinitions = columnDefinitions,
                Style = rowStyle
            };
        }


        /// <summary>
        /// Создает ячеку таблицы и добавляет в нее содержимое.
        /// </summary>
        /// <param name="cellContent">Содержимое ячейки.</param>
        /// <returns>Ячейка таблицы.</returns>
        private HtmlTableCell CreateTableCell(View? cellContent)
        {
            var cell = new HtmlTableCell();

            // Создание ячейки.
            if (cellContent != null)
            {
                // Установка стилей элемента интерфейса.
                SetStyle(cellContent);

                // Добавление элемента содержимого в ячейку.
                cell.AddView(cellContent);
            }

            // Установка стилей.
            SetStyle(cell);

            return cell;
        }
    }
}