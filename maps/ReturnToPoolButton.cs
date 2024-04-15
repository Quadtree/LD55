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
        if (StartMusic)
        {
            var audioStreamPlayer = new AudioStreamPlayer();
            GetTree().Root.AddChild(audioStreamPlayer);
            audioStreamPlayer.Stream = GD.Load<AudioStream>("res://music/bgm.ogg");
            audioStreamPlayer.VolumeDb = -5;
            audioStreamPlayer.Play();
        }

        GetTree().ChangeScene("res://maps/LevelStartScreen.tscn");
    }
}
