using Godot;
using System;

public class LargeRock : KinematicEntity
{
	private LifeHitbox _lifeHitbox;
	private AnimatedSprite _sprite;
	
	public override void _Ready()
	{
		base._Ready();
		_lifeHitbox = GetNode<LifeHitbox>("LifeHitbox");
		_sprite = GetNode<AnimatedSprite>("AnimatedSprite");
	}
	
	private void _OnHealthChanged(uint health)
	{
		var percentage = (float) health / _lifeHitbox.Health;
		
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
