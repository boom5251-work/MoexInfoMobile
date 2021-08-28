using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public abstract class SecurityInfo
    {
        protected SecurityInfo(XmlNode rows)
        {
            /// Получение основных значений атрибутов title по значению атрибутов name.
            SecId = GetValueWithName(rows, "SECID");
            Name = GetValueWithName(rows, "NAME");
            ShortName = GetValueWithName(rows, "SHORTNAME");
            LatName = GetValueWithName(rows, "LATNAME");
            TypeName = GetValueWithName(rows, "TYPENAME");
            GroupName = GetValueWithName(rows, "GROUPNAME");
        }



        public string SecId { get; } /// Идентификатор ценной бумаги (строковый).
        public string Name { get; } /// Название ценной бумаги.
        public string ShortName { get; } /// Короткое название ценной бумаги.
        public string LatName { get; } /// Латинское название.
        public string TypeName { get; } /// Вид ценной бумаги.
        public string GroupName { get; } /// Тип ценной бумаги.



        // Метод возвращает элемент row по атрибуту name.
        protected static string GetValueWithName(XmlNode rows, string name)
        {
            try
            {
                for (int i = 0; i < rows.ChildNodes.Count; i++)
                {
                    XmlNode row = rows.ChildNodes[i];

                    if (row.Attributes["name"].Value == name)
                    {
                        return row.Attributes["value"].Value;
                    }
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
