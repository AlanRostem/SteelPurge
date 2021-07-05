using Godot;
using Godot.Collections;

public class LivingEntity : KinematicEntity
{
	[Signal]
	public delegate void OnTakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType, bool isCritical = false);

	public delegate void StatusEffectInitializer(StatusEffect effect);

	public enum StatusEffectType
	{
		Burn,
		Stun,
		KnockBack,
		None,
	}

	private static readonly PackedScene[] StatusEffectScenes =
	{
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/firewall/BurnEffect.tscn"),
		GD.Load<PackedScene>("res://scenes/game/status_effects/StunEffect.tscn"),
		GD.Load<PackedScene>("res://scenes/game/status_effects/KnockBackEffect.tscn"),
	};

	private readonly Dictionary<StatusEffectType, StatusEffect> _effects =
		new Dictionary<StatusEffectType, StatusEffect>();

	[Export] public Array<StatusEffectType> EffectsImmuneTo = new Array<StatusEffectType>();
	[Export] public bool CanReceiveStatusEffect = true;
	[Export] public uint MaxHealth = 100;

	public uint Health
	{
		get => _health;

		set
		{
			_health = value;
			if (_health > MaxHealth)
				_health = MaxHealth;
			EmitSignal(nameof(HealthChanged), _health);
		}
	}

	private uint _health = 100;

	public override void _Ready()
	{
		base._Ready();
		Health = MaxHealth;
	}

	public void ApplyStatusEffect(StatusEffectType type)
	{
		ApplyStatusEffect(type, effect => { });
	}

	protected virtual void OnStatusEffectApplied(StatusEffectType type, StatusEffect effect)
	{
	}

	public virtual void Die()
	{
		QueueFree();
	}

	public void ApplyStatusEffect(StatusEffectType type, StatusEffectInitializer callback)
	{
		if (type == StatusEffectType.None || !CanReceiveStatusEffect)
			return;
		if (EffectsImmuneTo.Contains(type)) return;

		if (_effects.ContainsKey(type))
		{
			var effect = _effects[type];
			effect.ResetTime();
			callback(effect);
			effect.EmitSignal(nameof(StatusEffect.Start), this);
			OnStatusEffectApplied(type, effect);
			return;
		}


		var newEffect = (StatusEffect) StatusEffectScenes[(int) type].Instance();
		callback(newEffect);
		newEffect.EmitSignal(nameof(StatusEffect.Start), this);
		_effects[type] = newEffect;
		AddChild(newEffect);
		OnStatusEffectApplied(type, newEffect);
	}

	public void ClearStatusEffects()
	{
		foreach (var effect in _effects)
		{
			effect.Value.EmitSignal(nameof(StatusEffect.End));
		}
	}

	public void TakeDamage(uint damage, Vector2 direction, bool isCritical = false)
	{
		TakeDamage(damage, direction, VulnerableHitbox.DamageType.Standard, isCritical);
	}

	public virtual void TakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType,
		bool isCritical = false)
	{
		if (damage >= Health)
			Die();
		Health -= damage;
		EmitSignal(nameof(OnTakeDamage), damage, direction, damageType, isCritical);
	}

	public void RemoveStatusEffect(StatusEffectType type)
	{
		_effects.Remove(type);
	}
}
