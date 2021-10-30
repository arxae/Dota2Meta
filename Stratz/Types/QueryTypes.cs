namespace Dota2Meta.Stratz.Types
{
	public record StratzResponseType
	{
		public ConstantsQuery constants { get; set; }
		public HeroStatsQuery heroStats { get; set; }
	}
}
