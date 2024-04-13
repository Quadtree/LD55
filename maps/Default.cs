using Godot;
using System;
using System.Linq;

public class Default : Spatial
{
    [Export]
    public float Difficulty = 10;

    public static float? DifficultyOverride;

    public override void _Ready()
    {
        if (DifficultyOverride != null) Difficulty = DifficultyOverride.Value;

        var difficultyIncreasingObjects = GetTree().CurrentScene.FindChildrenByType<DifficultyIncreasing>().Where(it => (it as Human)?.IsSummoner != true).ToList();

        while (difficultyIncreasingObjects.Count > 1 && difficultyIncreasingObjects.Sum(it => it.DifficultyAdded) > Difficulty)
        {
            var toDelete = Util.Choice(difficultyIncreasingObjects);
            ((Node)toDelete).QueueFree();
            difficultyIncreasingObjects.Remove(toDelete);
        }
    }

    public override void _Process(float delta)
    {

    }
}
