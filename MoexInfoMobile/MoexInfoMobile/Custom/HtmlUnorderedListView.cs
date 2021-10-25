using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    public sealed class HtmlUnorderedListView : HtmlListView
    {
        // Метод инициализирует список.
        public override void InitializeList(string[] values)
        {
            foreach (string text in values)
            {
                Frame marker = CreateMarker() as Frame;
                AddRow(marker, text);
            }
        }


        // Метод создает маркер.
        protected override View CreateMarker()
        {
            Frame pointMarker = new Frame();
            pointMarker.BindingContext = this;
            pointMarker.SetBinding(StyleProperty, nameof(MarkerStyle));
            return pointMarker;
        }
    }
}
