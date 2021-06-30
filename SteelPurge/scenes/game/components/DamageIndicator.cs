using Godot;
using System;

public class DamageIndicator : Node2D
{
	private Timer _damageIndicatorTimer;

	public override void _Ready()
	{
		_damageIndicatorTimer = GetNode<Timer>("DamageIndicatorTimer");
	}

	public void Indicate(Color initialColor)
	{
		var parent = (Node2D) GetParent();
		parent.Modulate = initialColor;
		_damageIndicatorTimer.Start();
	}

	private void _OnRevertColor()
	{
		var parent = (Node2D) GetParent();
		parent.Modulate = new Color(1, 1, 1, 1);
	}
}
