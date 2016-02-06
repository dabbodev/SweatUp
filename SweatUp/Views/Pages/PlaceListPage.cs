using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

using Parse;

namespace SweatUp
{
	public class PlaceListPage : ContentPage
	{
		// Main List for populating data
		public ListView MList = new ListView();
		// The Map object
		public Map MapDisplay;

		public List<ListPlace> ListCont = new List<ListPlace>();

		public PlaceListPage ()
		{
			generateList ();
		}

		async void generateList() {
			var userGeoPoint = ParseUser.CurrentUser.Get<ParseGeoPoint>("location");

			MapDisplay = new Map () {
				IsShowingUser = true,
				HeightRequest = 200,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};
			MapDisplay.MoveToRegion (MapSpan.FromCenterAndRadius(new Position(userGeoPoint.Latitude, userGeoPoint.Longitude), Distance.FromMiles(5)));
			int farthestdist = 0;
			int thisdist = 0;
			// Open the Place class
			var query = ParseObject.GetQuery("Place");
			// Check for Places near the user
			query = query.WhereNear("Geo", userGeoPoint);
			// Limit results to 10
			query = query.Limit(10);
			// return the objects
			IEnumerable<ParseObject> placeObjects = await query.FindAsync();
			// go through the list
			foreach (ParseObject place in placeObjects) {
				var pictureurl = place.Get<ParseFile>("Picture");
				var address = place.Get<String>("Address");
				ParseGeoPoint ploc = place.Get<ParseGeoPoint> ("Geo");
				thisdist = (int) Decimal.Round (Decimal.Parse (ploc.DistanceTo (userGeoPoint).Miles.ToString ()), 0);
				if (thisdist > farthestdist) {
					farthestdist = thisdist;
				}
				ListCont.Add ( new ListPlace(
					place.Get<String>("Name"),
					address + " - " + thisdist + " miles",
					UriImageSource.FromUri(pictureurl.Url),
					place.ObjectId
				));
				var position = new Position(ploc.Latitude,ploc.Longitude); // Latitude, Longitude
				var pin = new Pin {
					Type = PinType.Place,
					Position = position,
					Label = place.Get<String>("Name"),
					Address = place.Get<String>("Address")
				};
				MapDisplay.Pins.Add(pin);
			}

			MapDisplay.MoveToRegion (MapSpan.FromCenterAndRadius(new Position(userGeoPoint.Latitude, userGeoPoint.Longitude), Distance.FromMiles(farthestdist + 1)));

			var cell = new DataTemplate (typeof(ImageCell));

			cell.SetBinding (TextCell.TextProperty, "Name");
			cell.SetBinding (TextCell.DetailProperty, "Desc");
			cell.SetBinding (ImageCell.ImageSourceProperty, "Image");

			MList.ItemsSource = ListCont;
			MList.ItemTemplate = cell;


			Content = new StackLayout {
				Children = {
					MapDisplay, MList
				}
			};
		}
	}
}


