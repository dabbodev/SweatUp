using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using Parse;

namespace SweatUp
{
	public class SubPage : ContentPage
	{
		public ListView Choices = new ListView();
		public List<BuyType> Buys = new List<BuyType>();

		public SubPage ()
		{
			showData ();

			
		}

		async void showData() {

			ParseQuery<ParseObject> query1 = ParseObject.GetQuery("Place");
			ParseObject gym = await query1.GetAsync(Glbals.currentGym);
			
			var query = from classes in ParseObject.GetQuery ("cPricing")
					where classes ["Gym"] == gym
				select classes;

			var query2 = from memships in ParseObject.GetQuery ("mPricing")
					where memships ["Gym"] == gym
				select memships;

			IEnumerable<ParseObject> results = await query.FindAsync();
			IEnumerable<ParseObject> results2 = await query2.FindAsync();

			foreach (ParseObject cprice in results) {
				if (cprice.Get<int> ("Batch") > 1) {
					Buys.Add (new BuyType (
						cprice.Get<int> ("Batch") + " Classes - $" + cprice.Get<double> ("Price"),
						cprice.Get<double> ("Price"),
						cprice.ObjectId,
					"Class"));
				} else {
					Buys.Add (new BuyType (
						cprice.Get<int> ("Batch") + " Class - $" + cprice.Get<double> ("Price"),
						cprice.Get<double> ("Price"),
						cprice.ObjectId,
					"Class"));
				}
			}

			foreach (ParseObject mprice in results2) {
				if (mprice.ContainsKey("Weeks")) {
					Buys.Add (new BuyType (
						mprice.Get<int>("Weeks") + " Week Membership - $" + mprice.Get<double>("Price"),
						mprice.Get<double>("Price"),
						mprice.ObjectId,
					"Memship"));
				}
				if (mprice.ContainsKey("Months")) {
					Buys.Add (new BuyType (
						mprice.Get<int>("Months") + " Month Membership - $" + mprice.Get<double>("Price"),
						mprice.Get<double>("Price"),
						mprice.ObjectId,
					"Memship"));
				}
				if (mprice.ContainsKey("Years")) {
					Buys.Add (new BuyType (
						mprice.Get<int>("Year") + " Year Membership - $" + mprice.Get<double>("Price"),
						mprice.Get<double>("Price"),
						mprice.ObjectId,
					"Memship"));
				}
			}
			
					var cell = new DataTemplate(typeof(TextCell));

					cell.SetBinding(TextCell.TextProperty, "Name");

					Choices.ItemsSource = Buys;
					Choices.ItemTemplate = cell;

					StackLayout layout = new StackLayout {
						VerticalOptions = LayoutOptions.Center,
						Children = { Choices }
					};

					Content = layout; 

		}

	}
}


