using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Parse;

namespace SweatUp
{
	public class RegisterPage : ContentPage
	{
		public Entry UsernameEntry;
		public Entry EmailEntry;
		public Entry PasswordEntry;
		public Entry ConfirmEntry;
		public Entry PhoneEntry;
		public Xamarin.Forms.Button RegisterButton;
		public Label LSwitch;
		public WebView browser;

		public RegisterPage ()
		{
			var TitleLabel = new Label {
				HorizontalTextAlignment = TextAlignment.Center,
				Text = "Register",
				FontSize = 45,
				TextColor = Color.White,
				VerticalOptions = LayoutOptions.Center
			};
			UsernameEntry = new Entry {
				Placeholder = "Full Name",
				ClassId = "Register",
				Keyboard = Keyboard.Text,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				PlaceholderColor = Color.Gray
			};
			UsernameEntry.Completed += (sender, e) => { EmailEntry.Focus(); };
			EmailEntry = new Entry {
				Placeholder = "Email Address",
				ClassId = "Register",
				Keyboard = Keyboard.Email,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				PlaceholderColor = Color.Gray
			};
			EmailEntry.Completed += (sender, e) => { PasswordEntry.Focus(); };
			PasswordEntry = new Entry {
				Placeholder = "Password",
				IsPassword = true,
				ClassId = "Register",
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				PlaceholderColor = Color.Gray
			};
			PasswordEntry.Completed += (sender, e) => {ConfirmEntry.Focus(); };
			ConfirmEntry = new Entry {
				Placeholder = "Confirm Password",
				IsPassword = true,
				ClassId = "Register",
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				PlaceholderColor = Color.Gray
			};
			ConfirmEntry.Completed += (sender, e) => { PhoneEntry.Focus(); };
			PhoneEntry = new Entry {
				Placeholder = "Phone Number",
				ClassId = "Register",
				Keyboard = Keyboard.Telephone,
				VerticalOptions = LayoutOptions.Center,
				TextColor = Color.White,
				PlaceholderColor = Color.Gray
			};
			RegisterButton = new Xamarin.Forms.Button {
				Text = "Register",
				VerticalOptions = LayoutOptions.Center
			};
			RegisterButton.Clicked += Register_Clicked;
			LSwitch = new Label {
				Text = "Already a member? Tap to Login",
				TextColor = Color.White,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			this.BackgroundImage = "LoginBG.jpg";

			StackLayout FormLayout = new StackLayout {
				Children = { TitleLabel, UsernameEntry, EmailEntry, PasswordEntry, ConfirmEntry, PhoneEntry, RegisterButton, LSwitch },
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			Content = FormLayout;
		}

		async void Register_Clicked (object sender, EventArgs e)
		{
			if (Regex.IsMatch (UsernameEntry.Text, @"^[a-zA-Z ]+$")) {

				if (Regex.IsMatch (PhoneEntry.Text, @"^[0-9]+$")) {

					if (Regex.IsMatch (PasswordEntry.Text, @"^[a-zA-Z0-9]+$")) {

						if (Regex.IsMatch (EmailEntry.Text,
							@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
							@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
							RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds (250))) {

							if (PasswordEntry.Text == ConfirmEntry.Text) {

								var fname = Regex.Replace (UsernameEntry.Text, @"(^\w)|(\s\w)", m => m.Value.ToUpper ());

								ParseUser user = new ParseUser () {
									Username = EmailEntry.Text.ToLower(),
									Password = PasswordEntry.Text,
									Email = EmailEntry.Text.ToLower()
								};

								user ["phone"] = PhoneEntry.Text;
								user ["name"] = fname;
								user ["location"] = new ParseGeoPoint (OpenedLocation.Lat, OpenedLocation.Lng);

								await user.SignUpAsync ();
								SweatUp.Droid.MyGcmListenerService.Toaster("Logging in as " + ParseUser.CurrentUser.Get<String>("name"));
								await Navigation.PopModalAsync ();
								MessagingCenter.Send(this, "popped");

							} else {
								await DisplayAlert ("Error", "Passwords do not match.", "Okay");
							}
						} else {
							await DisplayAlert ("Error", "Invalid Email", "Okay");
						}
					} else {
						await DisplayAlert ("Error", "Please only use letters and numbers for your password.", "Okay");
					}
				} else {
					await DisplayAlert ("Error", "Please only use numbers for your phone number", "Okay");
				}
			} else {
				await DisplayAlert ("Error", "Please only use letters for your username.", "Okay");
			}
		}
	}
}


