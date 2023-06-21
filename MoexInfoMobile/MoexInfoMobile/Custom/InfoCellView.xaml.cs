using MoexInfoMobile.Iss.Data;
using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Информационная ячейка-кнопка.<br />
    /// Логика взаимодействия с InfoCellView.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoCellView : ContentView, ICellView
    {
        ///<summary>
        ///Событие нажатия на элемент.
        ///</summary>
        public event CellViewTapped? Tapped;



        /// <summary>
        /// Конструктор ячейки информации новостного заголовка.
        /// </summary>
        /// <param name="headline">Новостной заголовок.</param>
        public InfoCellView(Headline headline)
        {
            InitializeComponent();

            // Установка заголовка и идентификатора.
            ulong headlineId = headline.Id;
            string headlineTitle = headline.Title;
            string dateStr = headline.PublishedAt.ToString(DateTimePatterns.DateTimeFormat, CultureInfo.InvariantCulture);

            InitializeView(headlineId, headlineTitle, dateStr);
        }


        /// <summary>
        /// Конструктор ячейки информации события.
        /// </summary>
        /// <param name="siteEvent">Событие.</param>
        public InfoCellView(Event siteEvent)
        {
            InitializeComponent();

            // Установка заголовка и идентификатора.
            ulong eventId = siteEvent.Id;
            string eventTitle = siteEvent.Title;
            string from = siteEvent.From.ToString(DateTimePatterns.DateTimeFormat, CultureInfo.InvariantCulture);
            string dateStr = $"Начало: {from}";

            InitializeView(eventId, eventTitle, dateStr);
        }



        /// <summary>
        /// Идентификатор новости или события.
        /// </summary>
        public ulong ItemId { get; private set; }

        

        /// <summary>
        /// Инициализирует значения.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="title">Заголовок</param>
        /// <param name="dateStr">Строка даты и времени.</param>
        private void InitializeView(ulong id, string title, string dateStr)
        {
            // Установка значений.
            ItemId = id;
            titleLabel.Text = title;
            dateLabel.Text = dateStr;

            // Обработка события нажатия на Frame.
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                Tapped?.Invoke(this);
        }
    }
}