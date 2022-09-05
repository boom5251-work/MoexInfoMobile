using Xamarin.Forms;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий нумерованный список.
    /// </summary>
    public sealed class HtmlNumberedListView : HtmlListView
    {
        /// <summary>Символ, стоящий после номера элемента.</summary>
        public char MarkerCharacter { get; set; }



        public override void InitializeList(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var marker = CreateMarker() as Label;
                marker.Text = $"{i}{MarkerCharacter}";

                AddRow(marker, values[i]);
            }
        }


        
        protected override View CreateMarker()
        {
            Label marker = CreateMarker() as Label;
            marker.BindingContext = this;
            marker.SetBinding(StyleProperty, nameof(MarkerStyle));
            return marker;
        }
    }
}