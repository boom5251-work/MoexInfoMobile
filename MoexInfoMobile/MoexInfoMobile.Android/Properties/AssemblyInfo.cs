using System.Reflection;
using System.Runtime.InteropServices;
using Android.App;
using MoexInfoMobile.Custom;
using MoexInfoMobile.Droid.Renderer;
using Xamarin.Forms;

// Основная информация.
[assembly: AssemblyTitle("MoexInfoMobile.Android")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MoexInfoMobile.Android")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]

// Версии:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

// Разрешения.
[assembly: UsesPermission(Android.Manifest.Permission.Internet)]
[assembly: UsesPermission(Android.Manifest.Permission.WriteExternalStorage)]

// Рендереры.
[assembly: ExportRenderer(typeof(BackgroundFrame), typeof(BackgroundFrameRenderer))]
[assembly: ExportRenderer(typeof(EditableBarScrollView), typeof(EditableBarScrollViewRenderer))]
