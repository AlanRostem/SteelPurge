using Godot;
using System;

public class KineticShieldAbility : TacticalAbility
{
	public override void _Ready()
	{
		base._Ready();
		GetWeapon().OwnerPlayer.Connect(nameof(LivingEntity.OnTakeDamage), this, nameof(_OnPlayerTakeDamage));
	}

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
		var color = new Color(Colors.Cyan) {a = 0.5f};
		DrawCircle(Vector2.Zero, 16, color);
	}

	private void _OnPlayerTakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType, bool isCritical)
	{
		if (!IsActive) return;
		DeActivate();
		GetWeapon().OwnerPlayer.BecomeInvincible();
	}
}
