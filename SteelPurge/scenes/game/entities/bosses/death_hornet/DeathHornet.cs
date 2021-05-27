using Godot;
using System;

public class DeathHornet : Boss
{
    private enum AttackMode
    {
        Rush,
        KamikazeRogues,
        Flight,
        RogueBombardment,
    }

    private static readonly PackedScene RogueScene
        = GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/HornetRogue.tscn");

    [Export] public uint CriticalDamageByRogue = 400u;
    [Export] public uint MaxRoguesShotOnKamikazeMode = 3;
    [Export] public uint MaxRoguesHitsOnFlightMode = 2;
    [Export] public uint PlayerDamage = 65u;
    [Export] public float RiseSpeed = 100;
    [Export] public float FlightStrafeSpeed = 60;
    [Export] public float GroundStrafeSpeed = 40;
    [Export] public float RushSpeed = 180;
    [Export] public float RegularRogueSpawnTime = 1.4f;
    [Export] public float FastRogueSpawnTime = 0.6f;

    public int StrafeDirection = -1;
    public int LookingDirection = -1;
    public float StrafeMargin = 48;

    private CollisionShape2D _criticalShape;
    private CollisionShape2D _rogueDamageShape;

    private Position2D _topRogueSpawnPoint;
    private Position2D _bottomRogueSpawnPoint;
    private Position2D _leftRogueSpawnPoint;
    private Position2D _rightRogueSpawnPoint;

    private Timer _rogueSpawnTimer;
    private Timer _rushWaitTimer;
    private Timer _rushStartDelayTimer;
    private Timer _rushRecoveryTimer;
    private Timer _flightDurationTimer;
    private Timer _bombardmentDurationTimer;

    private bool _playerAlreadyInsideLethalArea = false;
    private bool _isRushing = false;
    private float _kamikazeRogueModeStrafeAmount = 0;
    private uint _kamikazeRogueModeRoguesLaunched = 0;
    private bool _flightModeIsAscending = false;
    private bool _flightModeIsDescending = false;
    private uint _rogueHitsTakenInFlightMode = 0;
    private AttackMode _currentAttackMode = AttackMode.KamikazeRogues;

    public override void _Ready()
    {
        base._Ready();
        _criticalShape = GetNode<CollisionShape2D>("CriticalHitbox/CriticalShape");
        _rogueDamageShape = GetNode<CollisionShape2D>("RogueDamageArea/CollisionShape2D");
        _topRogueSpawnPoint = GetNode<Position2D>("TopRogueSpawnPoint");
        _bottomRogueSpawnPoint = GetNode<Position2D>("BottomRogueSpawnPoint");
        _leftRogueSpawnPoint = GetNode<Position2D>("LeftRogueSpawnPoint");
        _rightRogueSpawnPoint = GetNode<Position2D>("RightRogueSpawnPoint");
        _rogueSpawnTimer = GetNode<Timer>("RogueSpawnTimer");
        _rushWaitTimer = GetNode<Timer>("RushWaitTimer");
        _rushStartDelayTimer = GetNode<Timer>("RushStartDelayTimer");
        _rushRecoveryTimer = GetNode<Timer>("RushRecoveryTimer");
        _flightDurationTimer = GetNode<Timer>("FlightDurationTimer");
        _bombardmentDurationTimer = GetNode<Timer>("BombardmentDurationTimer");

        // TODO: Remove this test later
        StartPhaseOne();
    }

    protected override void _OnMovement(float delta)
    {
        if (_playerAlreadyInsideLethalArea)
        {
            DetectedPlayer.TakeDamage(PlayerDamage, new Vector2(LookingDirection, 0));
        }

        var phaseTwoHp = 0.50f * BaseHitPoints;
        var phaseThreeHp = 0f * BaseHitPoints; // 0.25f

        if (Health <= phaseTwoHp)
        {
            if (CurrentPhase == BossPhase.One)
            {
                CurrentPhase = BossPhase.Two;
                StartPhaseTwo();
            }
        }

        if (Health <= phaseThreeHp)
        {
            if (CurrentPhase == BossPhase.Two)
            {
                CurrentPhase = BossPhase.Three;
                StartPhaseThree();
            }
        }

        switch (CurrentPhase)
        {
            case BossPhase.One:
                PhaseOne(delta);
                break;
            case BossPhase.Two:
                PhaseTwo(delta);
                break;
            case BossPhase.Three:
                PhaseThree(delta);
                break;
        }
    }


    private void ChangeAttackMode(AttackMode mode)
    {
        // Clear up things from the previous mode
        switch (_currentAttackMode)
        {
            case AttackMode.Rush:
                if (!_rushWaitTimer.IsStopped())
                    _rushWaitTimer.Stop();
                _rushStartDelayTimer.Stop();
                _criticalShape.SetDeferred("disabled", true);
                break;
            case AttackMode.KamikazeRogues:
                if (!_rushWaitTimer.IsStopped())
                    _rushWaitTimer.Stop();
                _rogueSpawnTimer.Stop();
                _kamikazeRogueModeRoguesLaunched = 0;
                break;
            case AttackMode.Flight:
                if (_rogueDamageShape.Disabled)
                    _rogueDamageShape.SetDeferred("disabled", true);
                break;
            case AttackMode.RogueBombardment:
                Velocity.x = 0;
                _rogueSpawnTimer.Stop();
                _rogueSpawnTimer.WaitTime = RegularRogueSpawnTime;
                break;
        }

        // Init stuff for when changing to new mode
        switch (mode)
        {
            case AttackMode.Rush:
                _rushStartDelayTimer.Start();
                break;
            case AttackMode.KamikazeRogues:
                _rushWaitTimer.Start();
                _rogueSpawnTimer.Start();
                break;
            case AttackMode.Flight:
                _rogueSpawnTimer.Start();
                _flightDurationTimer.Start();
                _flightModeIsAscending = true;
                Velocity.y = -RiseSpeed;
                break;
            case AttackMode.RogueBombardment:
                _rogueSpawnTimer.WaitTime = FastRogueSpawnTime;
                _rogueSpawnTimer.Start();
                _bombardmentDurationTimer.Start();
                break;
        }

        _currentAttackMode = mode;
    }

    private void StartPhaseOne()
    {
        _rogueSpawnTimer.Start();
        _rushWaitTimer.Start();
    }

    private void PhaseOne(float delta)
    {
        switch (_currentAttackMode)
        {
            case AttackMode.Rush:
                if (_isRushing && IsOnWall())
                {
                    _isRushing = false;
                    LookingDirection *= -1;
                    _rushRecoveryTimer.Start();
                }

                break;
            case AttackMode.KamikazeRogues:
                if (_kamikazeRogueModeRoguesLaunched == MaxRoguesShotOnKamikazeMode)
                {
                    Velocity.x = -LookingDirection * FlightStrafeSpeed;
                    break;
                }

                Velocity.x = StrafeDirection * FlightStrafeSpeed;
                _kamikazeRogueModeStrafeAmount += Velocity.x * delta;
                if (_kamikazeRogueModeStrafeAmount > StrafeMargin && StrafeDirection > 0)
                {
                    StrafeDirection = -1;
                }
                else if (_kamikazeRogueModeStrafeAmount < -StrafeMargin && StrafeDirection < 0)
                {
                    StrafeDirection = 1;
                }

                break;
        }

        /*
        var verticalDirection = Mathf.Sign(ParentWorld.PlayerNode.Position.y - Position.y);
        if (verticalDirection < 0)
        {
            if (_currentAttackMode != AttackMode.Fireballs)
                ChangeAttackMode(AttackMode.Fireballs);
        }
        else if (verticalDirection > 0)
        {
            if (_currentAttackMode != AttackMode.KamikazeRogues)
                ChangeAttackMode(AttackMode.KamikazeRogues);
        }
        */
    }

    private void StartPhaseTwo()
    {
        ChangeAttackMode(AttackMode.Flight);
    }

    private void PhaseTwo(float delta)
    {
        switch (_currentAttackMode)
        {
            case AttackMode.Flight:
                if (_flightModeIsAscending)
                {
                    if (IsOnCeiling())
                    {
                        Velocity.y = 0;
                        StrafeDirection = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
                        _rogueSpawnTimer.Start();
                        _flightModeIsAscending = false;
                        _rogueDamageShape.SetDeferred("disabled", false);
                    }

                    break;
                }

                if (_flightModeIsDescending)
                {
                    Velocity.x = 0;
                    if (IsOnFloor())
                    {
                        Velocity.y = 0;
                        ChangeAttackMode(AttackMode.RogueBombardment);
                        _flightModeIsDescending = false;
                    }

                    break;
                }


                var distance = ParentWorld.PlayerNode.Position.x - Position.x;
                if (distance < -StrafeMargin)
                    StrafeDirection = -1;
                else if (distance > StrafeMargin)
                    StrafeDirection = 1;

                Velocity.x = StrafeDirection * FlightStrafeSpeed;
                break;
            case AttackMode.RogueBombardment:
                Velocity.x = LookingDirection * FlightStrafeSpeed;
                break;
        }
    }

    private void StartPhaseThree()
    {
    }

    private void PhaseThree(float delta)
    {
    }

    private void ShootRogueFromSide(int direction)
    {
        var position = direction < 0 ? _leftRogueSpawnPoint.Position : _rightRogueSpawnPoint.Position;
        var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene, position + Position);
        rogue.Direction = direction;
        if (_currentAttackMode != AttackMode.KamikazeRogues) return;
        _kamikazeRogueModeRoguesLaunched++;
        if (_kamikazeRogueModeRoguesLaunched == MaxRoguesShotOnKamikazeMode) _rogueSpawnTimer.Stop();
    }

    private void DropRogueFromBelow()
    {
        var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene,
            _bottomRogueSpawnPoint.Position + Position);
        rogue.Direction = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
    }

    private void _OnRogueHit(HornetRogue body)
    {
        TakeDamage(CriticalDamageByRogue, Vector2.Zero);
        body.QueueFree();
        var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, body.Position);
        scrap.Count = body.ScrapDropKilled;
        _rogueHitsTakenInFlightMode++;
        if (_rogueHitsTakenInFlightMode != MaxRoguesHitsOnFlightMode) return;
        _flightDurationTimer.Stop();
    }

    private void _OnSpawnRogue()
    {
        if (_currentAttackMode == AttackMode.RogueBombardment)
        {
            ShootRogueFromTop(LookingDirection = -LookingDirection);
            return;
        }

        if (_currentAttackMode != AttackMode.Flight)
        {
            ShootRogueFromSide(LookingDirection);
            return;
        }

        DropRogueFromBelow();
    }

    private void ShootRogueFromTop(int direction)
    {
        var rogue = ParentWorld.Entities.SpawnEntityDeferred<HornetRogue>(RogueScene,
            _topRogueSpawnPoint.Position + Position);
        rogue.Direction = direction;
    }

    private void _OnAttackPlayer(Player player)
    {
        if (_playerAlreadyInsideLethalArea) return;
        player.TakeDamage(PlayerDamage, new Vector2(LookingDirection, 0));
        _playerAlreadyInsideLethalArea = true;
    }

    private void _OnPlayerExitLethalArea(Player body)
    {
        _playerAlreadyInsideLethalArea = false;
    }

    private void _OnRushStart()
    {
        ChangeAttackMode(AttackMode.Rush);
    }

    private void _OnPerformRush()
    {
        _isRushing = true;
        Velocity.x = RushSpeed * LookingDirection;
        _criticalShape.SetDeferred("disabled", false);
    }

    private void _OnRecoveredFromRush()
    {
        ChangeAttackMode(AttackMode.KamikazeRogues);
    }

    private void _OnFlightEnd()
    {
        Velocity.y = 500;
        _rogueSpawnTimer.Stop();
        _flightModeIsDescending = true;
        _rogueHitsTakenInFlightMode = 0;
        _rogueDamageShape.SetDeferred("disabled", true);
    }

    private void _OnBombardmentEnd()
    {
        ChangeAttackMode(AttackMode.Flight);
    }
}