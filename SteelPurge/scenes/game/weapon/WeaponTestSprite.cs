using Godot;
using System;

public class WeaponTestSprite : Sprite
{
	private Weapon _weapon;
	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
	}

	public override void _Process(float delta)
	{
		if (Scale.x != _weapon.OwnerPlayer.HorizontalLookingDirection)
		{
			Scale = new Vector2(_weapon.OwnerPlayer.HorizontalLookingDirection, 1);
		}
		
		if (_weapon.OwnerPlayer.IsAimingDown)
		{
			Rotation = _weapon.OwnerPlayer.HorizontalLookingDirection * Mathf.Pi / 2f;
		}
		else if (_weapon.OwnerPlayer.IsAimingUp)
		{
			Rotation = -_weapon.OwnerPlayer.HorizontalLookingDirection * Mathf.Pi / 2f;
		}
		else
		{
			Rotation = 0;
		}
	}
}
