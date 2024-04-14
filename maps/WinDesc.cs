using Godot;
using System;

public class WinDesc : Label
{
    public override void _Ready()
    {
        Text += $"\n\nYour total fire points for all levels: {InterLevelState.Singleton.TotalFirePoints:n0}";
    }
}
