using Godot;
using System;

public class KineticBubbleAbility : TacticalAbility
{
	public override void OnActivate()
	{
		GetWeapon().OwnerPlayer.CanTakeDamage = false;
		Update();
	}

	public override void OnEnd()
	{
		GetWeapon().OwnerPlayer.CanTakeDamage = true;
		Update();
	}

	public override void _Draw()
	{
		if (!IsActive) return;
		var color = new Color(102f / 255f, 204f / 255f, 255f / 255f, 0.5f);
		DrawCircle(new Vector2(), 20f, color);
	}
}
