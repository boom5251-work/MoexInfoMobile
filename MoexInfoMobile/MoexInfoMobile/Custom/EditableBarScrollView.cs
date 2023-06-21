using Xamarin.Forms;

namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Пользовательское представление прокрутки.
    /// </summary>
    public class EditableBarScrollView : ScrollView
    {
        /// <summary>
        /// Привязка: видим ли горизонтальный ползунок.
        /// </summary>
        public static readonly BindableProperty HorizontalBarEnabledProperty =
            BindableProperty.Create(nameof(HorizontalBarEnabled), typeof(bool), typeof(EditableBarScrollView), true);

        /// <summary>
        /// Привязка: видим ли вертикальный ползунок.
        /// </summary>
        public static readonly BindableProperty VerticalBarEnabledProperty =
            BindableProperty.Create(nameof(VerticalBarEnabled), typeof(bool), typeof(EditableBarScrollView), true);



        /// <summary>
        /// Видим ли горизонтальный ползунок.
        /// </summary>
        public bool HorizontalBarEnabled
        {
            get => (bool)GetValue(HorizontalBarEnabledProperty);
            set => SetValue(HorizontalBarEnabledProperty, value);
        }

        /// <summary>
        /// Видим ли вертикальный ползунок.
        /// </summary>
        public bool VerticalBarEnabled
        {
            get => (bool)GetValue(VerticalBarEnabledProperty);
            set => SetValue(VerticalBarEnabledProperty, value);
        }        
    }
}