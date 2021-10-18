using System.Collections;
using System.Collections.Generic;

namespace MoexInfoMobile.Html
{
    public class HtmlElement : HtmlNode, IEnumerable
    {
        public HtmlElement(string name)
        {
            Name = name;
            ChildNodes = new List<HtmlNode>();
            Attributes = new List<HtmlNode>();
        }


        public List<HtmlNode> ChildNodes { get; private set; } // Дочерние элементы.
        public List<HtmlNode> Attributes { get; private set; } // Атрибуты.


        // Наличие дочерних узлов.
        public bool HasChildNodes
        {
            get
            {
                if (ChildNodes != null && ChildNodes.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        // Тип узла.
        public override HtmlNodeType NodeType
        {
            get { return HtmlNodeType.Element; }
        }


        // Получение и замена элементов по индексу.
        public HtmlNode this[int index]
        {
            get { return ChildNodes[index]; }
            set { ChildNodes[index] = value; }
        }


        
        // Метод добавляет дочерний элемент.
        public void AppendChild(HtmlNode childNode)
        {
            ChildNodes.Add(childNode);
        }


        // Метод добавляет дочерний элемент в начало.
        public void PrependChild(HtmlNode childNode)
        {
            ChildNodes.Insert(0, childNode);
        }


        // Метод вставляет новый дочерний элемент перед существующим.
        public bool InsertBefore(HtmlNode newChild, HtmlNode refChild)
        {
            if (ChildNodes.Contains(refChild))
            {
                int index = ChildNodes.IndexOf(refChild);
                ChildNodes.Insert(index, newChild);
                return true;
            }
            else
            {
                return false;
            }
        }


        // Метод вставляет новый дочерний элемент после существуещего.
        public bool InsertAfter(HtmlNode newChild, HtmlNode refChild)
        {
            if (ChildNodes.Contains(refChild))
            {
                int index = ChildNodes.IndexOf(refChild) + 1;
                ChildNodes.Insert(index, newChild);
                return true;
            }
            else
            {
                return false;
            }
        }


        // Метод возвращает атрибут по имени.
        public HtmlAttribute GetAttibuteNode(string attributeName)
        {
            HtmlNode node = ChildNodes.Find(attr => attr.Name == attributeName && attr.NodeType == HtmlNodeType.Attribute);
            return node as HtmlAttribute;
        }


        // Метод добавляет атрибут или изменяет его значение.
        public void SetAttributeNode(string name, string value)
        {
            HtmlAttribute attribute = GetAttibuteNode(name);

            if (attribute == null)
                Attributes.Add(new HtmlAttribute(name, value));
            else
                attribute.Value = value;
        }


        // Метод удаляет атрибут по имени.
        public void RemoveAttributeNode(string attributeName)
        {
            HtmlAttribute attribute = GetAttibuteNode(attributeName);
            Attributes.Remove(attribute);
        }


        // Метод возвращает объект Emumerator.
        public IEnumerator GetEnumerator()
        {
            return ChildNodes.GetEnumerator();
        }
    }
}
