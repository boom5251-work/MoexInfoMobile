using Android.Content;
using MoexInfoMobile.Custom;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

namespace MoexInfoMobile.Droid.Renderer
{
    /// <summary>
    /// Рендерер пользовательского элемента прокрутки.
    /// </summary>
    public class EditableBarScrollViewRenderer : ScrollViewRenderer
    {
        public EditableBarScrollViewRenderer(Context context) : base(context) { }



        /// <summary>
        /// Переопределение метода, обрабатывающаего событие "элемент изменен".
        /// </summary>
        /// <param name="e">Аргументы.</param>
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            
            if (e.OldElement != null)
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }


        /// <summary>
        /// Метод обрабатывает событие "свойство элемента изменено".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы.</param>
        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ChildCount > 0 && Element is EditableBarScrollView)
            {
                var scrollView = Element as EditableBarScrollView;

                // Горизонтальный ползунок.
                if (!scrollView.HorizontalBarEnabled)
                    GetChildAt(0).HorizontalScrollBarEnabled = false;
                else
                    GetChildAt(0).HorizontalScrollBarEnabled = true;

                // Вертикальный ползунок.
                if (!scrollView.HorizontalBarEnabled)
                    GetChildAt(0).VerticalScrollBarEnabled = false;
                else
                    GetChildAt(0).VerticalScrollBarEnabled = true;
            }
        }
    }
}