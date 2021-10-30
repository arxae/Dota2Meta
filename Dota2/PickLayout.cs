namespace Dota2Meta.Dota2
{
	using System.Collections.Generic;

	public class PickScreenLayout
	{
		public int version { get; set; }
		public List<Config> configs { get; set; } = new List<Config>();
	}

	public class Config
	{
		public string config_name { get; set; }
		public List<Category> categories { get; set; } = new List<Category>();
		public bool createdByDota2Meta { get; set; } // Used to sepparate existing layouts from generated ones
	}

	public class Category
	{
		public string category_name { get; set; }
		public double x_position { get; set; }
		public double y_position { get; set; }
		public double width { get; set; }
		public double height { get; set; }
		public List<int> hero_ids { get; set; } = new List<int>();
	}
}