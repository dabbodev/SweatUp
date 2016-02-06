using System;

using Xamarin.Forms;

namespace SweatUp
{
	public class IntroPage : ContentPage
	{
		public Label Next;
		public IntroPage ()
		{
			Padding = 5;
			Next = new Label {
				Text = "Next ->",
				FontSize = 25,
				HorizontalTextAlignment = TextAlignment.End,
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.End
			};

			var InnerLayout = new StackLayout { 
				Children = {
					new Label { Text = "Welcome to SweatUp", 
						FontSize = 50, 
						HorizontalTextAlignment = TextAlignment.Center, 
						HorizontalOptions = LayoutOptions.CenterAndExpand },
					new Label { Text = "On the next screen, we'll select your default gym.", 
						FontSize = 25, 
						HorizontalTextAlignment = TextAlignment.Center,
						HorizontalOptions = LayoutOptions.CenterAndExpand},
					Next
				},
				VerticalOptions = LayoutOptions.CenterAndExpand
			};


			var OuterLayout = new StackLayout {
				Children = { 
					InnerLayout, Next
				}
			};
			


			Content = OuterLayout;
		}
	}
}


