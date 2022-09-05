using Xamarin.Forms;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий неупорядоченный список список.
    /// </summary>
    public sealed class HtmlUnorderedListView : HtmlListView
    {
        public override void InitializeList(string[] values)
        {
            foreach (string text in values)
            {
                Frame marker = CreateMarker() as Frame;
                AddRow(marker, text);
            }
        }


        protected override View CreateMarker()
        {
            Frame pointMarker = new Frame
            {
                BindingContext = this
            };

            pointMarker.SetBinding(StyleProperty, nameof(MarkerStyle));
            return pointMarker;
        }
    }
}
