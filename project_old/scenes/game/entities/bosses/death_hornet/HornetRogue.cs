using Godot;
using System;

public class HornetRogue : Enemy
{
	[Export] public int Direction = -1;
	private AnimatedSprite _sprite;
	public bool Fly = false;

	public override void _Init()
	{
		base._Init();
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_sprite.FlipH = Direction < 0;
	}

	protected override void _OnMovement(float delta)
	{
		base._OnMovement(delta);
		if (IsOnFloor())
			VelocityX = Direction * 110;
		else if (!Fly)
			VelocityX = 0;
	}

	public override void _OnCollision(KinematicCollision2D collider)
	{
		if (IsOnWall() || IsOnCeiling())
			ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}

	public override void TakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType,
		bool isCritical = false)
	{
		if (direction.y != 0)
		{
			VelocityY = 0;
			IsGravityEnabled = false;
		}

		base.TakeDamage(damage, direction, damageType, isCritical);
	}

	private void _OnPlayerEnterExplosiveArea(Player player)
	{
		AttackPlayer(player, new Vector2(Direction, 0));
		if (IsCurrentlyLethal) ParentWorld.CurrentSegment.Entities.RemoveEntity(this);
	}
}
