using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class SecurityInfo
    {
        private SecurityInfo(string name, string shortName, string secId)
        {
            Name = name;
            ShortName = shortName;
            SecId = secId;
        }



        public string SecId { get; } /// Идентификатор ценной бумаги (строковый).
        public string Name { get; } /// Название ценной бумаги.
        public string ShortName { get; } /// Короткое название ценной бумаги.

        public string LatName { get; private set; } /// Латинское название.
        public string Isin { get; private set; } /// Исин код.
        public string RegNumber { get; private set; } /// Номер гос. регистрации.
        public string TypeName { get; private set; } /// Вид ценной бумаги.
        public string GroupName { get; private set; } /// Тип ценной бумаги.

        public ulong IssueSize { get; private set; } /// Объем выпуска.
        public DateTime IssueDate { get; private set; } /// Дата выпуска.



        // Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        public static bool CanExtractFromNode(XmlNode rows, out SecurityInfo securityInfo)
        {
            try
            {
                /// Получение необходимых значений атрибутов title по значению атрибутов name.
                string name = GetValueWithName(rows, "NAME");
                string shortName = GetValueWithName(rows, "SHORTNAME");
                string secId = GetValueWithName(rows, "SECID");

                ulong issueSize = ulong.Parse(GetValueWithName(rows, "ISSUESIZE"));

                /// Получение даты выпуска.
                string issueDateStr = GetValueWithName(rows, "ISSUEDATE");
                string format = "yyyy-mm-dd";
                DateTime issueDate = DateTime.ParseExact(issueDateStr, format, CultureInfo.InvariantCulture);

                /// Создание экземпляра класса информации о ценной бумаге.
                securityInfo = new SecurityInfo(name, shortName, secId)
                {
                    LatName = GetValueWithName(rows, "LATNAME"),
                    Isin = GetValueWithName(rows, "ISIN"),
                    RegNumber = GetValueWithName(rows, "REGNUMBER"),
                    TypeName = GetValueWithName(rows, "TYPENAME"),
                    GroupName = GetValueWithName(rows, "GROUPNAME"),

                    IssueSize = issueSize,
                    IssueDate = issueDate
                };
            }
            catch
            {
                securityInfo = null;
                return false;
            }

            return true;
        }



        // Метод возвращает элемент row по атрибуту name.
        private static string GetValueWithName(XmlNode rows, string name)
        {
            for (int i = 0; i < rows.ChildNodes.Count; i++)
            {
                XmlNode row = rows.ChildNodes[i];

                if (row.Attributes["name"].Value == name)
                {
                    return row.Attributes["value"].Value;
                }
            }

            return string.Empty;
        }
    }
}
