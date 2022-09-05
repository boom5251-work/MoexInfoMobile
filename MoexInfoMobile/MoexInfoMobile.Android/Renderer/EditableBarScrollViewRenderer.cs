using Android.Content;
using MoexInfoMobile.Custom;
using System.ComponentModel;
using Xamarin.Forms.Platform.Android;

namespace MoexInfoMobile.Droid.Renderer
{
    public class EditableBarScrollViewRenderer : ScrollViewRenderer
    {
        public EditableBarScrollViewRenderer(Context context) : base(context) { }


        // Переопределение метода, обрабатывающаего событие "элемент изменен".
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            
            if (e.OldElement != null)
            {
                e.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            e.NewElement.PropertyChanged += OnElementPropertyChanged;
        }


        // Метод обрабатывающет событие "свойства элемента изменено".
        protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (ChildCount > 0)
            {
                if (Element is EditableBarScrollView)
                {
                    EditableBarScrollView scrollView = Element as EditableBarScrollView;

                    /// Горизонтальный ползунок.
                    if (!scrollView.HorizontalBarEnabled)
                    {
                        GetChildAt(0).HorizontalScrollBarEnabled = false;
                    }
                    else
                    {
                        GetChildAt(0).HorizontalScrollBarEnabled = true;
                    }
                    /// Вертикальный ползунок.
                    if (!scrollView.HorizontalBarEnabled)
                    {
                        GetChildAt(0).VerticalScrollBarEnabled = false;
                    }
                    else
                    {
                        GetChildAt(0).VerticalScrollBarEnabled = true;
                    }
                }
            }
        }
    }
}