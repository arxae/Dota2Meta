namespace Dota2Meta.Stratz.Types
{
	public record HeroStatsQuery
	{
		public HeroWinType[] winDay { get; set; }
		public HeroWinType[] winWeek { get; set; }
		public HeroWinType[] winMonth { get; set; }
		public HeroPositionDetailType[] position { get; set; }
	}

	public record HeroWinType
	{
		public long day { get; set; }
		public long week { get; set; }
		public long month { get; set; }
		public byte heroId { get; set; }
		public int winCount { get; set; }
		public int matchCount { get; set; }

		public float WinRate => (float)winCount / (float)matchCount;
	}
}