using Godot;
using System;
using System.Linq;

public class Default : Spatial
{
    [Export]
    public float Difficulty = 10;

    [Export]
    public float DiffMul = 1;

    public static float? DifficultyOverride;

    public override void _Ready()
    {
        if (DifficultyOverride != null) Difficulty = DifficultyOverride.Value;

        var difficultyIncreasingObjects = GetTree().CurrentScene.FindChildrenByType<DifficultyIncreasing>().Where(it => (it as Human)?.IsSummoner != true).ToList();

        while (difficultyIncreasingObjects.Count > 1 && difficultyIncreasingObjects.Sum(it => it.DifficultyAdded) > Difficulty * DiffMul)
        {
            var toDelete = Util.Choice(difficultyIncreasingObjects);
            ((Node)toDelete).QueueFree();
            difficultyIncreasingObjects.Remove(toDelete);
        }

        var summoner = GetTree().CurrentScene.FindChildByPredicate<Human>(it => it.IsSummoner);
        if (summoner != null) summoner.SummonerSkill = (int)(25 + Difficulty * 3);
    }

    public override void _Process(float delta)
    {

    }
}
