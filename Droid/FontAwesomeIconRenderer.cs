using System;
using Android;
using Android.Content;
using Android.Views;
using Android.Graphics;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SweatUp.FontAwesomeIcon), typeof(SweatUp.Droid.Renderers.FontAwesomeIconRenderer))]

namespace SweatUp.Droid.Renderers
{
	public class FontAwesomeIconRenderer: LabelRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement == null)
			{
				//The ttf in /Assets is CaseSensitive, so name it FontAwesome.ttf
				Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets, "FontAwesome.ttf");
			}
		}
	}
}

