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
	
	[Signal]
	public delegate void TriggerApproach();

	[Signal]
	public delegate void TriggerStand();

	public override void _Ready()
	{
		base._Ready();
		PlayerDetectionRange = 160;
		_groundScanner = GetNode<RayCast2D>("GroundScanner");
	}
	
	protected override void _WhenPlayerDetected(Player player)
	{
		Direction = Mathf.Sign(player.Position.x - Position.x);

		Velocity.x = 0;
		if (_startWalking && _groundScanner.IsColliding())
			Velocity.x = Direction * WalkSpeed;
	}

	protected override void _WhenPlayerNotSeen()
	{
		Velocity.x = 0;
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
		var bullet = (HostileProjectile) BulletScene.Instance();
		bullet.Position = Position;
		bullet.DamageDirection = Direction;
		bullet.InitWithHorizontalVelocity();
		ParentWorld.AddChild(bullet);
	}
	
	private void _OnPlayerMelee(object body)
	{
		var player = (Player)body;
		player.TakeDamage(MeleeDamage, Direction);
	}
}
