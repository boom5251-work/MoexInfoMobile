using System;

namespace MoexInfoMobile.Html
{
    public abstract class HtmlNode : ICloneable
    {
        public string Name { get; set; } // Название узла.
        public string Value { get; set; } // Значение узла.

        public abstract HtmlNodeType NodeType { get; } // Тип узла.
                
        

        // Метод клонирует экземпляр.
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}