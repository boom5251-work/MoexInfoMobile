using System;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class Security
    {
        private Security(string shortName, string secId)
        {
            ShortName = shortName;
            SecId = secId;
        }



        /// <summary>Короткое название ценной бумаги.</summary>
        public string ShortName { get; } = string.Empty;

        /// <summary>Идентификатор ценной бумаги (строковый).</summary>
        public string SecId { get; } = string.Empty;

        /// <summary>ISIN-код.</summary>
        public string IsinCode { get; private set; } = string.Empty;

        /// <summary>Тип ценной бумаги.</summary>
        public string SecurityGroup { get; private set; } = string.Empty;

        /// <summary>Булево значение: торгуется ли бумага.</summary>
        public bool IsTraded { get; private set; }



        ///<summary>
        /// Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        /// </summary> 
        /// <param name="row">Объект ценной бумаги.</param>
        /// <param name="security">Ценная бумага.</param>
        /// <returns>True, если удалось получить данные. Fasle - нет.</returns>
        public static bool CanExtractFromNode(XmlNode row,  out Security security)
        {
            try
            {
                // Значение атрибута "группа (тип) ценных бумаг".
                string groupStr = row.Attributes["group"].Value;

                // Получение короткого названия ценной бумаги.
                string shortName = row.Attributes["shortname"].Value;
                // Получение идентификатора ценной бумаги (строкового).
                string secId = row.Attributes["secid"].Value.ToUpper(); 

                // Извлечение булева значения: тооргуется ли бумага.
                int isTradedNum = int.Parse(row.Attributes["is_traded"].Value);
                bool isTraded = Convert.ToBoolean(isTradedNum);

                // Создание экземпляра класса ценной бкмаги.
                security = new Security(shortName, secId)
                {
                    IsTraded = isTraded,
                    IsinCode = row.Attributes["isin"].Value,
                    SecurityGroup = row.Attributes["group"].Value
                };

                return true;
            }
            catch
            {
                security = null;
                return false;
            }
        }
    }
}