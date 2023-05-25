namespace SWMUFMWeb
{
	public static class NavbarHelper
	{
		public static string home;
		public static string team;
		public static string register;
		public static string match;
		public static string article;
		public static string rank;

		public static void IsHome()
		{
			home = "active";
			team = "";
			register = "";
			match = "";
			article = "";
			rank = "";
		}
		public static void IsTeam()
		{
			home = "";
			team = "active";
			register = "";
			match = "";
			article = "";
			rank = "";
		}
		public static void IsRegister()
		{
			home = "";
			team = "";
			register = "active";
			match = "";
			article = "";
			rank = "";
		}
		public static void IsRank()
		{
			home = "";
			team = "";
			register = "";
			match = "";
			article = "";
			rank = "active";
		}
		public static void IsArticle()
		{
			home = "";
			team = "";
			register = "";
			match = "";
			article = "active";
			rank = "";
		}
		public static void IsMatch()
		{
			home = "";
			team = "";
			register = "";
			match = "active";
			article = "";
			rank = "";
		}
	}
}
