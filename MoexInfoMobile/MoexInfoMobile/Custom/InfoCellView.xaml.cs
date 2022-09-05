using MoexInfoMobile.Iss.Data;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Информационная ячейка-кнопка.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoCellView : ContentView, ICellView
    {
        public InfoCellView(Headline headline)
        {
            InitializeComponent();

            // Установка заголовка и идентификатора.
            ulong headlineId = headline.Id;
            string headlineTitle = headline.Title;
            string dateStr = headline.PublishedAt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            InitializeView(headlineId, headlineTitle, dateStr);
        }

        public InfoCellView(Event siteEvent)
        {
            InitializeComponent();

            // Установка заголовка и идентификатора.
            ulong eventId = siteEvent.Id;
            string eventTitle = siteEvent.Title;
            string dateStr = $"Дата начала: {siteEvent.From.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";

            InitializeView(eventId, eventTitle, dateStr);
        }



        /// <summary>Идентификатор новости или события.</summary>
        public ulong ItemId { get; private set; }



        ///<summary>Событие нажатия на элемент.</summary>
        public event CellViewTapped Tapped;



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
            {
                Tapped.Invoke(this);
            };
        }
    }
}