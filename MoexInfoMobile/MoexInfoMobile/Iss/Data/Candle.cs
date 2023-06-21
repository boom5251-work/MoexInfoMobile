using MoexInfoMobile.StringPatterns;
using System;
using System.Globalization;
using System.Xml;

namespace MoexInfoMobile.Iss.Data
{
    /// <summary>
    /// Свеча.
    /// </summary>
    public sealed class Candle
    {
        /// <summary>
        /// Приватный конструктор свечи.
        /// </summary>
        /// <param name="open">Цена открытия.</param>
        /// <param name="close">Цена закрытия</param>
        /// <param name="high">Наибольшая цена.</param>
        /// <param name="low">Наименьшая цена.</param>
        private Candle(double open, double close, double high, double low)
        {
            Open = open;
            Close = close;
            High = high;
            Low = low;
        }



        /// <summary>
        /// Цена открытия.
        /// </summary>
        public double Open { get; }

        /// <summary>
        /// Цена закрытия.
        /// </summary>
        public double Close { get; }

        /// <summary>
        /// Наибольшая цена.
        /// </summary>
        public double High { get; }

        /// <summary>
        /// Наименьшая цена.
        /// </summary>
        public double Low { get; }

        /// <summary>
        /// Объем бумаг.
        /// </summary>
        public uint Volume { get; private set; }

        /// <summary>
        /// Дата и время начала торгов.
        /// </summary>
        public DateTime Begin { get; private set; }

        /// <summary>
        /// Дата и время окончания торгов.
        /// </summary>
        public DateTime End { get; private set; }



        /// <summary>
        /// Метод проверяет, возможно ли создать экземпляр данного класса на основе XmlNode.
        /// </summary>
        /// <param name="row">Строка XML.</param>
        /// <param name="candle">Свеча.</param>
        public static bool CanExtractFromNode(XmlNode row, out Candle? candle)
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

                var begin = DateTime.ParseExact(beginStr, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);
                var end = DateTime.ParseExact(endStr, DateTimePatterns.IssDateTimeFormat, CultureInfo.InvariantCulture);

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
        /// <returns>Число с плавающей точкой.</returns>
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