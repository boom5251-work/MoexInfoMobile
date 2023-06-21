using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Фьючерсный контракт.
    /// </summary>
    public sealed class FuturesFortInfo : SecurityInfo
    {
        public FuturesFortInfo(XmlNode rows) : base(rows)
        {
            string lastTrade = GetValueWithName(rows, "LASTDATE");
            string lastDelDte = GetValueWithName(rows, "LASTDELDATE");

            LastTrade = DateTime.ParseExact(lastTrade, DateTimePatterns.IssDateFormat, CultureInfo.InvariantCulture);
            LastDelDate = DateTime.ParseExact(lastDelDte, DateTimePatterns.IssDateFormat, CultureInfo.InvariantCulture);

            AssetCode = GetValueWithName(rows, "ASSETCODE");
        }



        /// <summary>
        /// Последний торговый день.
        /// </summary>
        public DateTime LastTrade { get; }

        /// <summary>
        /// День исполнения.
        /// </summary>
        public DateTime LastDelDate { get; }

        /// <summary>
        /// Код базового актива.
        /// </summary>
        public string AssetCode { get; }
    }
}