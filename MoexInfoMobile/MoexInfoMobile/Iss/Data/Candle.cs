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



        /// <summary>Цена открытия.</summary>
        public double Open { get; private set; }

        /// <summary>Цена закрытия.</summary>
        public double Close { get; private set; }

        /// <summary>Наибольшее значение.</summary>
        public double High { get; private set; }

        /// <summary>Наименьшее значение.</summary>
        public double Low { get; private set; }

        /// <summary>Объем бумаг.</summary>
        public uint Volume { get; private set; }

        /// <summary>Дата и время начала торгов.</summary>
        public DateTime Begin { get; private set; }

        /// <summary>Дата и ввремя окончания торгов.</summary>
        public DateTime End { get; private set; }



        /// <summary>
        /// Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        /// </summary>
        public static bool CanExtractFromNode(XmlNode row, out Candle candle)
        {
            try
            {
                // Извлечение стоимости.
                double open = GetNumber(row.Attributes["open"].Value);
                double close = GetNumber(row.Attributes["close"].Value);
                double high = GetNumber(row.Attributes["high"].Value);
                double low = GetNumber(row.Attributes["low"].Value);

                // Извлечение объема бумаг.
                uint volume = uint.Parse(row.Attributes["volume"].Value);

                // Извлечение дат начала и окончания торгов.
                string beginStr = row.Attributes["begin"].Value;
                string endStr = row.Attributes["end"].Value;
                string format = "yyyy-MM-dd HH:mm:ss";

                var begin = DateTime.ParseExact(beginStr, format, CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact(endStr, format, CultureInfo.InvariantCulture);

                candle = new Candle(open, close, high, low)
                {
                    Volume = volume,
                    Begin = begin,
                    End = end
                };

                return true;
            }
            catch (Exception)
            {
                candle = null;
                return false;
            }
        }


        /// <summary>
        /// Извлекает число с плавающей точкой из строки.
        /// </summary>
        /// <param name="strVal">Строка.</param>
        /// <returns>Число с плавающией точкой.</returns>
        /// <exception cref="FormatException">Строка не является числом.</exception>
        private static double GetNumber(string strVal)
        {
            if (!string.IsNullOrEmpty(strVal))
            {
                return double.Parse(strVal.Replace('.', ','));
            }
            else
            {
                throw new FormatException();
            }
        }
    }
}