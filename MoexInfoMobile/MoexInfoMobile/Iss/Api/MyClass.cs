using System;

namespace TestNamespace
{
    public class MyClass
    {
        public MyClass()
        {
            // ...
        }

        public string Info { get; private set; } // Основной текст.
        public string Start { get; private set; } // Начало в 19:00, 1 занятие.
        public Uri RegistrationLink { get; private set; } // Ссылка на регистрацию.
        public string PhoneNumber { get; private set; } // Номер телефона.
    }
}
