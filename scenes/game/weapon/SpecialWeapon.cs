using Godot;
using System;

public class SpecialWeapon : Weapon
{
	protected override void ReloadPerformed()
	{
		if (ClipSize > CurrentAmmo) {
			if (OwnerPlayer.PlayerInventory.XeSlugCount > (ClipSize - CurrentAmmo)) {
				var ammoDiff = ClipSize - CurrentAmmo;
				CurrentAmmo += ammoDiff;
				OwnerPlayer.PlayerInventory.XeSlugCount -= ammoDiff;
			} else {
				CurrentAmmo += OwnerPlayer.PlayerInventory.XeSlugCount;
				OwnerPlayer.PlayerInventory.XeSlugCount = 0;
			}
		}
	}
}
