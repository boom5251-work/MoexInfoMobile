namespace MoexInfoMobile.StringPatterns
{
    /// <summary>
    /// Строковые паттерны даты и времени.
    /// </summary>
    public abstract class DateTimePatterns
    {
        /// <summary>
        /// Формат даты и времени в API ИСС.<br />
        /// yyyy-MM-dd HH:mm:ss.
        /// </summary>
        public static readonly string IssDateTimeFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// Формат даты и времени в API ИСС.<br />
        /// yyyy-mm-dd.
        /// </summary>
        public static readonly string IssDateFormat = "yyyy-mm-dd";

        /// <summary>
        /// Формат даты и времени.<br />
        /// dd.MM.yyyy HH:mm.
        /// </summary>
        public static readonly string DateTimeFormat = "dd.MM.yyyy HH:mm";
    }
}