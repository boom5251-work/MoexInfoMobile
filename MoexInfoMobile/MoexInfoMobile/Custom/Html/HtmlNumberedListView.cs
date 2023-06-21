using Xamarin.Forms;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий нумерованный список.
    /// </summary>
    public sealed class HtmlNumberedListView : HtmlListView
    {
        /// <summary>
        /// Символ, стоящий после номера элемента.
        /// </summary>
        public char MarkerCharacter { get; set; }



        public override void InitializeList(string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                var marker = (Label)CreateMarker();
                marker.Text = $"{i + 1}{MarkerCharacter}";

                AddRow(marker, values[i]);
            }
        }

        
        protected override View CreateMarker()
        {
            Label marker = new Label
            {
                BindingContext = this
            };

            marker.SetBinding(StyleProperty, nameof(MarkerStyle));
            return marker;
        }
    }
}