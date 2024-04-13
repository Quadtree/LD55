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
        this.FindChildByName<Button>("IncButton").Connect("pressed", this, nameof(Increase));
        this.FindChildByName<Button>("DecButton").Connect("pressed", this, nameof(Decrease));
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        this.FindChildByType<Label>().Text = $"Increase {TextTemplate} ({GetBase()})";
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

    void Increase()
    {
        if (InterLevelState.Singleton.AvailableUpgradePoints > 0) SetBase(GetBase() + 1);
    }

    void Decrease()
    {
        if (GetBase() > 0) SetBase(GetBase() - 1);
    }
}
