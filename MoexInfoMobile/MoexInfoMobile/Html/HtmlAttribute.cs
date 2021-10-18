namespace MoexInfoMobile.Html
{
    public class HtmlAttribute : HtmlNode
    {
        public HtmlAttribute(string name, string value)
        {
            Name = name;
            Value = value;
        }

        // Тип узла.
        public override HtmlNodeType NodeType
        {
            get { return HtmlNodeType.Attribute; }
        }
    }
}
