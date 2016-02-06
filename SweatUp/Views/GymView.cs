using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SweatUp
{
	public class GymView : CarouselPage
	{
		public GymView ()
		{
			
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
				if (this.Children.IndexOf (this.CurrentPage) == 0) {
					this.Children.RemoveAt (1);
				} else {
					this.Children.RemoveAt (0);
				}
			}
		}

	}
}


