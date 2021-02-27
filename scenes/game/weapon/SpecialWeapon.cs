using Godot;
using System;

public class SpecialWeapon : Weapon
{
	[Export]
	public uint MaxReserveAmmo = 100;
	protected override void ReloadPerformed()
	{
		if (ClipSize > CurrentAmmo) {
			if (MaxReserveAmmo > (ClipSize - CurrentAmmo)) {
				var ammoDiff = ClipSize - CurrentAmmo;
				CurrentAmmo += ammoDiff;
				MaxReserveAmmo -= ammoDiff;
			} else {
				CurrentAmmo += MaxReserveAmmo;
				MaxReserveAmmo = 0;
			}
        }
	}
}
