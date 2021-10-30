namespace Dota2Meta.Stratz.Types
{
	public record GameVersionType
	{
		public byte id { get; set; }
		public string name { get; set; }
		public long asOfDateTime { get; set; }
	}
}