using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class StockBondInfo : SecurityInfo
    {
        public StockBondInfo(XmlNode rows) : base(rows)
        {
            Isin = GetValueWithName(rows, "ISIN");

            string issueSize = GetValueWithName(rows, "ISSUESIZE");
            IssueSize = ulong.Parse(issueSize);

            string issueDate = GetValueWithName(rows, "ISSUEDATE");
            IssueDate = DateTime.ParseExact(issueDate, "yyyy-mm-dd", CultureInfo.InvariantCulture);

            string path = $"http://invest-brands.cdn-tinkoff.ru/{ Isin }x160.png"; /// Ресурс с тинькофф-инвестиций (160x160).
            BrandLogoUri = new Uri(path);
        }



        public string Isin { get; } /// Исин-код
        public ulong IssueSize { get; } /// Объем выпуска.
        public DateTime IssueDate { get; } /// Дата выпуска.
        public Uri BrandLogoUri { get; } /// Путь к интернет-ресурсу с логотипом.
    }
}
