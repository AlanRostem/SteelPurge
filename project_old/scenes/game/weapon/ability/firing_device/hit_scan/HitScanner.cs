using Godot;
using System;

public class HitScanner : RayCast2D
{
	private HitScanFiringDevice _parent;
	public override void _Ready()
	{
		base._Ready();
		_parent = GetParent<HitScanFiringDevice>();
	}

	protected virtual void _OnScan(uint damage, float range, float angle)
	{
		CastTo = new Vector2(range * Mathf.Cos(angle), range * Mathf.Sin(angle));
		ForceRaycastUpdate();
		if (!IsColliding()) return;
		var collider = GetCollider();
		switch (collider)
		{
			case TileMap tileMap:
				break;
			case VulnerableHitbox hitBox:
				if (hitBox is CriticalHitbox criticalHitbox)
				{
					var directionalAngle = Mathf.Rad2Deg(angle);
					var targetAngle = Mathf.Rad2Deg(criticalHitbox.CriticalHitDirection.Angle());
					var angleDiff = (directionalAngle - targetAngle + 180 + 360) % 360 - 180;
					if (angleDiff > criticalHitbox.CriticalHitAngularMargin ||
						angleDiff < -criticalHitbox.CriticalHitAngularMargin) return;
				}
				
				hitBox.TakeHit(damage, Vector2.Zero, VulnerableHitbox.DamageType.HitScan);
				_parent.GetWeapon().EmitSignal(nameof(Weapon.DamageDealt), damage, hitBox);
				break;
		}
	}
}
