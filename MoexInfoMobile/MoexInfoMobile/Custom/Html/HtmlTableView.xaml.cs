using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий таблицу.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HtmlTableView : ContentView
    {
        public HtmlTableView()
        {
            InitializeComponent();
        }



        /// <summary>Количество строк таблицы.</summary>
        public int RowsCount { get; private set; }

        /// <summary>Количество колонок таблицы.</summary>
        public int ColumnsCount { get; private set; }

        /// <summary>Содержимое ячеек таблицы.</summary>
        public View[,] CellsContent { get; private set; }


        /// <summary>Стиль ячеек таблицы.</summary>
        public Style CellStyle
        {
            get { return (Style)GetValue(CellStyleProperty); }
            set { SetValue(CellStyleProperty, value); }
        }

        public static readonly BindableProperty CellStyleProperty =
            BindableProperty.Create(nameof(CellStyle), typeof(Style), typeof(HtmlTableView), null);


        /// <summary>Стиль пустых ячеек таблицы.</summary>
        public Style EmptyCellStyle
        {
            get { return (Style)GetValue(EmptyCellStyleProperty); }
            set { SetValue(EmptyCellStyleProperty, value); }
        }

        public static readonly BindableProperty EmptyCellStyleProperty =
            BindableProperty.Create(nameof(EmptyCellStyle), typeof(Style), typeof(HtmlTableView), null);


        /// <summary>Стиль текстовых элементов в ячейках таблицы.</summary>
        public Style TextStyle
        {
            get { return (Style)GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public static readonly BindableProperty TextStyleProperty =
            BindableProperty.Create(nameof(TextStyle), typeof(Style), typeof(HtmlTableView), null);


        /// <summary>Стиль заголовков в ячейках таблицы.</summary>
        public Style TextStyleBold
        {
            get { return (Style)GetValue(TextStyleBoldProperty); }
            set { SetValue(TextStyleBoldProperty, value); }
        }

        public static readonly BindableProperty TextStyleBoldProperty =
            BindableProperty.Create(nameof(TextStyleBold), typeof(Style), typeof(HtmlTableView), null);




        /// <summary>
        /// Метод инициализирует таблицу.
        /// </summary>
        /// <param name="cellsContent">Содержимое ячеек таблицы.</param>
        public void InitializeTable(View[,] cellsContent)
        {
            CellsContent = cellsContent;
            RowsCount = CellsContent.GetLength(0);
            ColumnsCount = CellsContent.GetLength(1);

            // Если в таблица содержит не более трех столбцов, то она отображается.
            if (ColumnsCount <= 3)
            {
                // Отображение таблицы.
                CreateDefaultStyle();
            }
            else
            {
                // Отображение блока загрузки.
                CreateDownloadBlock();
            }
        }



        /// <summary>
        /// Метод создает таблицу.
        /// </summary>
        private void CreateDefaultStyle()
        {
            // Перебор содержимого ячеек.
            for (int i = 0; i < RowsCount; i++)
            {
                // Создание строки с колонками.
                Grid tableRow = new Grid
                {
                    ColumnDefinitions = GetColumnDefinitions(),
                    Style = rowStyle
                };

                for (int j = 0; j < ColumnsCount; j++)
                {
                    HtmlTableCell cell = new HtmlTableCell();

                    // Создание ячеек.
                    if (CellsContent[i, j] != null)
                    {
                        View view = CellsContent[i, j];

                        // Установка стилей элемента интерфейса.
                        SetStyle(view);

                        // Добавление элемента в ячейку.
                        cell.AddView(view);
                    }

                    SetStyle(cell); // Установка стилей.
                    Grid.SetColumn(cell, j); // Установка колонки.
                    tableRow.Children.Add(cell); // Добавление ячейки в троку.
                }

                // Добавление строки в таблицу.
                _table.Children.Add(tableRow);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        private void CreateDownloadBlock()
        {
            // TODO: Добавить логику отображения.
            throw new NotImplementedException();
        }



        /// <summary>
        /// Метод устанавливает связи для стилей элементов.
        /// </summary>
        /// <param name="view">Элмент представления.</param>
        private void SetStyle(View view)
        {
            view.BindingContext = this;

            if (view is Label)
            {
                if ((view as Label).FontAttributes == FontAttributes.Bold)
                {
                    view.SetBinding(StyleProperty, nameof(TextStyleBold));
                }
                else
                {
                    view.SetBinding(StyleProperty, nameof(TextStyle));
                }
            }
            else if (view is HtmlTableCell)
            {
                if ((view as HtmlTableCell).HasChildren)
                {
                    view.SetBinding(StyleProperty, nameof(CellStyle));
                }
                else
                {
                    view.SetBinding(StyleProperty, nameof(EmptyCellStyle));
                }
            }
        }



        /// <summary>
        /// Метод создает разделение на колонки.
        /// </summary>
        /// <returns>Коллекция разделений.</returns>
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