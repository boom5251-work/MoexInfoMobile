using Android.Content;
using MoexInfoMobile.Custom;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace MoexInfoMobile.Droid.Renderer
{
    public class BackgroundFrameRenderer : FrameRenderer
    {
        public BackgroundFrameRenderer(Context context) : base(context) { }


        // Переопределение метода, обрабатывающаего событие "элемент изменен".
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            SetBackgroundResource();
        }


        // Переопределение метода, обрабатывающего событие "свойства элемента изменено".
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetBackgroundResource();
        }


        // Установка ресурса-листа слоев отрисовки.
        private void SetBackgroundResource()
        {
            if (Element is BackgroundFrame)
            {
                BackgroundResource resource = (Element as BackgroundFrame).BackgroundResource;

                switch (resource)
                {
                    case BackgroundResource.BackgroundFrameClassicChalk:
                        SetBackgroundResource(Resource.Drawable.background_frame_white);
                        break;
                }
            }
        }
    }
}