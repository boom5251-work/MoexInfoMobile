using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    public sealed class Candle
    {
        private Candle(double open, double close, double high, double low)
        {

            Open = open;
            Close = close;
            High = high;
            Low = low;
        }



        public double Open { get; private set; } /// Цена открытия.
        public double Close { get; private set; } /// Цена закрытия.
        public double High { get; private set;  } /// Наибольшее значение.
        public double Low { get; private set;  } /// Наименьшее значение.

        public uint Volume { get; private set; } /// Объем бумаг.

        public DateTime Begin { get; private set; } /// Дата и время начала торгов.
        public DateTime End { get; private set; } /// Дата и ввремя окончания торгов.



        // Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        public static bool CanExtractFromNode(XmlNode row, out Candle candle)
        {
            candle = null;

            try
            {
                /// Извлечение стоимости.
                double open = double.Parse(row.Attributes["open"].Value.Replace('.', ','));
                double close = double.Parse(row.Attributes["close"].Value.Replace('.', ','));
                double high = double.Parse(row.Attributes["high"].Value.Replace('.', ','));
                double low = double.Parse(row.Attributes["low"].Value.Replace('.', ','));

                /// Извлечение объема бумаг.
                uint volume = uint.Parse(row.Attributes["volume"].Value);

                /// Извлечение дат начала и окончания торгов.
                string beginStr = row.Attributes["begin"].Value;
                string endStr = row.Attributes["end"].Value;
                string format = "yyyy-MM-dd HH:mm:ss";

                DateTime begin = DateTime.ParseExact(beginStr, format, CultureInfo.InvariantCulture);
                DateTime end = DateTime.ParseExact(endStr, format, CultureInfo.InvariantCulture);

                candle = new Candle(open, close, high, low)
                {
                    Volume = volume,
                    Begin = begin,
                    End = end
                };

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
