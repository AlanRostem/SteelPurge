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
	
	public override void _Ready()
	{
		_hpBar = GetNode<TextureProgress>("CanvasLayer/BossHPBar");
		_hpBar.MaxValue = BaseHitPoints;
		_hpBar.Value = BaseHitPoints;
		CanBeKnockedBack = false;
		base._Ready();
	}
	
	private void _OnBossHealthChanged(uint health)
	{
		_hpBar.Value = health;
	}
}
