using Godot;
using System;

public class LevelDesc : Label
{
    Func<string> Templater;

    public override void _Ready()
    {
        var txt = Text;
        Templater = () => $"Greetings {InterLevelState.RANKS[InterLevelState.Singleton.Level - 1]}!\n\n" +
         txt.Replace("<Quota>", InterLevelState.Singleton.CurrentFireQuota.ToString())
            .Replace("##%", $"{InterLevelState.Singleton.SummonerSkill - InterLevelState.Singleton.PlayerBreakBonus}%");
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Text = Templater();
    }
}
