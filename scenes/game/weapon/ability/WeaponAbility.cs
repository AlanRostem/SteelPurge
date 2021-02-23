using Godot;
using System;

public class WeaponAbility : Node2D
{
	private Weapon _weapon;
	
	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
	}
	
	public T GetWeapon<T>() where T : Weapon
	{
		return (T)_weapon;
	}

	public Weapon GetWeapon()
	{
		return _weapon;
	}
}
