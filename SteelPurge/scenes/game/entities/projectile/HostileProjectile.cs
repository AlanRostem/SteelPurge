using Godot;

public class HostileProjectile : KinematicEntity
{
	[Export] public float DirectionAngle = 0;
	[Export] public int DamageDirection = 0;
	[Export] public float MaxVelocity = 250;
	private bool _hasDisappeared = false;

	public override void _Ready()
	{
		base._Ready();
		CurrentCollisionMode = CollisionMode.Move;
	}

	public void InitWithAngularVelocity()
	{
		var angle = Mathf.Deg2Rad(DirectionAngle);
		Velocity = new Vector2(
			MaxVelocity * Mathf.Cos(angle),
			MaxVelocity * Mathf.Sin(angle));
	}

	public void InitWithHorizontalVelocity()
	{
		Velocity = new Vector2(DamageDirection * MaxVelocity, 0);
	}

	private void _OnBodyHit(object body)
	{
		if (body is Player player)
		{
			// TODO: Consider checking the enemy's lethality
			player.TakeDamage(new Vector2(DamageDirection, 0));
		}

		if (!_hasDisappeared)
		{
			_hasDisappeared = true;
			_OnDisappear();
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}
	
	private void _OnAreaHit(object area)
	{
		if (!_hasDisappeared)
		{
			_hasDisappeared = true;
			_OnDisappear();
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
		}
	}

	public virtual void _OnDisappear()
	{
		ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
}



