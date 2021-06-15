using Godot;
using System;

public class DamageNumberGenerator : Node2D
{
	private static readonly PackedScene DamageNumberScene =
		GD.Load<PackedScene>("res://scenes/game/components/damage_number/DamageNumber.tscn");

	private static readonly Vector2 StartOffset = new Vector2(-16, -32);

	private DamageNumber _damageNumber;


	public void ShowDamageNumber(uint damage, Vector2 position, World parentWorld)
	{
		ShowDamageNumber(damage, position, parentWorld, Colors.White);
	}

	public void ShowDamageNumber(uint damage, Vector2 position, World parentWorld, Color color)
	{
		if (_damageNumber is null)
		{
			var number = (DamageNumber) DamageNumberScene.Instance();
			number.Damage = damage;
			number.RectPosition = position + Position - number.RectSize / 2;
			number.Modulate = color;
			parentWorld.AddChild(number);
			_damageNumber = number;  
			number.Connect(nameof(DamageNumber.Disappear), this, nameof(_OnDamageNumberDisappear));
		}
		else
		{
			if (_damageNumber.Modulate != color)
				_damageNumber.Modulate = color;
			_damageNumber.RectPosition = position + Position - _damageNumber.RectSize / 2;
			_damageNumber.Damage += damage;
		}
	}

	private void _OnDamageNumberDisappear()
	{
		_damageNumber = null;
	}
}
