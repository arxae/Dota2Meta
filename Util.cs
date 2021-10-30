namespace Dota2Meta
{
	using System;
	using System.IO;
	using System.Collections.Generic;

	public static class Util
	{
		// Quick n' dirty config. Replace with something more durable later
		public static Dictionary<string, string> ParseConfig(string[] configText)
		{
			var c = new Dictionary<string, string>();
			foreach(var l in configText)
			{
				var split = l.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
				if(split.Length != 2) throw new ArgumentException("Config line expects key/value, but couldn't parse as such");
				if(c.ContainsKey(split[0])) throw new ArgumentException("Duplicate key in config");

				c.Add(split[0], split[1]);
			}

			// Create some extra paths
			// Check for steam.exe
			if(File.Exists(Path.Combine(c["SteamPath"], "Steam.exe")) == false)
			{
				throw new FileNotFoundException("Provided steam path is invalid");
			}

			// DOTA2 Steam id = 570
			c.Add("DotaPath", Path.Combine(c["SteamPath"], "userdata", c["AccountId"], "570", "remote"));

			// Check if dota 2 account folder exists
			if(Directory.Exists(c["DotaPath"]) == false)
			{
				throw new DirectoryNotFoundException("Unable to find dota2 directory with provided account id");
			}

			c.Add("GridPath", Path.Combine(c["DotaPath"], "cfg", "hero_grid_config.json"));

			return c;
		}
	}
}