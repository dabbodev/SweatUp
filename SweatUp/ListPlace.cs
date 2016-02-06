using System;
using Xamarin.Forms;

namespace SweatUp
{
	public class ListPlace
	{
		public ListPlace (string name, string desc, ImageSource image, string id)
		{
			this.Name = name;
			this.Desc = desc;
			this.Image = image;
			this.ID = id;
		}

		public string Name { private set; get; }

		public string Desc { private set; get; }

		public ImageSource Image { private set; get; }

		public string ID { private set; get; }
	}
}

