using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Table : ContentView
    {
        public Table()
        {
            InitializeComponent();
        }



        public int RowsCount { get; private set; }
        public int ColumnsCount { get; private set; }

        public View[,] CellsContent { get; private set; }


        // Стиль ячеек таблицы.
        public Style CellStyle
        {
            get { return (Style)GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
        }

        public static readonly BindableProperty CellStyleProperty =
            BindableProperty.Create(nameof(CellStyle), typeof(Style), typeof(Table), null);


        // Стиль пустых ячеек таблицы.
        public Style EmptyCellStyle
        {
            get { return (Style)GetValue(EmptyCellStyleProperty); }
            set { SetValue(EmptyCellStyleProperty, value); }
        }

        public static readonly BindableProperty EmptyCellStyleProperty =
            BindableProperty.Create(nameof(EmptyCellStyle), typeof(Style), typeof(Table), null);


        // Стиль текстовых элементов в ячейках таблицы.
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(Table), null);



        // Метод инициализирует таблицу.
        public void InitializeTable(View[,] cellsContent)
        {
            CellsContent = cellsContent;
            RowsCount = CellsContent.GetLength(0);
            ColumnsCount = CellsContent.GetLength(1);

            /// Если в таблица содержит не более трех столбцов, то она отображается.
            if (ColumnsCount <= 3)
            {
                CreateDefaultStyle(); /// Отображение таблицы.
            }
            else
            {
                CreateDownloadBlock(); /// Отображение блока загрузки.
            }
        }



        // Метод создает таблицу.
        private void CreateDefaultStyle()
        {
            /// Перебор содержимого ячеек.
            for (int i = 0; i < RowsCount; i++)
            {
                /// Создание строки с колонками.
                Grid tableRow = new Grid
                {
                    ColumnDefinitions = GetColumnDefinitions(),
                    Style = rowStyle
                };

                for (int j = 0; j < ColumnsCount; j++)
                {
                    TableCell cell = new TableCell();
                    /// Создание ячеек.
                    if (CellsContent[i, j] != null)
                    {
                        View view = CellsContent[i, j];
                        /// Установка стилей элемента интерфейса.
                        SetStyle(view);
                        /// Добавление элемента в ячейку.
                        cell.AddView(view);
                    }

                    SetStyle(cell); /// Установка стилей.
                    Grid.SetColumn(cell, j); /// Установка колонки.
                    tableRow.Children.Add(cell); /// Добавление ячейки в троку.
                }

                /// Добавление строки в таблицу.
                _table.Children.Add(tableRow);
            }
        }



        // 
        private void CreateDownloadBlock()
        {
            // TODO: Добавить логику отображения.
        }



        // Метод устанавливает связи для стилей элементов.
        private void SetStyle(View view)
        {
            view.BindingContext = this;

            if (view is Label)
            {
                view.SetBinding(StyleProperty, nameof(TextStyle));
            }
            else if (view is TableCell)
            {
                if ((view as TableCell).HasChildren)
                {
                    view.SetBinding(StyleProperty, nameof(CellStyle));
                }
                else
                {
                    view.SetBinding(StyleProperty, nameof(EmptyCellStyle));
                }
            }
        }



        // Метод создает разделение на колонки.
        private ColumnDefinitionCollection GetColumnDefinitions()
        {
            ColumnDefinitionCollection columnDefinitions = new ColumnDefinitionCollection();

            for (int j = 0; j < ColumnsCount; j++)
            {
                columnDefinitions.Add(new ColumnDefinition());
            }

            return columnDefinitions;
        }
    }
}