using Godot;
using System;

public class LargeRock : KinematicEntity
{
	[Export] public uint Health = 10;
	
	private const uint MaxHealth = 10;
	
	private LifeHitbox _lifeHitbox;
	private AnimatedSprite _sprite;
	
	public override void _Init()
	{
		base._Init();
		_lifeHitbox = GetNode<LifeHitbox>("LifeHitbox");
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
		_lifeHitbox.CurrentHealth = Health;
		_lifeHitbox.Health = Health;
	}

	protected override void _OnMovement(float delta)
	{
		VelocityX = 0;
	}

	private void _OnHealthChanged(uint health)
	{
		var percentage = (float) health / MaxHealth;
		if (percentage <= .25f)
			_sprite.Animation = "crack_2";
		else if (percentage <= .50f)
			_sprite.Animation = "crack_1";
		else if (percentage <= .75f)
			_sprite.Animation = "crack_0";
		else 
			_sprite.Animation = "default";
	}
}
