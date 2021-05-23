using Godot;
using System;

public class DeathHornet : Boss
{
	private enum AttackMode
	{
		Rush,
		KamikazeRogues,
		Fireballs,
		Flight,
		FlightWithFireballs,
	}

	private static readonly PackedScene RogueScene
		= GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/HornetRogue.tscn");

	private static readonly PackedScene FireballScene
		= GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/FireballProjectile.tscn");

	[Export] public uint CriticalDamageByRogue = 400u;
	[Export] public uint PlayerDamage = 65u;
	[Export] public uint MaxFireballShotsPerInterval = 3u;
	[Export] public float RiseSpeed = 100;
	[Export] public float FlightStrafeSpeed = 60;
	[Export] public float RushSpeed = 180;

	public int StrafeDirection = -1;
	public int LookingDirection = -1;
	public float StrafeMargin = 48;
	private Position2D _fireballSpawnPoint;
	private Position2D _bottomRogueSpawnPoint;
	private Position2D _leftRogueSpawnPoint;
	private Position2D _rightRogueSpawnPoint;
	private Timer _rogueSpawnTimer;
	private Timer _rushWaitTimer;
	private Timer _rushStartDelayTimer;
	private Timer _reloadTimer;
	private Timer _firingTimer;
	private uint _shotsFired = 0;
	private bool _playerAlreadyInsideLethalArea = false;
	private bool _isRushing = false;
	private AttackMode _currentAttackMode = AttackMode.KamikazeRogues;

	public override void _Ready()
	{
		base._Ready();
		_fireballSpawnPoint = GetNode<Position2D>("FireballSpawnPoint");
		_bottomRogueSpawnPoint = GetNode<Position2D>("BottomRogueSpawnPoint");
		_leftRogueSpawnPoint = GetNode<Position2D>("LeftRogueSpawnPoint");
		_rightRogueSpawnPoint = GetNode<Position2D>("RightRogueSpawnPoint");
		_rogueSpawnTimer = GetNode<Timer>("RogueSpawnTimer");
		_rushWaitTimer = GetNode<Timer>("RushWaitTimer");
		_reloadTimer = GetNode<Timer>("ReloadTimer");
		_firingTimer = GetNode<Timer>("FiringTimer");
		_rushStartDelayTimer = GetNode<Timer>("RushStartDelayTimer");

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
		var phaseThreeHp = 0.25f * BaseHitPoints;

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


	private void ShootFireball()
	{
		if (_shotsFired >= MaxFireballShotsPerInterval)
		{
			_reloadTimer.Start();
			return;
		}

		var fireball =
			ParentWorld.Entities.SpawnEntityDeferred<FireballProjectile>(FireballScene,
				_fireballSpawnPoint.Position + Position);
		fireball.DamageDirection = LookingDirection;
		fireball.InitWithHorizontalVelocity();
		_shotsFired++;
	}


	private void ChangeAttackMode(AttackMode mode)
	{
		// Clear up things from the previous mode
		switch (_currentAttackMode)
		{
			case AttackMode.Rush:
				_rushWaitTimer.Start();
				break;
			case AttackMode.KamikazeRogues:
				_rogueSpawnTimer.Stop();
				break;
			case AttackMode.Flight:
				break;
			case AttackMode.Fireballs:
				_firingTimer.Stop();
				break;
			case AttackMode.FlightWithFireballs:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		// Init stuff for when changing to new mode
		switch (mode)
		{
			case AttackMode.Rush:
				_rushStartDelayTimer.Start();
				break;
			case AttackMode.KamikazeRogues:
				_rogueSpawnTimer.Start();
				break;
			case AttackMode.Flight:
				_rogueSpawnTimer.Start();
				break;
			case AttackMode.Fireballs:
				ShootFireball();
				_firingTimer.Start();
				break;
			case AttackMode.FlightWithFireballs:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
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
		if (_currentAttackMode == AttackMode.Rush)
		{
			if (!_isRushing || !IsOnWall()) return;
			_isRushing = false;
			LookingDirection *= -1;
			ChangeAttackMode(AttackMode.KamikazeRogues);

			return;
		}
		
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
	}

	private void StartPhaseTwo()
	{
		Velocity.y = -RiseSpeed;
		ChangeAttackMode(AttackMode.Flight);
	}

	private void PhaseTwo(float delta)
	{
		if (IsOnCeiling())
		{
			Velocity.y = 0;
			StrafeDirection = Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
			_rogueSpawnTimer.Start();
		}

		var distance = ParentWorld.PlayerNode.Position.x - Position.x;
		if (distance < -StrafeMargin)
			StrafeDirection = -1;
		else if (distance > StrafeMargin)
			StrafeDirection = 1;

		Velocity.x = StrafeDirection * FlightStrafeSpeed;
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
	}

	private void _OnSpawnRogue()
	{
		if (CurrentPhase == BossPhase.One)
		{
			ShootRogueFromSide(LookingDirection);
			return;
		}

		DropRogueFromBelow();
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
	}

	private void _OnCanFire()
	{
		_shotsFired = 0;
		_firingTimer.Start();
	}
}
