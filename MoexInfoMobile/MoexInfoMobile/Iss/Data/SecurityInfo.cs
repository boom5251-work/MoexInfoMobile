using System;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Информация ценной бумаги.
    /// </summary>
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



        /// <summary>
        /// Идентификатор ценной бумаги (строковый).
        /// </summary>
        public string SecId { get; }

        /// <summary>
        /// Название ценной бумаги.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Короткое название ценной бумаги.
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Латинское название.
        /// </summary>
        public string LatName { get; }

        /// <summary>
        /// Вид ценной бумаги.
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        /// Тип ценной бумаги.
        /// </summary>
        public string GroupName { get; }




        /// <summary>
        /// Метод возвращает элемент row по атрибуту name.
        /// </summary>
        /// <param name="rows">Объект ценной бумаги.</param>
        /// <param name="name">Название свойства.</param>
        /// <returns>Получения значения дочернего элемента по названию.</returns>
        protected string GetValueWithName(XmlNode rows, string name)
        {
            try
            {
                for (int i = 0; i < rows.ChildNodes.Count; i++)
                {
                    var row = rows.ChildNodes[i];

                    if (row.Attributes["name"].Value == name)
                        return row.Attributes["value"].Value;
                    else
                        throw new InvalidOperationException();
                }

                throw new InvalidOperationException();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}