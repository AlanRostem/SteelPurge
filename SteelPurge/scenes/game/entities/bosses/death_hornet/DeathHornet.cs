using Godot;
using System;

public class DeathHornet : Boss
{
	private static readonly PackedScene RogueScene
		= GD.Load<PackedScene>("res://scenes/game/entities/bosses/death_hornet/HornetRogue.tscn");

	[Export] public uint CriticalDamageByRogue = 400u;
	[Export] public float RiseSpeed = 100;
	[Export] public float FlightStrafeSpeed = 80;

	public int StrafeDirection = -1;
	private Position2D _bottomRogueSpawnPoint;
	private Position2D _leftRogueSpawnPoint;
	private Position2D _rightRogueSpawnPoint;
	private Timer _rogueSpawnTimer;
	private Timer _firingTimer;

	public override void _Ready()
	{
		base._Ready();
		_bottomRogueSpawnPoint = GetNode<Position2D>("BottomRogueSpawnPoint");
		_leftRogueSpawnPoint = GetNode<Position2D>("LeftRogueSpawnPoint");
		_rightRogueSpawnPoint = GetNode<Position2D>("RightRogueSpawnPoint");
		_rogueSpawnTimer = GetNode<Timer>("RogueSpawnTimer");
		_firingTimer = GetNode<Timer>("FiringTimer");

		// TODO: Remove this test later
		_rogueSpawnTimer.Start();
	}

	protected override void _OnMovement(float delta)
	{
		var phaseTwoHp = 0.75f * BaseHitPoints;
		var phaseThreeHp = 0.50f * BaseHitPoints;

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
				CurrentPhase = BossPhase.Three;
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
			case BossPhase.Four:
				break;
		}
	}

	private void ShootFireball()
	{
	}

	private void PhaseOne(float delta)
	{
	}

	private void StartPhaseTwo()
	{
		Velocity.y = -RiseSpeed;
	}

	private void PhaseTwo(float delta)
	{
		if (IsOnCeiling())
		{
			Velocity.y = 0;
			StrafeDirection =  Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x);
		}

		if (IsOnWall())
		{
			StrafeDirection *= -1;
		}

		Velocity.x = StrafeDirection * FlightStrafeSpeed;
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
			ShootRogueFromSide(Mathf.Sign(ParentWorld.PlayerNode.Position.x - Position.x));
			return;
		}

		DropRogueFromBelow();
	}
}
