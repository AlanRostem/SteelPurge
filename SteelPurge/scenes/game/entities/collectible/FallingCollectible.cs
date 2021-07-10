using Godot;
using System;

public class FallingCollectible : KinematicEntity
{
	private static readonly RandomNumberGenerator Rng = new RandomNumberGenerator();

	[Export] public float LungeSpeed = 50;
	[Export] public bool InteractToPickUp = false;

	private Player _player;
	public Sprite IconSprite { get; private set; }

	public override void _Init()
	{
		base._Init();
		Velocity = new Vector2(
			Rng.RandfRange(-1, 1) * LungeSpeed,
			Rng.Randf() * -LungeSpeed
		);

		IconSprite = GetNode<Sprite>("Sprite");
	}

	public virtual void OnCollected(Player player)
	{
	}

	protected override void _OnMovement(float delta)
	{
		if (IsOnFloor())
			VelocityX = 0;


		if (!InteractToPickUp) return;

		if (Input.IsActionJustPressed("interact") && _player != null)
		{
			if (CollectionCondition(_player))
			{
				OnCollected(_player);
				ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
			}
		}
	}

	private void _OnPlayerEnter(object body)
	{
		if (InteractToPickUp)
		{
			_player = (Player) body;
			return;
		}

		if (CollectionCondition((Player) body))
		{
			OnCollected((Player) body);
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}

	private void _OnPlayerExit(object body)
	{
		_player = null;
	}

	public virtual bool CollectionCondition(Player player)
	{
		return true;
	}
}
