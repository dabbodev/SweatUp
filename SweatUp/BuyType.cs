using System;

namespace SweatUp
{
	public class BuyType
	{
		public BuyType (string name, double price, string id, string type)
		{
			this.Name = name;
			this.Price = price;
			this.ID = id;
			this.Type = type;
		}

		public string Name { private set; get; }
		public double Price { private set; get; }
		public string ID { private set; get; }
		public string Type { private set; get; }	
	}
}

