using Godot;
using System;

public class HitScanner : RayCast2D
{
	private void _OnScan(float range, uint damage)
	{
		ForceRaycastUpdate();
		CastTo = new Vector2(range, 0);
		if (!IsColliding()) return;
		var collider = GetCollider();
		switch (collider)
		{
			case TileMap tileMap:
				break;
			case VulnerableHitbox hitBox:
				hitBox.EmitSignal(nameof(VulnerableHitbox.Hit), damage);
				break;
		}
	}
}
