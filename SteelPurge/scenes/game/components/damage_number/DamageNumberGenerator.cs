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
			number.Position = position + Position;
			number.Modulate = color;
			parentWorld.AddChild(number);
			_damageNumber = number;  
			number.Connect(nameof(FloatingTempText.Disappear), this, nameof(_OnDamageNumberDisappear));
		}
		else
		{
			if (_damageNumber.Modulate != color)
				_damageNumber.Modulate = color;
			_damageNumber.Position = position + Position;
			_damageNumber.Damage += damage;
		}
	}

	private void _OnDamageNumberDisappear()
	{
		_damageNumber = null;
	}
}
	
