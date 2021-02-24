using Godot;
using System;

public class FiringDevice : WeaponAbility
{
	public override void _Ready()
	{
		base._Ready();
		GetWeapon().Connect(nameof(Weapon.Fired), this, nameof(OnFire));
        GetWeapon().FiringDevice = this;
    }

	public virtual void OnFire()
	{

	}
}
