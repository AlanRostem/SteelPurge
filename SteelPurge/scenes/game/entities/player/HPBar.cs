using Godot;
using System;

public class HPBar : TextureProgress
{
	private Timer _flashTimer;
	private readonly Texture GainTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over_gain.png");
	private readonly Texture LossTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over_loss.png");
	private readonly Texture DefaultTexture = GD.Load<Texture>("res://assets/texture/ui/hud/hp_bar_over.png");

	private float _gainTime = 0.1f;
	private float _lossTime = 0.2f;
	
	public override void _Ready()
	{
		_flashTimer = GetNode<Timer>("FlashTimer");
	}

	private void _OnPlayerHealthChanged(uint health)
	{
		if (health < Value)
		{
			TextureOver = LossTexture;
			_flashTimer.WaitTime = _lossTime;
		}
		else
		{
			TextureOver = GainTexture;
			_flashTimer.WaitTime = _gainTime;
		}
		
		Value = health;
		_flashTimer.Start();
	}

	private void _OnFlashTimeout()
	{
		TextureOver = DefaultTexture;
	}
}
