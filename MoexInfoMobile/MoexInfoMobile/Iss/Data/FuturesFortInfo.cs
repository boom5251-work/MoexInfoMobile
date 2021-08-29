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



        public DateTime LastTrade { get; } /// Последний торговый день.
        public DateTime LastDelDate { get; } /// День исполнения.
        public string AssetCode { get; } /// Код базового актива.
    }
}
