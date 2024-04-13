using Godot;
using System;

public class UpgradeBox : VBoxContainer
{
    enum StatType
    {
        Firepower,
        Speed,
        Break
    }

    [Export]
    string TextTemplate;

    [Export]
    StatType SelectedStatType;

    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

    }

    void SetBase(int val)
    {
        if (SelectedStatType == StatType.Firepower) InterLevelState.Singleton.FirepowerUpgrades = val;
        if (SelectedStatType == StatType.Speed) InterLevelState.Singleton.SpeedUpgrades = val;
        if (SelectedStatType == StatType.Break) InterLevelState.Singleton.BreakUpgrades = val;
    }

    int GetBase()
    {
        if (SelectedStatType == StatType.Firepower) return InterLevelState.Singleton.FirepowerUpgrades;
        if (SelectedStatType == StatType.Speed) return InterLevelState.Singleton.SpeedUpgrades;
        if (SelectedStatType == StatType.Break) return InterLevelState.Singleton.BreakUpgrades;

        throw new Exception();
    }
}
