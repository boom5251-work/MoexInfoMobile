using MoexInfoMobile.Iss.Data;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InfoCellView : ContentView
    {
        public InfoCellView(Headline headline)
        {
            InitializeComponent();

            /// Установка заголовка и идентификатора.
            ulong headlineId = headline.Id;
            string headlineTitle = headline.Title;
            string dateStr = headline.PublishedAt.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            InitializeView(headlineId, headlineTitle, dateStr);
        }

        public InfoCellView(Event siteEvent)
        {
            InitializeComponent();

            /// Установка заголовка и идентификатора.
            ulong eventId = siteEvent.Id;
            string eventTitle = siteEvent.Title;
            string dateStr = $"Дата начала: {siteEvent.From.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}";

            InitializeView(eventId, eventTitle, dateStr);
        }



        private ulong ItemId { get; set; } // Идентификатор новости.


        // Событие нажатия на элемент.
        public delegate void InfoCellViewTapped(InfoCellView sender, ulong id);
        public event InfoCellViewTapped Tapped;



        // Метод инициализирует значения.
        private void InitializeView(ulong id, string title, string dateStr)
        {
            /// Установка значений.
            ItemId = id;
            titleLabel.Text = title;
            dateLabel.Text = dateStr;

            /// Обработка события нажатия на Frame.
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                Tapped.Invoke(this, ItemId);
            };
        }
    }
}