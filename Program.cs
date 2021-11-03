namespace Dota2Meta
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using GraphQL.Client.Http;
	using GraphQL.Client.Serializer.SystemTextJson;
	using Stratz.Types;
	using System.Linq;
	using System.IO;

	using CultureInfo = System.Globalization.CultureInfo;
	using Json = System.Text.Json.JsonSerializer;

	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Dota2Meta, updating..");

			// Get Config
			Console.WriteLine("Reading config");
			string cfgPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");
			var config = Config.ParseFromPath(cfgPath);

			// Setup GQL
			var gql = new GraphQLHttpClient("https://api.stratz.com/graphql", new SystemTextJsonSerializer());
			var stratz = new Stratz.StratzProvider(gql);

			// Deserialize existing grid
			Console.WriteLine("Deserializing current hero grid");
			var heroGrid = Json.Deserialize<Dota2.PickScreenLayout>(File.ReadAllText(config.gridPath));

			// Remove existing grids
			Console.WriteLine("Removing previous grids");
			heroGrid.configs.RemoveAll(c => c.createdByDota2Meta);

			int configNum = 1;
			foreach (var grid in config.gridConfig)
			{
				Console.WriteLine($"Creating config {configNum}/{config.gridConfig.Count} ({grid.name})");
				int bracketNum = 1;
				foreach (var bracket in grid.brackets)
				{
					Console.WriteLine($"Creating grid for {bracket} bracket ({bracketNum}/{grid.brackets.Count})");
					var currBracket = Enum.Parse<RankBracket>(bracket.ToUpper());

					// Make the config name
					string configName = grid.name
						.Replace("$bracket$", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currBracket.ToString().ToLower()))
						.Replace("$version$", await stratz.GetLatestVersionNumber())
						.Replace("$shortdate$", DateTime.Now.ToShortDateString())
						.Replace("$longdate$", DateTime.Now.ToLongDateString());

					var pickConfig = new Dota2.Config();
					pickConfig.config_name = configName;
					pickConfig.createdByDota2Meta = true;

					// Convert position to enum values
					var positions = new List<MatchPlayerPositionType>();
					grid.positions.ForEach(p => positions.Add(Enum.Parse<MatchPlayerPositionType>(p)));

					double xPos = 0;
					foreach (var pos in positions)
					{
						var c = new Dota2.Category();

						// Create a better looking name based on the enum text
						c.category_name = CultureInfo.CurrentCulture.TextInfo
							.ToTitleCase(pos.ToString().ToLower())
							.Replace('_', ' ');
						c.x_position = xPos;
						c.y_position = 0;
						c.width = grid.width;
						c.height = grid.height;

						var wins = (await stratz.GetWinWeekForPosition(pos, currBracket, GameModeEnumType.ALL_PICK_RANKED))
									.Where(w => w.matchCount > 100)
									.OrderByDescending(w => w.WinRate)
									.Take(grid.topWins);
						c.hero_ids.AddRange(wins.Select(c => (int)c.heroId));
						pickConfig.categories.Add(c);
						xPos = c.x_position + c.width + 5;
					}

					// Add custom rows (if any)
					if(grid.custom?.Count > 0)
					{
						foreach(var custom in grid.custom)
						{
							// Skip if section is defined, but has no heroes
							if(custom.heroes.Count == 0) continue;

							Console.WriteLine($"Adding custom grid '{custom.name}'");
							var c = new Dota2.Category();
							c.category_name = custom.name;
							c.x_position = xPos;
							c.y_position = 0;
							c.width = grid.width;
							c.height = grid.height;

							foreach(var h in custom.heroes)
							{
								var hero = await stratz.GetHeroByDisplayName(h);

								if(hero != null)
								{
									c.hero_ids.Add(hero.id);
								}
							}
							pickConfig.categories.Add(c);

							xPos = c.x_position + c.width + 5;
						}
					}

					// Add config to the grid menu
					heroGrid.configs.Add(pickConfig);

					bracketNum++;
				}

				configNum++;
			}

			File.WriteAllText(config.gridPath, System.Text.Json.JsonSerializer.Serialize(heroGrid));

			Console.WriteLine("Done, press any key to exit");
			Console.ReadKey();
		}
	}
}
