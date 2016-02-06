using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

using Parse;

namespace SweatUp
{
	
	public class HomeView : MasterDetailPage
	{
		GymView container = new GymView ();

		public HomeView ()
		{
			MessagingCenter.Subscribe<LoginPage>(this, "popped", ((obj) => {
				OnAppearing ();
			}));
			MessagingCenter.Subscribe<WelcomeView>(this, "popped", ((obj) => {
				OnAppearing ();
			}));
			MessagingCenter.Subscribe<RegisterPage>(this, "popped", ((obj) => {
				OnAppearing ();
			}));
			// Check if user logged in
			if (ParseUser.CurrentUser != null)
			{
				// Generate the List of Places

			}
			else
			{
				// Show registration screen

			}

			Button button = new Button {
				Text = "Sign Out"
			};
			button.Clicked += Button_Clicked;

			this.Master = new ContentPage { 
				Title = "Home",
				Content = new StackLayout {
					Children = {
						button
					}
				},

			};

			this.Detail = new ContentPage {
				Content = new StackLayout {
					Children = {
					}
				}
			};
					
		}

		async void ShowReg ()
		{
			Page Register = new LoginView ();
			await Navigation.PushModalAsync(Register);
		}

		void ButtonT_Clicked (object sender, EventArgs e)
		{
			this.IsPresented = true;
		}

		void Button_Clicked (object sender, EventArgs e)
		{
			this.IsPresented = false;
			ParseUser.LogOut ();
			ShowReg ();
		}

		void generateHome() {
			var user = ParseUser.CurrentUser;
			ParseObject gym = (ParseObject)user ["Default"];
			String gymID = gym.ObjectId;
			Glbals.currentGym = gymID;
			GymPage gympage = new GymPage (gymID);
			container.Children.Add (gympage);
			this.Detail = container;
			var tgr = new TapGestureRecognizer ();
			gympage.MenuSwitch.GestureRecognizers.Add (tgr);
			tgr.Tapped += OpenMaster;
			gympage.Options.ItemSelected += Gympage_Options_ItemSelected;
		}

		void Gympage_Options_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem.ToString () == "Buy Classes or Memberships") {
				SubPage bv = new SubPage ();
				container.Children.Add (bv);
				bv.Choices.ItemSelected += Bv_Choices_ItemSelected;
			}
		}

		void Bv_Choices_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			Glbals.currentBuy = ((BuyType)e.SelectedItem).ID;
			Glbals.cBuyType = ((BuyType)e.SelectedItem).Type;
			Glbals.cBuyName = ((BuyType)e.SelectedItem).Name;
			BuyPage bp = new BuyPage ();
			MessagingCenter.Subscribe<BuyPage>(this, "goback", ((obj) => {
				var user = ParseUser.CurrentUser;
				ParseObject gym = (ParseObject)user ["Default"];
				String gymID = gym.ObjectId;
				Glbals.currentGym = gymID;
				GymPage gympage = new GymPage (gymID);
				container.Children.Insert (0, gympage);
				this.Detail = container;
				var tgr = new TapGestureRecognizer ();
				gympage.MenuSwitch.GestureRecognizers.Add (tgr);
				tgr.Tapped += OpenMaster;
				gympage.Options.ItemSelected += Gympage_Options_ItemSelected;
			}));
			container.Children.Add (bp);

		}

		void OpenMaster (object sender, EventArgs e) {
			this.IsPresented = true;
		}

		async void Welcome ()
		{
			Page Welcome = new WelcomeView ();
			await Navigation.PushModalAsync(Welcome);
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			if (ParseUser.CurrentUser != null) {
				try {
					var user = ParseUser.CurrentUser;
					if (user["Default"] != null) {
						SweatUp.Droid.RegistrationIntentService.SubUserTopics();
						generateHome ();
					} else {
						Welcome ();
					}
				}catch (Exception e) {
					Welcome ();
				}
			} else {
				ShowReg ();
			}
		}
	}
}


