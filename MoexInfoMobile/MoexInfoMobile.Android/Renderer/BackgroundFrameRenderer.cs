using Android.Content;
using MoexInfoMobile.Custom;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

namespace MoexInfoMobile.Droid.Renderer
{
    /// <summary>
    /// Рендерер фрейма фона.
    /// </summary>
    public class BackgroundFrameRenderer : FrameRenderer
    {
        public BackgroundFrameRenderer(Context context) : base(context) { }



        /// <summary>
        /// Переопределение метода, обрабатывающего событие "элемент изменен".
        /// </summary>
        /// <param name="e">Аргументы.</param>
        protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Frame> e)
        {
            base.OnElementChanged(e);
            SetBackgroundResource();
        }


        /// <summary>
        /// Переопределение метода, обрабатывающего событие "свойства элемента изменено".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы.</param>
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            SetBackgroundResource();
        }


        /// <summary>
        /// Установка ресурса-листа слоев отрисовки.
        /// </summary>
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