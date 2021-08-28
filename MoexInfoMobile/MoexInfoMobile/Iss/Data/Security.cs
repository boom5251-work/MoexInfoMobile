using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public class Security
    {
        private Security(string shortName, string secId)
        {
            ShortName = shortName;
            SecId = secId;
        }


        protected Security(Security security)
        {
            // TODO: Реализвать конструктор.
        }



        public string ShortName { get; } /// Короткое название ценной бумаги.
        public string SecId { get; } /// Идентификатор ценной бумаги (строковый).

        public bool IsTraded { get; private set; } /// Булево значение: торгуется ли бумага.

        public string SecurityGroup { get; private set; } /// Группа ценной бумаги.



        // Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        public static bool CanExtractFromNode(XmlNode row, out Security security)
        {
            security = null;

            try
            {
                string groupStr = row.Attributes["group"].Value; /// Значение атрибута "группа (тип) ценных бумаг".

                /// Если тип ценной бумаги соответсвует необходимым, то row разбирается.
                if (groupStr == "futures_forts" || groupStr == "stock_bonds" || groupStr == "stock_shares")
                {
                    string shortName = row.Attributes["shortname"].Value; /// Получение короткого названия ценной бумаги.
                    string secId = row.Attributes["secid"].Value.ToUpper(); /// Получение идентификатора ценной бумаги (строкового).

                    /// Извлечение булева значения: тооргуется ли бумага.
                    string isTradedStr = row.Attributes["is_traded"].Value;
                    bool isTraded;

                    /// Если единица, то торгуется.
                    if (isTradedStr == "1")
                    {
                        isTraded = true;
                    }
                    else
                    {
                        isTraded = false;
                    }

                    /// Извлечение группы ценной бумаги.
                    string securityGroup = row.Attributes["group"].Value;

                    /// Создание экземпляра класса ценной бкмаги.
                    security = new Security(shortName, secId)
                    {
                        IsTraded = isTraded,
                        SecurityGroup = securityGroup
                    };
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
