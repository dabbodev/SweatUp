using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;

using Xamarin.Payments.Stripe;
using Newtonsoft.Json;
using Parse;

namespace SweatUp
{
	public class BuyPage : ContentPage
	{
		Entry CCNumber = new Entry();
		Entry CCCVC = new Entry();
		Entry CCExpM = new Entry();
		Entry CCExpY = new Entry();
		ListView Cards = new ListView();
		List<ListCard> CardData = new List<ListCard>();

		public BuyPage ()
		{
			getCustomer ();

		}

		async void getCustomer() {
			ParseQuery<ParseObject> query2 = ParseObject.GetQuery("Place");
			var place = await query2.GetAsync (Glbals.currentGym);
			StripePayment payment = new StripePayment (place.Get<String>("SecretKey"));
			try {
				var cquery = from gcid in ParseObject.GetQuery("c_ID")
						where gcid["Gym"] == place
						where gcid["User"] == ParseUser.CurrentUser
					select gcid;
				ParseObject cid = await cquery.FirstOrDefaultAsync ();
				StripeCustomer customer = payment.GetCustomer(cid.Get<String>("cID"));
				int cardindex = 0;
				foreach (StripeCard card in customer.Cards.Data) {
					CardData.Add (new ListCard (card.Type + " Card - Ending in " + card.Last4, cardindex, card.ID));
					cardindex++;
				}
				CardData.Add (new ListCard("Or add new card", cardindex, "NEW"));

				var cell = new DataTemplate (typeof(TextCell));
				cell.SetBinding (TextCell.TextProperty, "Text");

				Cards.ItemTemplate = cell;
				Cards.ItemsSource = CardData;

				Cards.ItemSelected += Cards_ItemSelected;
				Label label = new Label {
					Text = Glbals.cBuyName
				};
					
				Label label2 = new Label {
					Text = "Select a card on file"
				};
				Content = new StackLayout {
					Children = { label, label2, Cards }
				};
			}catch (Exception e) {
				CCNumber.Placeholder = "CC Number";
				CCCVC.Placeholder = "CC CVC";
				CCExpM.Placeholder = "CC Exp Month";
				CCExpY.Placeholder = "CC Exp Year";
				CCNumber.Keyboard = Keyboard.Numeric;
				CCCVC.Keyboard = Keyboard.Numeric;
				CCExpM.Keyboard = Keyboard.Numeric;
				CCExpY.Keyboard = Keyboard.Numeric;
				Label label = new Label {
					Text = Glbals.cBuyName
				};

				Button Send = new Button {
					Text = "Submit"
				};
				Send.Clicked += Send_Clicked;
				Content = new StackLayout {
					Children = { label, CCNumber, CCExpM, CCExpY, CCCVC, Send }
				};
			}
		}

		async void Cards_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			if (((ListCard)e.SelectedItem).ID != "NEW") {
				ParseQuery<ParseObject> query2 = ParseObject.GetQuery("Place");
				var place = await query2.GetAsync (Glbals.currentGym);
				StripePayment payment = new StripePayment (place.Get<String>("SecretKey"));
				StripeCustomer customer = new StripeCustomer();
				try {
					var cquery = from gcid in ParseObject.GetQuery("c_ID")
							where gcid["Gym"] == place
							where gcid["User"] == ParseUser.CurrentUser
						select gcid;
					ParseObject cid = await cquery.FirstOrDefaultAsync ();
					customer = payment.GetCustomer(cid.Get<String>("cID"));
				}catch (Exception exep) {

				}

				ParseQuery<ParseObject> query;
				if (Glbals.cBuyType == "Class") {
					query = ParseObject.GetQuery ("cPricing");
				} else {
					query = ParseObject.GetQuery ("mPricing");
				}
				var cprice = await query.GetAsync (Glbals.currentBuy);
				StripeCharge charge = payment.Charge (((int)cprice.Get<double> ("Price") * 100), "usd", customer.ID, ((ListCard)e.SelectedItem).ID, Glbals.cBuyName);
				if (charge.Paid == true) {
					if (Glbals.cBuyType == "Class") {
						int batch = 0;
						while (batch < cprice.Get<int> ("Batch")) {
							var nclass = new ParseObject ("Class");
							nclass ["User"] = ParseUser.CurrentUser;
							nclass ["Gym"] = place;
							await nclass.SaveAsync ();
							batch++;
						}
					} else {
						DateTime mtime = new DateTime();
						if (cprice.ContainsKey ("Weeks")) {
							mtime = DateTime.Now.AddDays (cprice.Get<int> ("Weeks") * 7);
						}
						if (cprice.ContainsKey ("Months")) {
							mtime = DateTime.Now.AddMonths (cprice.Get<int> ("Months"));
						}
						if (cprice.ContainsKey ("Years")) {
							mtime = DateTime.Now.AddYears (cprice.Get<int> ("Years"));
						}
						ParseObject memship = new ParseObject ("Membership");
						memship ["User"] = ParseUser.CurrentUser;
						memship ["Gym"] = place;
						memship ["Starts"] = DateTime.Now;
						memship ["Expires"] = mtime;
						await memship.SaveAsync ();
					}
					await Task.Delay (1000);
					MessagingCenter.Send (this, "goback");
				} else {
					await DisplayAlert ("Error", charge.FailureMessage, "Okay");
				}
			}else{
				CCNumber.Placeholder = "CC Number";
				CCCVC.Placeholder = "CC CVC";
				CCExpM.Placeholder = "CC Exp Month";
				CCExpY.Placeholder = "CC Exp Year";
				CCNumber.Keyboard = Keyboard.Numeric;
				CCCVC.Keyboard = Keyboard.Numeric;
				CCExpM.Keyboard = Keyboard.Numeric;
				CCExpY.Keyboard = Keyboard.Numeric;
				Label label = new Label {
					Text = Glbals.cBuyName
				};

				Button Send = new Button {
					Text = "Submit"
				};
				Send.Clicked += Send_Clicked;
				Content = new StackLayout {
					Children = { label, CCNumber, CCExpM, CCExpY, CCCVC, Send }
				};
			}
		}

		async void Send_Clicked (object sender, EventArgs e)
		{
			
			ParseQuery<ParseObject> query2 = ParseObject.GetQuery("Place");
			var place = await query2.GetAsync (Glbals.currentGym);
			StripePayment payment = new StripePayment (place.Get<String>("SecretKey"));
			StripeCustomer customer;
			try {
				var cquery = from gcid in ParseObject.GetQuery("c_ID")
						where gcid["Gym"] == place
						where gcid["User"] == ParseUser.CurrentUser
					select gcid;
				ParseObject cid = await cquery.FirstOrDefaultAsync ();
				customer = payment.GetCustomer(cid.Get<String>("cID"));
				var cc = GetCC();
				StripeCreditCardToken tok = payment.CreateToken (cc);
				StripeCustomerInfo info_update = new StripeCustomerInfo ();
				info_update.Card = tok.ID;
				customer = payment.UpdateCustomer (customer.ID, info_update);
			}catch (Exception exep) {
				var cc = GetCC ();
				StripeCreditCardToken tok = payment.CreateToken (cc);
				StripeCustomerInfo cinfo = new StripeCustomerInfo ();
				cinfo.Card = tok.ID;
				cinfo.Email = ParseUser.CurrentUser.Get<String> ("email");
				cinfo.Description = ParseUser.CurrentUser.Get<String> ("name");

				customer = payment.CreateCustomer (cinfo);

			}

			ParseQuery<ParseObject> query;
			if (Glbals.cBuyType == "Class") {
				query = ParseObject.GetQuery ("cPricing");
			} else {
				query = ParseObject.GetQuery ("mPricing");
			}
			var cprice = await query.GetAsync (Glbals.currentBuy);

			StripeCharge charge = payment.Charge (((int)cprice.Get<double> ("Price") * 100), "usd", customer.ID, Glbals.cBuyName);
			if (charge.Paid == true) {
				if (Glbals.cBuyType == "Class") {
					int batch = 0;
					while (batch < cprice.Get<int> ("Batch")) {
						var nclass = new ParseObject ("Class");
						nclass ["User"] = ParseUser.CurrentUser;
						nclass ["Gym"] = place;
						await nclass.SaveAsync ();
						batch++;
					}
				} else {
					DateTime mtime = new DateTime();
					if (cprice.ContainsKey ("Weeks")) {
						mtime = DateTime.Now.AddDays (cprice.Get<int> ("Weeks") * 7);
					}
					if (cprice.ContainsKey ("Months")) {
						mtime = DateTime.Now.AddMonths (cprice.Get<int> ("Months"));
					}
					if (cprice.ContainsKey ("Years")) {
						mtime = DateTime.Now.AddYears (cprice.Get<int> ("Years"));
					}
					ParseObject memship = new ParseObject ("Membership");
					memship ["User"] = ParseUser.CurrentUser;
					memship ["Gym"] = place;
					memship ["Starts"] = DateTime.Now;
					memship ["Expires"] = mtime;
					await memship.SaveAsync ();
				}
				await Task.Delay (1000);
				MessagingCenter.Send (this, "goback");
			} else {
				await DisplayAlert ("Error", charge.FailureMessage, "Okay");
			}


			if (charge.Paid == true) {

			} else {
				await DisplayAlert ("Error", charge.FailureMessage, "Okay");
			}

		}

		StripeCreditCardInfo GetCC ()
		{
			StripeCreditCardInfo cc = new StripeCreditCardInfo ();
			cc.CVC = CCCVC.Text;
			cc.ExpirationMonth = Int32.Parse(CCExpM.Text);
			cc.ExpirationYear = Int32.Parse(CCExpY.Text);
			cc.Number = CCNumber.Text;
			return cc;
		}

	}
}


