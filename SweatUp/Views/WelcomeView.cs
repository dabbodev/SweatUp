using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Parse;

namespace SweatUp
{
	public class WelcomeView : CarouselPage
	{
		IntroPage intro = new IntroPage ();
		PlaceListPage listpage = new PlaceListPage ();

		public WelcomeView ()
		{
			var tgr = new TapGestureRecognizer ();
			intro.Next.GestureRecognizers.Add (tgr);
			tgr.Tapped += Tgr_Tapped;
			Children.Add (intro);
		}

		void Tgr_Tapped (object sender, EventArgs e)
		{
			toList ();
		}

		void toList() {
			listpage.MList.ItemSelected += Listpage_MList_ItemSelected;
			Children.Add (listpage);
		}

		async void Listpage_MList_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			ListPlace selected = (ListPlace)e.SelectedItem;
			var user = ParseUser.CurrentUser;
			ParseQuery<ParseObject> query = ParseObject.GetQuery ("Place");
			ParseObject place = await query.GetAsync(selected.ID);
			user ["Default"] = place;
			var relation = user.GetRelation<ParseObject>("customerAt");
			relation.Add(place);
			List<String> topics = new List<String>();
			topics.Add (place.ObjectId);
			user ["topics"] = topics;
			await user.SaveAsync();
			await Navigation.PopModalAsync ();
			MessagingCenter.Send(this, "popped");
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
				this.Children.RemoveAt (0);
			}
		}
	}
}


