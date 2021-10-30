namespace Dota2Meta
{
	using System.IO;
	using System.Text.Json;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	public class Config
	{
		public string steamPath { get; set; }
		public string accountId { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string dotaPath { get; set; }

		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string gridPath { get; set; }

		public List<GridConfig> gridConfig { get; set; }

		public static Config ParseFromPath(string path)
		{
			if (File.Exists(path) == false)
			{
				throw new FileNotFoundException("Unable to find config file");
			}

			string json = File.ReadAllText(path);
			var c = JsonSerializer.Deserialize<Config>(json);

			// Do some checks
			// Check for Steam.exe
			if (File.Exists(Path.Combine(c.steamPath, "Steam.exe")) == false)
			{
				throw new FileNotFoundException("Provided steam path is invalid");
			};

			// Check for the Dota2 userdata path
			// DOTA2 steam id = 570
			c.dotaPath = Path.Combine(c.steamPath, "userdata", c.accountId, "570", "remote");

			if (Directory.Exists(c.dotaPath) == false)
			{
				throw new DirectoryNotFoundException("Unabel to find the dota2 directory with the provided account id");
			}

			c.gridPath = Path.Combine(c.dotaPath, "cfg", "hero_grid_config.json");

			return c;
		}
	}

	public class GridConfig
	{
		public string name { get; set; }
		public List<string> brackets { get; set; }
		public double height { get; set; }		
		public double width { get; set; }
		public int topWins { get; set; }
		public List<string> positions { get; set; }
		public List<CustomRow> custom { get; set; }
	}

	public class CustomRow
    {
        public string name { get; set; }
        public List<string> heroes { get; set; }
    }
}