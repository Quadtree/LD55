public class InterLevelState
{
    public int Level;
    public int FirepowerUpgrades;
    public int SpeedUpgrades;
    public int BreakUpgrades;

    public int AvailableUpgradePoints => Level - FirepowerUpgrades - SpeedUpgrades - BreakUpgrades;

    public int Difficulty => Level * 2 + 3;

    public int SummonerSkill => (int)(25 + Difficulty * 3);
    public int PlayerBreakBonus => BreakUpgrades * 20;

    public static InterLevelState Singleton = new InterLevelState();
}