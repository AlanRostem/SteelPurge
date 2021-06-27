using Godot;
using System;

public class AR43Executor : Enemy
{
	private readonly PackedScene BulletScene =
		GD.Load<PackedScene>("res://scenes/game/entities/enemies/ar43_executor/ExecutorBullet.tscn");

	public int Direction = -1;
	[Export] public float WalkSpeed = 57;
	[Export] public uint MeleeDamage = 40;
	private RayCast2D _groundScanner;

	private bool _startWalking = false;
	private Timer _approachIntervalTimer;
	private Timer _standIntervalTimer;
	private Timer _fireRateTimer;
	
	[Signal]
	public delegate void TriggerApproach();

	[Signal]
	public delegate void TriggerStand();

	public override void OnEnableAi()
	{
		_fireRateTimer.Start();
		_standIntervalTimer.Start();
	}

	public override void OnDisableAi()
	{
		_approachIntervalTimer.Stop();
		_standIntervalTimer.Stop();
		_fireRateTimer.Stop();
		_startWalking = false;
	}

	public override void _Ready()
	{
		base._Ready();
		PlayerDetectionRange = 160;
		_groundScanner = GetNode<RayCast2D>("GroundScanner");
		_approachIntervalTimer = GetNode<Timer>("ApproachIntervalTimer");
		_standIntervalTimer = GetNode<Timer>("StandIntervalTimer");
		_fireRateTimer = GetNode<Timer>("FireRateTimer");
	}
	
	protected override void _ProcessWhenPlayerDetected(Player player)
	{
		Direction = Mathf.Sign(player.Position.x - Position.x);

		StopMovingX();
		if (_startWalking && _groundScanner.IsColliding())
			MoveX(Direction * WalkSpeed);
	}

	protected override void _ProcessWhenPlayerNotSeen()
	{
		StopMovingX();
	}

	private void _OnApproach()
	{
		_startWalking = false;
		EmitSignal(nameof(TriggerStand));
	}
	
	private void _OnStand()
	{
		_startWalking = true;
		EmitSignal(nameof(TriggerApproach));
	}
	
	private void _OnFire()
	{
		var bullet = ParentWorld.Entities.SpawnEntityDeferred<HostileProjectile>(BulletScene, Position);
		bullet.DamageDirection = Direction;
		var angle = ParentWorld.PlayerNode.Position.AngleToPoint(Position);
		bullet.DirectionAngle = Mathf.Rad2Deg(angle);
		bullet.Rotation = angle;
		bullet.InitWithAngularVelocity();
	}
	
	private void _OnPlayerMelee(object body)
	{
		var player = (Player)body;
		AttackPlayer(MeleeDamage, player, new Vector2(Direction, 0));
	}
}
