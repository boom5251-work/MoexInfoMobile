using System;

namespace TestNamespace
{
    public class MyClass
    {
        public MyClass()
        {
            // TODO: Реализовать.
        }



        /// <summary>Основной текст.</summary>
        public string Info { get; private set; } = string.Empty;

        /// <summary>Например: Начало в 19:00, 1 занятие.</summary>
        public string Start { get; private set; } = string.Empty;

        /// <summary>Ссылка на регистрацию.</summary>
        public Uri RegistrationLink { get; private set; }

        /// <summary>Номер телефона.</summary>
        public string PhoneNumber { get; private set; } = string.Empty;
    }
}