public class InterLevelState
{
    public static string[] RANKS = {
        "Spark",
        "Candle",
        "Torch",
        "Campfire",
        "Bonfire",
        "Wildfire"
    };

    public int Level = 1;
    public int FirepowerUpgrades;
    public int SpeedUpgrades;
    public int BreakUpgrades;

    public int AvailableUpgradePoints => Level - FirepowerUpgrades - SpeedUpgrades - BreakUpgrades;

    public int Difficulty => Level * 2 + 3;

    public int SummonerSkill => (int)(25 + Difficulty * 3);
    public int PlayerBreakBonus => BreakUpgrades * 20;

    public int CurrentFireQuota => 20 + Level * 5;

    public float LastLevelFirePoints = 0;

    public static InterLevelState Singleton = new InterLevelState();
}