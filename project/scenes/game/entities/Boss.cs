using Godot;
using System;

public class Boss : Enemy
{
	public enum BossPhase
	{
		One,
		Two,
		Three,
		Four,
		
		Count
	}
	
	private TextureProgress _hpBar;
	public BossPhase CurrentPhase = BossPhase.One;
	
	public override void _Init()
	{
		_hpBar = GetNode<TextureProgress>("CanvasLayer/BossHPBar");
		_hpBar.MaxValue = MaxHealth;
		_hpBar.Value = MaxHealth;
		CanBeKnockedBack = false;
		base._Init();
	}

	private void _OnBossHealthChanged(uint health)
	{
		_hpBar.Value = health;
	}
}
