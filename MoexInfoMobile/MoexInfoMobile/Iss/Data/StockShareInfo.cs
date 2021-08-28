using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class StockShareInfo : SecurityInfo
    {
        public StockShareInfo(XmlNode rows) : base(rows)
        {
            Isin = GetValueWithName(rows, "ISIN");

            string issueSize = GetValueWithName(rows, "ISSUESIZE");
            IssueSize = ulong.Parse(issueSize);

            string issueDate = GetValueWithName(rows, "ISSUEDATE");
            string matDate = GetValueWithName(rows, "MATDATE");
            string couponDate = GetValueWithName(rows, "COUPONDATE");
            string format = "yyyy-mm-dd";

            IssueDate = DateTime.ParseExact(issueDate, format, CultureInfo.InvariantCulture);
            MatDate = DateTime.ParseExact(matDate, format, CultureInfo.InvariantCulture);
            CouponDate = DateTime.ParseExact(couponDate, format, CultureInfo.InvariantCulture);

            InitialFaceValue = uint.Parse(GetValueWithName(rows, "INITIALFACEVALUE"));
            FaceValue = uint.Parse(GetValueWithName(rows, "FACEVALUE"));
            DayStoreDemition = uint.Parse(GetValueWithName(rows, "DAYSTOREDEMITION"));

            DayStoreDemition = ushort.Parse(GetValueWithName(rows, "COUPONFREQUENCY"));
            CouponPercent = double.Parse(GetValueWithName(rows, "COUPONFPERCENT"));
        }



        public string Isin { get; } /// Исин-код
        public ulong IssueSize { get; } /// Объем выпуска.

        public DateTime IssueDate { get; } /// Дата выпуска.
        public DateTime MatDate { get; } /// Дата погашения.
        public DateTime CouponDate { get; } /// Дата выплаты купона.

        public uint InitialFaceValue { get; } /// Первоначальная номинальная стоимость.
        public uint FaceValue { get; } /// Номиниальная стоимость.
        public uint DayStoreDemition { get; } /// Дней до погашения.
        public ushort CouponFrequency { get; } /// Переодичность выплаты купона в год.
        public double CouponPercent { get; } /// Ставка купона.
    }
}
