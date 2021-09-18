using Godot;
using System;
using Godot.Collections;

public class AR43Executor : Enemy
{
	private static readonly PackedScene BulletScene =
		GD.Load<PackedScene>("res://scenes/game/entities/enemies/ar43_executor/ExecutorBullet.tscn");

	public int Direction = -1;
	[Export] public float WalkSpeed = 57;
	private RayCast2D _groundScanner;

	private bool _startWalking = false;
	private CustomTimer _approachIntervalTimer;
	private CustomTimer _standIntervalTimer;
	private CustomTimer _fireRateTimer;

	public override Dictionary<string, object> ExportEntityData()
	{
		var data = new EntityData(base.ExportEntityData());
		data.SetTimer(nameof(_approachIntervalTimer), _approachIntervalTimer);
		data.SetTimer(nameof(_standIntervalTimer), _standIntervalTimer);
		data.SetTimer(nameof(_fireRateTimer), _fireRateTimer);
		data.SetAny(nameof(_startWalking), _startWalking);
		return data.GetJson();
	}

	public override void FeedEntityData(Dictionary<string, object> data)
	{
		base.FeedEntityData(data);
		var eData = new EntityData(data);
		eData.ConfigureTimer(nameof(_approachIntervalTimer), _approachIntervalTimer);
		eData.ConfigureTimer(nameof(_standIntervalTimer), _standIntervalTimer);
		eData.ConfigureTimer(nameof(_fireRateTimer), _fireRateTimer);
		_startWalking = eData.GetAny<bool>(nameof(_startWalking));
	}

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

	public override void _Init()
	{
		base._Init();
		_groundScanner = GetNode<RayCast2D>("GroundScanner");
		_approachIntervalTimer = GetNode<CustomTimer>("ApproachIntervalTimer");
		_standIntervalTimer = GetNode<CustomTimer>("StandIntervalTimer");
		_fireRateTimer = GetNode<CustomTimer>("FireRateTimer");
	}

	protected override void _ProcessWhenPlayerDetected(Player player)
	{
		Direction = Mathf.Sign(player.Position.x - Position.x);

		StopMovingX();
		if (_startWalking && _groundScanner.IsColliding())
			MoveX(Direction * WalkSpeed);
	}

	protected override void OnPlayerDetected(Player player)
	{
		_fireRateTimer.Start();
		_standIntervalTimer.Start();
		_approachIntervalTimer.Stop();
		_startWalking = false;
	}

	protected override void OnPlayerVisualLost(Player player)
	{
		_fireRateTimer.Stop();
		_approachIntervalTimer.Stop();
		_standIntervalTimer.Stop();
		_startWalking = false;
	}

	protected override void _ProcessWhenPlayerNotSeen()
	{
		StopMovingX();
	}

	private void _OnApproach()
	{
		_startWalking = false;
		_standIntervalTimer.Start();
		_fireRateTimer.Start();
	}

	private void _OnStand()
	{
		_startWalking = true;
		_approachIntervalTimer.Start();
		_fireRateTimer.Stop();
	}

	private void _OnFire()
	{
		var bullet = ParentWorld.CurrentSegment.Entities.SpawnEntityDeferred<HostileProjectile>(BulletScene, Position);
		bullet.DamageDirection = Direction;
		var angle = ParentWorld.PlayerNode.Position.AngleToPoint(Position);
		bullet.DirectionAngle = Mathf.Rad2Deg(angle);
		bullet.Rotation = angle;
		bullet.InitWithAngularVelocity();
	}

	private void _OnPlayerMelee(object body)
	{
		var player = (Player) body;
		AttackPlayer(player, new Vector2(Direction, 0));
	}
}
