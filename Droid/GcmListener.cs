using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Gms.Gcm;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Media;
using Android.Support.V4.App;
using Android.Util;
using Android.OS;
using Android.Widget;
using Parse;
using Xamarin.Forms;

namespace SweatUp.Droid
{
	[Service (Exported = false), IntentFilter (new [] { "com.google.android.c2dm.intent.RECEIVE" })]
	public class MyGcmListenerService : GcmListenerService
	{
		const string TAG = "MyGcmListenerService";

		public override void OnMessageReceived (string from, Bundle data)
		{
			var message = data.GetString ("message");
			var title = data.GetString ("title");
			var topic = data.GetString ("topic");
			Log.Debug (TAG, "From: " + from);
			Log.Debug (TAG, "Message: " + message);

			SendNotification (title, message, topic);
		}

		async void SendNotification (string title, string message, string topic)
		{
			var intent = new Intent (this, typeof(MainActivity));
			intent.AddFlags (ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity (this, 0, intent, PendingIntentFlags.OneShot);

			var defaultSoundUri = RingtoneManager.GetDefaultUri (RingtoneType.Notification);

			if (topic != "global") {

				ParseQuery<ParseObject> query = ParseObject.GetQuery ("Place");
				var place = await query.GetAsync(topic);
				var imageFile = place.Get<ParseFile> ("Picture");

				Bitmap BigIco = GetImageBitmapFromUrl(imageFile.Url.ToString());

				var notificationBuilder = new NotificationCompat.Builder (this)
				.SetLargeIcon (BigIco)
				.SetSmallIcon (SweatUp.Droid.Resource.Drawable.icon)
				.SetContentTitle (title)
				.SetContentText (message)
				.SetAutoCancel (true)
				.SetSound (defaultSoundUri)
				.SetVibrate (new long[] { 500, 300, 500, 300, 500 })
				.SetLights (Android.Graphics.Color.AliceBlue, 300, 700)
				.SetCategory (Notification.CategorySocial)
				.SetContentIntent (pendingIntent);

				var notificationManager = (NotificationManager)GetSystemService (Context.NotificationService);
				notificationManager.Notify (0, notificationBuilder.Build ());

			} else {
				var notificationBuilder = new NotificationCompat.Builder (this)
					.SetSmallIcon (SweatUp.Droid.Resource.Drawable.icon)
					.SetContentTitle (title)
					.SetContentText (message)
					.SetAutoCancel (true)
					.SetSound (defaultSoundUri)
					.SetVibrate (new long[] { 500, 300, 500, 300, 500 })
					.SetLights (Android.Graphics.Color.AliceBlue, 300, 700)
					.SetCategory (Notification.CategorySocial)
					.SetContentIntent (pendingIntent);

				var notificationManager = (NotificationManager)GetSystemService (Context.NotificationService);
				notificationManager.Notify (0, notificationBuilder.Build ());
			}


		}

		private Bitmap GetImageBitmapFromUrl(string url)
		{
			Bitmap imageBitmap = null;

			using (var webClient = new WebClient())
			{
				var imageBytes = webClient.DownloadData(url);
				if (imageBytes != null && imageBytes.Length > 0)
				{
					imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
				}
			}

			return imageBitmap;
		}

		public static Boolean Toaster (string Message) {
			Toast.MakeText (Android.App.Application.Context, Message, ToastLength.Long).Show();
			return true;
		}

	}
}

