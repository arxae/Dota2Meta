namespace Dota2Meta.Stratz.Types
{
	public record HeroType
	{
		public byte id { get; set; }
		public string name { get; set; }
		public string displayName { get; set; }
		public string shortName { get; set; }
		public string[] aliases { get; set; }
		public int gameVersionId { get; set; }
		public HeroAbilityType[] abilities { get; set; }
		public HeroRoleType[] roles { get; set; }
		public HeroLanguageType language { get; set; }
		public HeroTalentType[] talents { get; set; }
		public HeroStatType stats { get; set; }

		public override string ToString() => $"{id}: {displayName}";
	}

	public record HeroAbilityType
	{
		public byte slot { get; set; }
		public int gameVersionId { get; set; }
		public int abilityId { get; set; }
		public AbilityType ability { get; set; }
	}

	public record AbilityType
	{
		public byte id { get; set; }
		public string name { get; set; }
		public string uri { get; set; }
		public AbilityLanguageType language { get; set; }
		public AbilityStatType stat { get; set; }
		public AbilityAttributeType[] attributes { get; set; }
		public bool drawMatchPage { get; set; }
		public bool isTalent { get; set; }
	}

	public record AbilityLanguageType
	{
		public string displayName { get; set; }
		public string[] description { get; set; }
		public string[] attributes { get; set; }
		public string lore { get; set; }
		public string aghanimDescription { get; set; }
		public string shardDescription { get; set; }
		public string[] notes { get; set; }
	}

	public record AbilityStatType
	{
		public byte abilityId { get; set; }
		public int type { get; set; }
		public long behavior { get; set; }
		public long unitTargetType { get; set; }
		public int unitTargetTeam { get; set; }
		public long unitTargetFlags { get; set; }
		public int unitDamageType { get; set; }
		public int spellImmunity { get; set; }
		public float modifierSupportValue { get; set; }
		public byte modifierSupportBonus { get; set; }
		public bool isOnCastbar { get; set; }
		public bool isOnLearnbar { get; set; }
		public byte fightRecapLevel { get; set; }
		public bool isGrantedByScepter { get; set; }
		public bool hasScepterUpgrade { get; set; }
		public byte maxLevel { get; set; }
		public byte levelsBetweenUpgrades { get; set; }
		public byte requiredLevel { get; set; }
		public string hotKeyOverride { get; set; }
		public bool displayAdditionalHeroes { get; set; }
		public int[] castRange { get; set; }
		public int[] castRangeBuffer { get; set; }
		public float[] castPoint { get; set; }
		public float[] channelTime { get; set; }
		public float[] cooldown { get; set; }
		public float[] damage { get; set; }
		public float[] manaCost { get; set; }
		public bool isUltimate { get; set; }
		public string duration { get; set; }
		public string charges { get; set; }
		public string chargeRestoreTime { get; set; }
		public bool isGrantedByShard { get; set; }
		public AbilityDispellEnum dispellable { get; set; }
	}

	public record AbilityAttributeType
	{
		public string name { get; set; }
		public string value { get; set; }
		public byte linkedSpecialBonusAbilityId { get; set; }
		public bool requiresScepter { get; set; }
	}

	public record HeroRoleType
	{
		public byte roleId { get; set; }
		public byte level { get; set; }
	}

	public record HeroLanguageType
	{
		public string displayName { get; set; }
		public string lore { get; set; }
		public string hype { get; set; }
	}

	public record HeroTalentType
	{
		public byte abilityId { get; set; }
		public byte slot { get; set; }
	}

	public record HeroStatType
	{
		public bool enabled { get; set; }
		public float heroUnlockOrder { get; set; }
		public bool team { get; set; }
		public bool cMEnabled { get; set; }
		public bool newPlayerEnabled { get; set; }
		public string attackType { get; set; }
		public float startingArmor { get; set; }
		public float startingMagicArmor { get; set; }
		public float startingDamageMin { get; set; }
		public float startingDamageMax { get; set; }
		public float attackRate { get; set; }
		public float attackAnimationPoint { get; set; }
		public float attackAcquisitionRange { get; set; }
		public float attackRange { get; set; }
		public string primaryAttribute { get; set; }
		public float strengthBase { get; set; }
		public float strengthGain { get; set; }
		public float intelligenceBase { get; set; }
		public float intelligenceGain { get; set; }
		public float agilityBase { get; set; }
		public float agilityGain { get; set; }
		public float mpRegen { get; set; }
		public float moveSpeed { get; set; }
		public float moveTurnRate { get; set; }
		public float hpBarOffset { get; set; }
		public float visionDaytimeRange { get; set; }
		public float visionNighttimeRange { get; set; }
		public byte complexity { get; set; }
	}

	public record HeroPositionDetailType
	{
		public MatchPlayerPositionType position { get; set; }
		public int count { get; set; }
		public double wins { get; set; }
		public double kills { get; set; }
		public double deaths { get; set; }
		public double assists { get; set; }
		public double cs { get; set; }
		public double dn { get; set; }
		public double heroDamage { get; set; }
		public double towerDamage { get; set; }
	}
}