using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Parse;

namespace SweatUp.Droid
{
	[Activity (Theme = "@style/Theme.Splash", Label = "SweatUp", Icon = "@drawable/icon", NoHistory = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class Splash : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			StartActivity (typeof(MainActivity));
		}
	}
}

