using Godot;
using System;

public class HitScanner : RayCast2D
{
	private void _OnScan(float range, uint damage)
	{
		ForceRaycastUpdate();
		CastTo = new Vector2(range, 0);
		var collider = GetCollider();
		if (collider is TileMap tileMap)
		{
			
		}
        else if (collider is Enemy enemy)
		{
            enemy.TakeDamage(damage);
		}
    }
}
