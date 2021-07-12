using Godot;
using System;

public class TacticalAbility : WeaponAbility
{
	[Export] public float CoolDown = 6;
	[Export] public float Duration = 1;
	[Export] public uint AmmoDrain = 5;

	public bool IsOnCoolDown = false;
	private bool _removeWeaponOnEnd = false;

	private Timer _cooldownTimer;
	private Timer _durationTimer;
	private TextureProgress _abilityBar;

	public override void _Ready()
	{
		base._Ready();
		_cooldownTimer = GetNode<Timer>("CoolDownTimer");
		_durationTimer = GetNode<Timer>("DurationTimer");
		_abilityBar = GetNode<TextureProgress>("AbilityBar");
		GetWeapon().TacticalEnhancement = this;
		_abilityBar.Visible = false;
	}

	public override void ReCharge()
	{
		if (IsActive) return;
		IsOnCoolDown = false;
		_abilityBar.Visible = false;
		_cooldownTimer.Stop();
	}

	public virtual void OnActivate()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnEnd()
	{
	}

	public override void DeActivate()
	{
		IsActive = false;
		OnEnd();

		if (_removeWeaponOnEnd)
		{
			GetWeapon().OwnerPlayer.PlayerInventory.SwitchWeapon(Inventory.InventoryWeapon.P336);
			return;
		}


		_abilityBar.Visible = true;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		_durationTimer.Stop();
		_abilityBar.MaxValue = CoolDown * 1000;
		_abilityBar.Value = 0;
	}

	public override void _Process(float delta)
	{
		base._Process(delta);
		if (Input.IsActionJustPressed("tactical_ability") && GetWeapon().Equipped)
		{
			if (!IsOnCoolDown && !IsActive && GetWeapon().Ammo >= AmmoDrain)
			{
				IsActive = true;
				OnActivate();
				GetWeapon().RemoveAmmoButDontDisappear(AmmoDrain);
				if (GetWeapon().Ammo == 0)
				{
					_removeWeaponOnEnd = true;
				}
				_durationTimer.Start();
			}
			else
			{
				// TODO: Play sound and flash red on icon
			}
		}

		if (IsActive)
		{
			OnUpdate();
		}
		else if (IsOnCoolDown)
		{
			_abilityBar.Value += delta * 1000;
		}
	}

	private void _OnCoolDownFinished()
	{
		IsOnCoolDown = false;
		_abilityBar.Visible = false;
	}

	private void _OnStartCoolDown()
	{
		IsActive = false;
		if (_removeWeaponOnEnd)
		{
			GetWeapon().OwnerPlayer.PlayerInventory.SwitchWeapon(Inventory.InventoryWeapon.P336);
			return;
		}
		
		_abilityBar.Visible = true;
		IsOnCoolDown = true;
		_cooldownTimer.Start();
		_abilityBar.MaxValue = CoolDown * 1000;
		_abilityBar.Value = 0;
		OnEnd();
	}

	public override void OnWeaponSwapped()
	{
		_abilityBar.Visible = false;
	}
}
