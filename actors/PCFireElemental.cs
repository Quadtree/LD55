using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class PCFireElemental : KinematicBody, Actor, HasFaction
{
    public float Mana = 50;

    public float SummonerSkill => GetTree().CurrentScene.FindChildByPredicate<Human>(it => it.IsSummoner)?.EffectiveSummonerSkill ?? 0;

    public float GlobalCooldown = 0;

    public bool HasBrokenFree = false;

    public float FirePoints = 0;

    public Spatial AsSpatial => this;
    public Actor AsActor => this;

    bool Moving;

    [Export]
    public float ManaRegenPerSecond = 10f;

    public float TimeBasedBreakoutPower = 0f;
    public float LevelBasedBreakoutPower = 0f;
    public float AttemptBasedBreakoutPower = 0f;

    public const float GLOBAL_COOLDOWN = 0.75f;
    public const float FIREBALL_MANA_COST = 40;
    public const float FLAME_SLASH_MANA_COST = 20;

    public const float DECAY_RATE = 7;

    public float FirepowerModifier => 1f + (InterLevelState.Singleton.FirepowerUpgrades * 0.5f);

    [Export]
    public bool Active = true;

    public override void _Ready()
    {
        LevelBasedBreakoutPower = InterLevelState.Singleton.PlayerBreakBonus;
    }

    public override void _Process(float delta)
    {
        if (!Active)
        {
            //this.FindChildByType<AnimationPlayer>().Play("Idle");
            return;
        }

        Moving = false;
        if (Input.IsActionPressed("move"))
        {
            var dest = Picking.PickPointAtCursor(this);

            if (dest != null)
            {
                var cam = this.FindChildByType<Camera>();

                var ct = cam.GlobalTransform;

                var destVal = dest.Value;
                destVal.y = this.GetGlobalLocation().y;

                LookAt(destVal, Vector3.Up);

                cam.GlobalTransform = ct;

                MoveAndSlide((destVal - GlobalTranslation).Normalized() * (6 + 3 * InterLevelState.Singleton.SpeedUpgrades));
                Moving = true;

                for (var i = 0; i < GetSlideCount(); ++i)
                {
                    var col = GetSlideCollision(i);
                    if (col.Collider is WardingRune wr)
                    {
                        wr.Health -= delta * 5;
                    }
                }
            }
        }

        //TimeBasedBreakoutPower += delta * 1.2f;

        Util.SpeedUpPhysicsIfNeeded();

        GlobalCooldown = Util.Clamp(GlobalCooldown + delta, 0, 2);

        Mana = Util.Clamp(Mana + ManaRegenPerSecond * delta, 0, 100);

        var flammable = this.FindChildByType<Flammable>();
        var damagable = this.FindChildByType<Damagable>();

        if (flammable.Heat < 0)
        {
            damagable.Health += flammable.Heat;
            flammable.Heat = 0;
        }

        damagable.Health -= delta * DECAY_RATE;

        if (Input.IsActionJustPressed("cheat_finish_level"))
        {
            this.FindChildByType<Damagable>().Health = 1;
        }

        if (Input.IsActionJustPressed("cheat_grant_fire_quota"))
        {
            FirePoints += 100;
        }

        if (this.FindChildByType<Damagable>().Health <= 0)
        {
            // end level
            InterLevelState.Singleton.LastLevelFirePoints = FirePoints;
            InterLevelState.Singleton.TotalFirePoints += FirePoints;

            if (FirePoints >= InterLevelState.Singleton.CurrentFireQuota && InterLevelState.Singleton.Level >= 4)
            {
                GetTree().ChangeScene("res://maps/WinGameScreen.tscn");
            }
            else
            {
                GetTree().ChangeScene("res://maps/LevelOverScreen.tscn");
            }
        }

        if (GlobalCooldown < 1)
        {
            this.FindChildByType<AnimationPlayer>().Play("Shoot");
        }
        else if (Moving)
        {
            this.FindChildByType<AnimationPlayer>().Play("Run");
        }
        else
        {
            this.FindChildByType<AnimationPlayer>().Play("Idle");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        if (!Active) return;

        base._PhysicsProcess(delta);

        if (Input.IsActionPressed("action") && GlobalCooldown >= 1)
        {
            var dest = Picking.PickPointAtCursor(this);

            if (dest != null)
            {
                GD.Print($"Dist={dest.Value.DistanceTo(GlobalTranslation)}");

                var validTargetArea = HasBrokenFree;

                if (!validTargetArea)
                {
                    var targetAreaPos = GetTree().CurrentScene.FindChildByType<TargetArea>()?.GlobalTranslation;

                    if (targetAreaPos != null && dest.Value.DistanceTo(targetAreaPos.Value) > 4)
                    {
                        // we are trying to break free
                        if (Mana >= FIREBALL_MANA_COST)
                        {
                            var breakFreeRoll = Util.RandInt(0, 100) + TimeBasedBreakoutPower + LevelBasedBreakoutPower + AttemptBasedBreakoutPower;
                            if (breakFreeRoll >= SummonerSkill)
                            {
                                // we have broken free
                                HasBrokenFree = true;
                                validTargetArea = true;
                                GetTree().CurrentScene.FindChildByType<TargetArea>().QueueFree();
                            }
                            else
                            {
                                // failed to break free
                                Mana -= FIREBALL_MANA_COST;
                                AttemptBasedBreakoutPower += 5;
                                GlobalCooldown = 0;
                            }
                        }
                    }
                    else
                    {
                        validTargetArea = true;
                    }
                }

                if (validTargetArea)
                {
                    GD.Print("Fireball!");
                    Fireball(dest.Value);
                }
            }
        }

        if (Util.RandChance(delta * 2f))
        {
            var targetAreaPos = GetTree().CurrentScene.FindChildByType<TargetArea>()?.GlobalTranslation;

            foreach (var it in GetTree().CurrentScene.FindChildrenByType<Flammable>()
                .Where(it => HasBrokenFree || it.GlobalTranslation.DistanceSquaredTo(targetAreaPos ?? new Vector3()) < Mathf.Pow(4f, 2.0f))
                .Where(it => it.GlobalTranslation.DistanceSquaredTo(GlobalTranslation) < Mathf.Pow(2.5f, 2.0f)))
            {
                it.Heat += 75 * FirepowerModifier;
            }
        }
    }

    public void FlameSlash()
    {
        if (Mana < FLAME_SLASH_MANA_COST) return;
        if (GlobalCooldown < GLOBAL_COOLDOWN) return;

        Mana -= FLAME_SLASH_MANA_COST;
        GlobalCooldown = 0;

        foreach (var it in this.FindChildByType<Area>().GetOverlappingBodies())
        {
            GD.Print(it);
            Flammable.AddHeat((Node)it, 150);
        }
    }

    public void Fireball(Vector3 target)
    {
        if (Mana < FIREBALL_MANA_COST) return;
        if (GlobalCooldown < GLOBAL_COOLDOWN) return;

        Mana -= FIREBALL_MANA_COST;
        GlobalCooldown = 0;

        var fb = GD.Load<PackedScene>("res://actors/Fireball.tscn").Instance<Fireball>();
        GetTree().CurrentScene.AddChild(fb);
        fb.GlobalTranslation = GlobalTranslation;
        fb.Target = target;
    }

    public int FactionId => HasBrokenFree ? 2 : 0;
}
