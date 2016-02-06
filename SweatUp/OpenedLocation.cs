using System;

namespace SweatUp
{
	public static class OpenedLocation
	{
		public static double Lat = 0;
		public static double Lng = 0;

		public static Boolean SetLoc (double lat, double lng)
		{
			Lat = lat;
			Lng = lng;
			return true;
		}
	}
}

