using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public abstract class SecurityInfo
    {
        protected SecurityInfo(XmlNode rows)
        {
            // Получение основных значений атрибутов title по значению атрибутов name.
            SecId = GetValueWithName(rows, "SECID");
            Name = GetValueWithName(rows, "NAME");
            ShortName = GetValueWithName(rows, "SHORTNAME");
            LatName = GetValueWithName(rows, "LATNAME");
            TypeName = GetValueWithName(rows, "TYPENAME");
            GroupName = GetValueWithName(rows, "GROUPNAME");
        }



        /// <summary>Идентификатор ценной бумаги (строковый).</summary>
        public string SecId { get; } = string.Empty;

        /// <summary>Название ценной бумаги.</summary>
        public string Name { get; } = string.Empty;

        /// <summary>Короткое название ценной бумаги.</summary>
        public string ShortName { get; } = string.Empty;

        /// <summary>Латинское название.</summary>
        public string LatName { get; } = string.Empty;

        /// <summary>Вид ценной бумаги.</summary>
        public string TypeName { get; } = string.Empty;

        /// <summary>Тип ценной бумаги.</summary>
        public string GroupName { get; } = string.Empty;




        /// <summary>
        /// Метод возвращает элемент row по атрибуту name.
        /// </summary>
        /// <param name="rows">Объект ценной бумаги.</param>
        /// <param name="name">Название свойства.</param>
        /// <returns></returns>
        protected static string GetValueWithName(XmlNode rows, string name)
        {
            try
            {
                for (int i = 0; i < rows.ChildNodes.Count; i++)
                {
                    var row = rows.ChildNodes[i];

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