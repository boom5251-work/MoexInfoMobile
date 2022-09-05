using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class StockShareInfo : SecurityInfo
    {
        public StockShareInfo(XmlNode rows) : base(rows)
        {
            try
            {
                Isin = GetValueWithName(rows, "ISIN");

                string issueSize = GetValueWithName(rows, "ISSUESIZE");
                IssueSize = ulong.Parse(issueSize);

                InitializeDates(rows);
                InitializeCost(rows);
                InitializeCoupon(rows);
            }
            catch { }
        }



        /// <summary>Исин-код.</summary>
        public string Isin { get; }

        /// <summary>Объем выпуска.</summary>
        public ulong IssueSize { get; }

        /// <summary>Дата выпуска.</summary>
        public DateTime IssueDate { get; private set; }

        /// <summary>Дата погашения.</summary>
        public DateTime MatDate { get; private set; }

        /// <summary>Дата выплаты купона.</summary>
        public DateTime CouponDate { get; private set; }

        /// <summary>Первоначальная номинальная стоимость.</summary>
        public uint InitialFaceValue { get; private set; }

        /// <summary>Номиниальная стоимость.</summary>
        public uint FaceValue { get; private set; }

        /// <summary>Дней до погашения.</summary>
        public uint DayStoreDemition { get; private set; }

        /// <summary>Переодичность выплаты купона в год.</summary>
        public ushort CouponFrequency { get; private set; }

        /// <summary>Ставка купона.</summary>
        public double CouponPercent { get; private set; }



        /// <summary>
        /// Инициализирует даты.
        /// </summary>
        /// <param name="rows">Объект ценной бумаги.</param>
        private void InitializeDates(XmlNode rows)
        {
            const string Format = "yyyy-mm-dd";

            string issueDate = GetValueWithName(rows, "ISSUEDATE");
            string matDate = GetValueWithName(rows, "MATDATE");
            string couponDate = GetValueWithName(rows, "COUPONDATE");

            IssueDate = DateTime.ParseExact(issueDate, Format, CultureInfo.InvariantCulture);
            MatDate = DateTime.ParseExact(matDate, Format, CultureInfo.InvariantCulture);
            CouponDate = DateTime.ParseExact(couponDate, Format, CultureInfo.InvariantCulture);
        }



        /// <summary>
        /// Инициализирует цену.
        /// </summary>
        /// <param name="rows">Объект ценной бумаги.</param>
        private void InitializeCost(XmlNode rows)
        {
            InitialFaceValue = uint.Parse(GetValueWithName(rows, "INITIALFACEVALUE"));
            FaceValue = uint.Parse(GetValueWithName(rows, "FACEVALUE"));
            DayStoreDemition = uint.Parse(GetValueWithName(rows, "DAYSTOREDEMITION"));
        }



        /// <summary>
        /// Инициализация купона.
        /// </summary>
        /// <param name="rows">Объект ценной бумаги.</param>
        private void InitializeCoupon(XmlNode rows)
        {
            CouponFrequency = ushort.Parse(GetValueWithName(rows, "COUPONFREQUENCY"));
            CouponPercent = double.Parse(GetValueWithName(rows, "COUPONFPERCENT"));
        }
    }
}