using MoexInfoMobile.Iss.Data;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeadlineView : ContentView
    {
        public HeadlineView(Headline headline)
        {
            InitializeComponent();

            /// Установка заголовка и идентификатора.
            headlineId = headline.Id;
            titleLabel.Text = headline.Title;

            /// Установка даты публикации.
            string format = "dd.MM.yyyy HH:mm";
            dateLabel.Text = headline.PublishedAt.ToString(format, CultureInfo.InvariantCulture);

            /// Обработка события нажатия на Frame.
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                Tapped.Invoke(this, headlineId);
            };
        }


        private long headlineId; // Идентификатор новости.


        // Событие нажатия на элемент.
        public delegate void HeadlineViewTapped(HeadlineView sender, long headlineId);
        public event HeadlineViewTapped Tapped;
    }
}