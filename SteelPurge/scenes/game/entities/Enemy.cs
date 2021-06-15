using Godot;
using System;
using System.Collections;
using Object = Godot.Object;

public class Enemy : KinematicEntity
{
    private static readonly Vector2 DamageNumberOffsetPos = new Vector2(-12, -20);

    protected static readonly PackedScene ScrapScene =
        GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");

    [Export] public uint ScrapDropHit = 2;
    [Export] public uint ScrapDropKilled = 25;

    [Export] public uint BaseHitPoints = 45;
    [Export] public float PlayerDetectionRange = 1000;
    [Export] public float KnockBackSpeed = 300;
    [Export] public bool CanBeKnockedBack = true;


    private bool _isDead;
    private bool _dropScrap;
    private bool _isKnockedBack;
    public Player DetectedPlayer { get; private set; }
    private Timer _meleeAffectedKnockBackTimer;
    private Timer _damageIndicationTimer;
    private DamageNumberGenerator _damageNumberGenerator;

    public override void _Ready()
    {
        base._Ready();
        Health = BaseHitPoints;
        _meleeAffectedKnockBackTimer = GetNode<Timer>("MeleeAffectedKnockBackTimer");
        _damageIndicationTimer = GetNode<Timer>("DamageIndicationTimer");
        _damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
    }

    public virtual void OnDie()
    {
    }

    public override void _Process(float delta)
    {
        base._Process(delta);
        if (_isDead)
        {
            var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
            scrap.Count = ScrapDropKilled;
            QueueFree();
        }

        if (_dropScrap)
        {
            _dropScrap = false;
            var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
            scrap.Count = ScrapDropHit;
        }

        var distance = Mathf.Abs(ParentWorld.PlayerNode.Position.x - Position.x);
        if (distance < PlayerDetectionRange)
        {
            if (DetectedPlayer is null)
                DetectedPlayer = ParentWorld.PlayerNode;
            _ProcessWhenPlayerDetected(ParentWorld.PlayerNode);
        }
        else
        {
            _ProcessWhenPlayerNotSeen();
        }
    }

    public override void TakeDamage(uint damage, Vector2 direction, bool isCritical = false)
    {
        if (damage >= Health)
        {
            if (!_isDead)
            {
                OnDie();
                // Assuming the player gave damage to the enemy
                ParentWorld.PlayerNode.PlayerInventory.IncrementKillCount();
                _isDead = true;
                _damageNumberGenerator.ShowDamageNumber(Health, Position + DamageNumberOffsetPos, ParentWorld);
                Health = 0;
            }
        }
        else
        {
            _dropScrap = true;
            Health -= damage;
            _damageNumberGenerator.ShowDamageNumber(damage, Position + DamageNumberOffsetPos, ParentWorld);
            if (!isCritical)
                Modulate = new Color(255, 255, 255);
            else
                Modulate = new Color(255, 0, 0);
            _damageIndicationTimer.Start();
            if (direction.x != 0 || direction.y != 0)
            {
                KnockBack(direction);
            }
        }
    }

    public void KnockBack(Vector2 direction, float speed)
    {
        if (!CanBeKnockedBack) return;
        ApplyForce(direction * speed);
        CanMove = false;
        _isKnockedBack = true;
        _meleeAffectedKnockBackTimer.Start();
    }

    public void KnockBack(Vector2 direction)
    {
        KnockBack(direction, KnockBackSpeed);
    }

    private void _OnVulnerableHitboxHit(uint damage, Vector2 knockBackDirection)
    {
        TakeDamage(damage, knockBackDirection);
    }

    protected virtual void _ProcessWhenPlayerDetected(Player player)
    {
    }

    protected virtual void _ProcessWhenPlayerNotSeen()
    {
    }

    private void _OnKnockBackEnd()
    {
        if (_isKnockedBack)
        {
            CanMove = true;
        }

        _isKnockedBack = false;
    }

    private void _OnDamageIndicationTimeout()
    {
        Modulate = new Color(1, 1, 1);
    }
}