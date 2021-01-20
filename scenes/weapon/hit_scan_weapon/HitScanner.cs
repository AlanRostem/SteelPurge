using Godot;
using System;

public class HitScanner : RayCast2D
{
	private void _OnScan(float range, float direction, uint damage)
	{
		ForceRaycastUpdate();
		CastTo = new Vector2(range * direction, 0);
		var collider = GetCollider();
		if (collider is TileMap tileMap)
		{
			
		}
		
		// if (collider is Enemy)
		// {
		//	
		// }
	}
}
