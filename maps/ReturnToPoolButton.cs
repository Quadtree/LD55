using Godot;
using System;

public class ReturnToPoolButton : Button
{
    [Export]
    bool StartMusic = false;

    public override void _Ready()
    {
        Connect("pressed", this, nameof(StartLevel));
    }

    void StartLevel()
    {
        if (StartMusic && GetTree().Root.FindChildByName<AudioStreamPlayer>("BGM") == null)
        {
            var audioStreamPlayer = new AudioStreamPlayer();
            GetTree().Root.AddChild(audioStreamPlayer);
            audioStreamPlayer.Name = "BGM";
            audioStreamPlayer.Stream = GD.Load<AudioStream>("res://music/bgm.ogg");
            audioStreamPlayer.VolumeDb = -10;
            audioStreamPlayer.Play();
        }

        InterLevelState.Singleton = new InterLevelState();

        GetTree().ChangeScene("res://maps/LevelStartScreen.tscn");
    }
}
