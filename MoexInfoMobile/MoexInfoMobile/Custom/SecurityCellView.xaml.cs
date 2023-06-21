using MoexInfoMobile.Iss.Data;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Ячейка-кнопка ценной бумаги.<br />
    /// Логика взаимодействия с SecurityCellView.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SecurityCellView : ContentView, ICellView
    {
        public SecurityCellView(Security security)
        {
            InitializeComponent();

            // Установка значений.
            shortNameLabel.Text = security.ShortName;
            secIdLabel.Text = security.SecId;

            SecId = security.SecId ?? string.Empty;

            // Обработка события нажатия на Frame.
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                Tapped?.Invoke(this);
        }



        /// <summary>
        /// Строковый идентификатор ценной бумаги.
        /// </summary>
        public string SecId { get; private set; }


        /// <summary>
        /// Событие нажатия на элемент.
        /// </summary>
        public event CellViewTapped? Tapped;
    }
}