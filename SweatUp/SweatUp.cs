using System;

using Xamarin.Forms;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Android.Util;
using NotificationCompat = Android.Support.V4.App.NotificationCompat;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Parse;

namespace SweatUp
{
	public class App : Xamarin.Forms.Application
	{
		public double openedlat;
		public double openedlng;

		public App ()
		{

			MainPage = new HomeView();


		}


	}
}

