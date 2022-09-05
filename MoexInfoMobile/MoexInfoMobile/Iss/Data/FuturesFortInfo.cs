using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class FuturesFortInfo : SecurityInfo
    {
        public FuturesFortInfo(XmlNode rows) : base(rows)
        {
            try
            {
                string lastTrade = GetValueWithName(rows, "LASTDATE");
                string lastDelDte = GetValueWithName(rows, "LASTDELDATE");
                string format = "yyyy-mm-dd";

                LastTrade = DateTime.ParseExact(lastTrade, format, CultureInfo.InvariantCulture);
                LastDelDate = DateTime.ParseExact(lastDelDte, format, CultureInfo.InvariantCulture);

                AssetCode = GetValueWithName(rows, "ASSETCODE");
            }
            catch { }
        }



        /// <summary>Последний торговый день.</summary>
        public DateTime LastTrade { get; }

        /// <summary>День исполнения.</summary>
        public DateTime LastDelDate { get; }

        /// <summary>Код базового актива.</summary>
        public string AssetCode { get; } = string.Empty;
    }
}