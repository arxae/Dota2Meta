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

		public async Task<List<HeroWinType>> GetWinWeekForPosition(MatchPlayerPositionType position, RankBracket bracket)
		{
			var req = new GraphQLRequest
			{
				Query = @"
				query getWinWeekForPosition($pos: [MatchPlayerPositionType], $bracket: [RankBracket]) {
					heroStats {
						winWeek(take: 1, positionIds: $pos, bracketIds: $bracket) {
							heroId,
							winCount
						}
					}
				}",
				Variables = new
				{
					pos = new MatchPlayerPositionType[] { position },
					bracket = new RankBracket[] { bracket }
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
	}
}