using System;
using Android;
using Android.Content;
using Android.Views;

using Xamarin.Forms;


[assembly: ExportRenderer(typeof(TextCell), typeof(SweatUp.Droid.Renderers.SweatUpTextCellRenderer))]

namespace SweatUp.Droid.Renderers
{
	public class SweatUpTextCellRenderer : Xamarin.Forms.Platform.Android.TextCellRenderer
	{
		protected override global::Android.Views.View GetCellCore(Cell item, global::Android.Views.View convertView, ViewGroup parent, Context context)
		{
			var view = base.GetCellCore(item, convertView, parent, context) as ViewGroup;

			if (item is TextCell)
			if (String.IsNullOrEmpty((item as TextCell).Text))
			{
				if (view != null)
				{
					view.Visibility = ViewStates.Gone;
					while (view.ChildCount > 0)
						view.RemoveViewAt(0);
					view.SetMinimumHeight(0);
					view.SetPadding(0, 0, 0, 0);
				}
			}

			return view;
		}
	}
}