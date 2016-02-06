using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;
using Parse;

namespace SweatUp
{
	public class LoginView : CarouselPage
	{

		public int pageid;

		public LoginView ()
		{
			BackgroundImage = "LoginBG.jpg";
			var RegPage = new RegisterPage ();
			var tgr = new TapGestureRecognizer ();
			RegPage.LSwitch.GestureRecognizers.Add(tgr);
			tgr.Tapped +=(s,e)=>LSwitch_Focused();
			Children.Add(RegPage);
			pageid = 0;
		}
	

		protected override void OnChildAdded (Element child)
		{
			base.OnChildAdded (child);
			this.CurrentPage = (ContentPage) child;
		}

		protected override async void OnCurrentPageChanged ()
		{
			base.OnCurrentPageChanged ();
			if (Children.Count > 1) {
				await Task.Delay (250);
				if (pageid == 1) {
					this.Children.RemoveAt (0);
				} else {
					this.Children.RemoveAt (1);
				}
			}
		}

		void LSwitch_Focused ()
		{
			var LogPage = new LoginPage ();
			var LSBack = LogPage.LSwitchBack;
			var tgr = new TapGestureRecognizer ();
			LSBack.GestureRecognizers.Add(tgr);
			tgr.Tapped +=(s,e)=>LSwitchBack_Focused();
			pageid = 1;
			Children.Insert (1, LogPage);
		}

		void LSwitchBack_Focused() {
			var RegPage = new RegisterPage ();
			var LSwitch = RegPage.LSwitch;
			var tgr = new TapGestureRecognizer ();
			LSwitch.GestureRecognizers.Add(tgr);
			tgr.Tapped +=(s,e)=>LSwitch_Focused();
			pageid = 0;
			Children.Insert(0, RegPage);
		}
	}
}


