using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    public sealed class HtmlNumberedListView : HtmlListView
    {
        public char MarkerCharacter { get; set; } // Сивол, стоящий после номера элемента.


        // Метод инициализирует список.
        public override void InitializeList(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                Label marker = CreateMarker() as Label;
                marker.Text = $"{i}{MarkerCharacter}";
                AddRow(marker, values[i]);
            }
        }


        // Метод создает маркер.
        protected override View CreateMarker()
        {
            Label marker = CreateMarker() as Label;
            marker.BindingContext = this;
            marker.SetBinding(StyleProperty, nameof(MarkerStyle));
            return marker;
        }
    }
}