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
	
	
}
