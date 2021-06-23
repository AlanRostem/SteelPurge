using Godot;
using System;

public class WeaponAbility : Node2D
{
	[Export] public Texture Icon;
	public bool IsActive = false;
	
	private Weapon _weapon;
	
	public override void _Ready()
	{
		_weapon = GetParent<Weapon>();
		_weapon.Connect(nameof(Weapon.SwitchedTo), this, nameof(OnSwitchTo));
	}
	
	public T GetWeapon<T>() where T : Weapon
	{
		return (T)_weapon;
	}

	public Weapon GetWeapon()
	{
		return _weapon;
	}

	public virtual void Activate()
	{
		
	}
	
	public virtual void DeActivate()
	{
		
	}

	public virtual void OnWeaponSwapped()
	{
		
	}

	public virtual void OnSwitchTo()
	{
		
	}
}
