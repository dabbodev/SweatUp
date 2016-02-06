using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Preferences;
using Android.Gms.Common;
using Parse;
using Android.Util;
using Android.Locations;
using Newtonsoft.Json;
using Xamarin.Payments.Stripe;

using Android.Support.V4.Content;
using Android.Support.V7.App;

[assembly: Permission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "@PACKAGE_NAME@.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: MetaData ("com.google.android.maps.v2.API_KEY", Value="AIzaSyA8Y3XezVhFMxG_mcvvxyWAnmfDkdWrPes")]
namespace SweatUp.Droid
{
	[Activity (Label = "SweatUp", Icon = "@drawable/icon", MainLauncher = true)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity, ILocationListener
	{
		// What we want from Google Play Services
		const int PLAY_SERVICES_RESOLUTION_REQUEST = 9000;
		// The reciever for GCM
		BroadcastReceiver mRegistrationBroadcastReceiver;
		// The manager for the GPS
		LocationManager locMgr;
		// Sets what provider for GPS (GPS, Wifi, Coarse)
		string locprovider; 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			// Google Cloud Messaging init
			mRegistrationBroadcastReceiver = new BroadcastReceiver ();
			mRegistrationBroadcastReceiver.Receive += (sender, e) => {
				var sharedPreferences =	PreferenceManager.GetDefaultSharedPreferences ((Context)sender);
				var sentToken = sharedPreferences.GetBoolean (QuickstartPreferences.SENT_TOKEN_TO_SERVER, false);
			};

			ParseClient.Configuration pconfig = new ParseClient.Configuration {
				ApplicationId = "avQAqGyX0JS2yOISV9Tp8QzQOjsIRN5gOimG3BXg",
				WindowsKey = "GdbIbFuxIHk87HOYzKN8pcucxksL0j4Fp8BIfT7N"
			};

			// Parse init (APPID, .NET Key)
			ParseClient.Initialize(pconfig);

			// init Xamarin.Forms for viewable objects
			global::Xamarin.Forms.Forms.Init (this, bundle);
			global::Xamarin.FormsMaps.Init (this, bundle);
			// Turn off the fucking title bar
			global::Xamarin.Forms.Forms.SetTitleBarVisibility (Xamarin.Forms.AndroidTitleBarVisibility.Never);

			// Start the GCM registration service
			var intent = new Intent (this, typeof(RegistrationIntentService));
			StartService (intent);
			// Start the Location Manager (GPS)
			locMgr = GetSystemService (Context.LocationService) as LocationManager;

			// Commence the Application
			LoadApplication (new App ());
		}
		// Turns GPS back on
		protected override void OnResume ()
		{
			locprovider = LocationManager.GpsProvider;
			base.OnResume ();
			LocalBroadcastManager.GetInstance (this).RegisterReceiver (mRegistrationBroadcastReceiver,
				new IntentFilter ("registrationComplete"));
			locMgr.RequestLocationUpdates(locprovider, 0, 0, this);
		}
		// Turns GPS off
		protected override void OnPause ()
		{
			LocalBroadcastManager.GetInstance (this).UnregisterReceiver (mRegistrationBroadcastReceiver);
			base.OnPause ();
			locMgr.RemoveUpdates(this);
		}
			// Updates locations
			async public void OnLocationChanged (Location location)
			{
				var geopoint = new ParseGeoPoint (location.Latitude, location.Longitude);
				var user = ParseUser.CurrentUser;

			if (user != null) {
				user ["location"] = geopoint;
				await user.SaveAsync ();
			}
			OpenedLocation.SetLoc (location.Latitude, location.Longitude);
			}
		// GCM Reciever
		class BroadcastReceiver : Android.Content.BroadcastReceiver
		{
			public EventHandler<BroadcastEventArgs> Receive { get; set; }

			public override void OnReceive (Context context, Intent intent)
			{
				if (Receive != null)
					Receive (context, new BroadcastEventArgs (intent));
			}
		}

		class BroadcastEventArgs : EventArgs
		{
			public Intent Intent { get; private set; }

			public BroadcastEventArgs (Intent intent)
			{
				Intent = intent;
			}
		}

		public void OnProviderEnabled (string provider)
		{
				
			}

		public void OnProviderDisabled (string provider)
		{
				
			}

		public void OnStatusChanged (string provider, Availability status, Bundle extras)
		{
				
			}
		
	}
}

