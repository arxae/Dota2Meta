namespace Dota2Meta.Stratz
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using GraphQL;
	using GraphQL.Client.Http;
	using Types;

	public class StratzProvider
	{
		readonly GraphQLHttpClient client;
		List<HeroType> _allHeroes;

		public StratzProvider(GraphQLHttpClient gqlClient)
		{
			client = gqlClient;
		}

		public async Task<List<HeroWinType>> GetWinWeekForPosition(MatchPlayerPositionType position, RankBracket bracket, GameModeEnumType gameMode)
		{
			var req = new GraphQLRequest
			{
				Query = @"
				query getWinWeekForPosition($pos: [MatchPlayerPositionType], $bracket: [RankBracket], $gameMode: [GameModeEnumType]) {
					heroStats {
						winWeek(take: 1, positionIds: $pos, bracketIds: $bracket, gameModeIds: $gameMode) {
							heroId
							winCount
							matchCount
						}
					}
				}",
				Variables = new
				{
					pos = new MatchPlayerPositionType[] { position },
					bracket = new RankBracket[] { bracket },
					gameMode = new GameModeEnumType[] { gameMode }
				}
			};

			var res = await client.SendQueryAsync<StratzResponseType>(req);

			return res.Data.heroStats.winWeek.ToList();
		}

		public async Task<string> GetLatestVersionNumber()
		{
			var req = new GraphQLRequest
			{
				Query = @"
				query getLatestVersions {
					constants {
						gameVersions {
							id
						}
					}
				}"
			};

			// Get higehst id
			byte latestId = (await client.SendQueryAsync<StratzResponseType>(req))
				.Data.constants.gameVersions[0]
				.id;

			req = new GraphQLRequest
			{
				Query = @"
				query getVersionString($id: Short!) {
					constants {
						gameVersion(id: $id) {
						name
						}
					}
				}",
				Variables = new { id = latestId }
			};

			return (await client.SendQueryAsync<StratzResponseType>(req))
				.Data.constants.gameVersion.name;
		}

		public async Task<List<HeroType>> GetAllHeroes()
		{
			if(_allHeroes != null) return _allHeroes;

			var req = new GraphQLRequest
			{
				Query = @"
				query getAllHeroes {
					constants {
						heroes {
							id
							displayName
						}
					}
				}"
			};

			var res = await client.SendQueryAsync<StratzResponseType>(req);

			_allHeroes = res.Data.constants.heroes.ToList();

			return _allHeroes;
		}

		public async Task<HeroType> GetHeroByDisplayName(string displayName)
		{
			if(_allHeroes == null) await GetAllHeroes();

			var hero = _allHeroes.FirstOrDefault(h => h.displayName.Equals(displayName, System.StringComparison.OrdinalIgnoreCase));

			if(hero == null)
			{
				throw new System.ArgumentException($"Unable to find hero with displayname '{displayName}'");
			}

			return hero;
		}

		public async Task<HeroType> GetHeroById(byte id)
		{
			if(_allHeroes == null) await GetAllHeroes();

			var hero = _allHeroes.FirstOrDefault(h => h.id == id);

			if(hero == null)
			{
				throw new System.ArgumentException($"Unable to find hero with id '{id}'");
			}

			return hero;
		}

		public async Task<IEnumerable<HeroPositionDetailType>> GetPositionDetails(byte heroId)
		{
			var req = new GraphQLRequest
			{
				Query = @"
				query getHeroPositionStats($heroId: Short!) {
					heroStats {
						position(heroId: $heroId) {
							position
							count
							wins
							kills
							deaths
							assists
							cs
							dn
							heroDamage
							towerDamage
						}
					}
				}",
				Variables = new {
					heroId = heroId
				}
			};

			var res = await client.SendQueryAsync<StratzResponseType>(req);

			return res.Data.heroStats.position.ToList();
		}
	}
}