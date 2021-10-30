namespace Dota2Meta.Stratz.Types
{
	public record ConstantsQuery
	{
		public HeroType[] heroes { get; set; }
		public HeroType hero { get; set; }
		public GameVersionType[] gameVersions { get; set; }
		public GameVersionType gameVersion { get; set; }
	}
}