using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class StockBondInfo : SecurityInfo
    {
        public StockBondInfo(XmlNode rows) : base(rows)
        {
            try
            {
                Isin = GetValueWithName(rows, "ISIN");

                string issueSize = GetValueWithName(rows, "ISSUESIZE");
                IssueSize = ulong.Parse(issueSize);

                string issueDate = GetValueWithName(rows, "ISSUEDATE");
                IssueDate = DateTime.ParseExact(issueDate, "yyyy-mm-dd", CultureInfo.InvariantCulture);

                // Ресурс с тинькофф-инвестиций (160x160).
                string path = $"http://invest-brands.cdn-tinkoff.ru/{ Isin }x160.png";
                BrandLogoUri = new Uri(path);
            }
            catch { }
        }



        /// <summary>Исин-код.</summary>
        public string Isin { get; }

        /// <summary>Объем выпуска.</summary>
        public ulong IssueSize { get; }

        /// <summary>Дата выпуска.</summary>
        public DateTime IssueDate { get; }

        /// <summary>Путь к интернет-ресурсу с логотипом.</summary>
        public Uri BrandLogoUri { get; }
    }
}
