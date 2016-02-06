using System;

using Xamarin.Forms;
using Parse;

namespace SweatUp
{
	public class GymPage : ContentPage
	{
		public Image header;
		public ImageCell GymInfo;
		public TableView DisplayInfo;
		public ListView Options = new ListView();
		public FontAwesomeIcon MenuSwitch = new FontAwesomeIcon();


		public GymPage (String gym)
		{
			showGymPage (gym);
		}

		async void showGymPage(String sgym)
		{
			
			ParseQuery<ParseObject> query = ParseObject.GetQuery("Place");
			ParseObject gym = await query.GetAsync(sgym);
			var source = new UriImageSource {
				Uri = gym.Get<ParseFile> ("Header").Url
			};

			header = new Image {
				Source = source,
				Aspect = Aspect.AspectFill,
				HeightRequest = 200,
				WidthRequest = 960,
				VerticalOptions = LayoutOptions.Start
			};

			GymInfo = new ImageCell {
				Text = gym.Get<String> ("Name"), 
				Detail = gym.Get<String> ("Address"), 
				ImageSource = new UriImageSource {
					Uri = gym.Get<ParseFile> ("Picture").Url
				}
			};

			DisplayInfo = new TableView {
				Intent = TableIntent.Data,
				HasUnevenRows = true,
				Root = new TableRoot {
					new TableSection {
						GymInfo
					},
				},
				VerticalOptions = LayoutOptions.Start,
				HeightRequest = 70
			};

			DisplayInfo.GestureRecognizers.Clear();

			Label InfoLabel = new Label {
				Text = "You have no classes or memberships",
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Start
			};
		
			try {
				var cquery = from classes in ParseObject.GetQuery("Class")
						where classes["Gym"] == gym
						where classes["User"] == ParseUser.CurrentUser
						where classes["Used"] == null
					select classes;
				var numclasses = await cquery.CountAsync();
				if (numclasses > 0) {
					if (numclasses == 1) {
						InfoLabel.Text = "You have " + numclasses + " class";
					}else{
						InfoLabel.Text = "You have " + numclasses + " classes";
					}
				}
					
			}catch (Exception e) {
				Console.WriteLine (e.Message);
			}

			try {
				var mquery = from memship in ParseObject.GetQuery("Membership")
						where memship["Gym"] == gym
						where memship["User"] == ParseUser.CurrentUser
						where (DateTime)memship["Expires"] > DateTime.Now
					select memship;
				ParseObject curship = await mquery.FirstAsync();
				if (curship != null)
					InfoLabel.Text = "Your membership expires on " + curship.Get<DateTime>("Expires").Month + "/" + curship.Get<DateTime>("Expires").Day + "/" + curship.Get<DateTime>("Expires").Year;
			}catch (Exception e) {
				Console.WriteLine (e.Message);
			}

			Options.ItemsSource = new String[] {
				"Buy Classes or Memberships",
				"Class Schedule",
				"Instructors",
				"Gym Store",
				"Contact"
			};

			Options.VerticalOptions = LayoutOptions.Start;

			StackLayout InnerLayout = new StackLayout { 
				Padding = 0,
				Spacing = 0,
				Children = {
					header, DisplayInfo, InfoLabel, Options
				},
				VerticalOptions = LayoutOptions.StartAndExpand
			};

			MenuSwitch.Text = FontAwesomeIcon.FABars;
			MenuSwitch.FontFamily = "FontAwesome";
			MenuSwitch.FontSize = 25;
			MenuSwitch.HorizontalOptions = LayoutOptions.Start;
			MenuSwitch.VerticalOptions = LayoutOptions.Start;
			MenuSwitch.TextColor = Color.White;

			StackLayout Overlay = new StackLayout {
				Padding = 12,
				Children = { MenuSwitch }
			};

			RelativeLayout OuterLayout = new RelativeLayout ();

			OuterLayout.Children.Add (InnerLayout, 
				Constraint.Constant (0), 
				Constraint.Constant (0),
				Constraint.RelativeToParent ((parent) => { return parent.Width; }),
				Constraint.RelativeToParent ((parent) => { return parent.Height; }));

			OuterLayout.Children.Add (Overlay, 
				Constraint.Constant (0), 
				Constraint.Constant (0),
				Constraint.RelativeToParent ((parent) => { return parent.Width; }),
				Constraint.RelativeToParent ((parent) => { return parent.Height; }));

			Content = OuterLayout;
		}
	}
}


