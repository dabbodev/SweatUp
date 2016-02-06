using System;

using Xamarin.Forms;

using Parse;

namespace SweatUp
{
	public class LoginPage : ContentPage
	{
		public Entry EmailEntry;
		public Entry PasswordEntry;
		public Button LoginButton;
		public Label LSwitchBack;

		public LoginPage ()
		{
			var TitleLabel = new Label {
				HorizontalTextAlignment = TextAlignment.Center,
				Text = "Login",
				FontSize = 45,
				TextColor = Color.White
			};
			EmailEntry = new Entry {
				Placeholder = "Email Address",
				ClassId = "Register",
				Keyboard = Keyboard.Text
			};


			PasswordEntry = new Entry {
				Placeholder = "Password",
				IsPassword = true,
				ClassId = "Register"
			};


			LoginButton = new Xamarin.Forms.Button {
				Text = "Login",
			};
			LoginButton.Clicked += LoginButton_Clicked;

			LSwitchBack = new Label {
				Text = "Not a member? Tap to Register",
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center
			};

			this.BackgroundImage = "LoginBGSpan.jpg";

			StackLayout FormLayout = new StackLayout {
				Children = { TitleLabel, EmailEntry, PasswordEntry, LoginButton, LSwitchBack },
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			Content = FormLayout;
		}

		async void LoginButton_Clicked (object sender, EventArgs e)
		{
			try
			{
				await ParseUser.LogInAsync(EmailEntry.Text.ToLower(), PasswordEntry.Text);
				SweatUp.Droid.MyGcmListenerService.Toaster("Logging in as " + ParseUser.CurrentUser.Get<String>("name"));
				await Navigation.PopModalAsync ();
				MessagingCenter.Send(this, "popped");
			}
			catch (Exception exep)
			{
				DisplayAlert ("Error", exep.Message, "Okay");
			}
		}
	}
}


