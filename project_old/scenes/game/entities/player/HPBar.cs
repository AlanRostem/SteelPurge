using Godot;
using System;

public class HPBar : TextureProgress
{
	private Timer _flashTimer;
	private static readonly Texture GainTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over_gain.png");
	private static readonly Texture LossTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over_loss.png");
	private static readonly Texture DefaultTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over.png");

	private const float GainTime = 0.1f;
	private const float LossTime = 0.2f;
	
	public override void _Ready()
	{
		_flashTimer = GetNode<Timer>("FlashTimer");
		MaxValue = GetParent().GetParent<Player>().MaxHealth;
	}

	private void _OnPlayerHealthChanged(uint health)
	{
		if (health < Value)
		{
			TextureOver = LossTexture;
			_flashTimer.WaitTime = LossTime;
		}
		else
		{
			TextureOver = GainTexture;
			_flashTimer.WaitTime = GainTime;
		}
		
		Value = health;
		_flashTimer.Start();
	}

	private void _OnFlashTimeout()
	{
		TextureOver = DefaultTexture;
	}
}
