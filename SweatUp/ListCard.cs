using System;

namespace SweatUp
{
	public class ListCard
	{
		public ListCard (string text, int index, string id)
		{
			this.Text = text;
			this.Index = index;
			this.ID = id;
		}

		public string Text { private set; get; }
		public int Index { private set; get; }
		public string ID { private set; get; }
	}
}

