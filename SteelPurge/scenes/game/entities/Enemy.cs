using Godot;
using System;
using System.Collections;
using Object = Godot.Object;

public class Enemy : LivingEntity
{
	protected static readonly PackedScene ScrapScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/scrap/Scrap.tscn");
	protected static readonly PackedScene TeCellScene =
		GD.Load<PackedScene>("res://scenes/game/entities/collectible/fuel/FuelCollectible.tscn");

	[Export] public uint ScrapDropHit = 2;
	[Export] public uint ScrapDropKilled = 25;
	[Export] public uint TeCellDrop = 25;

	[Export] public float PlayerDetectionRange = 1000;
	[Export] public float KnockBackSpeed = 300;
	[Export] public bool CanBeKnockedBack = true;
	[Export] public bool DropScrapWhenDamaged = true;
	[Export] public bool DropTeCells = true;
	public bool IsCurrentlyLethal = true;

	public bool IsAiEnabled
	{
		get => _isAiEnabled;
		set
		{
			_isAiEnabled = value;
			IsCurrentlyLethal = value;
			CanMove = value;
			
			if (value)
				OnEnableAi();
			else
				OnDisableAi();
		}
	}

	private bool _isAiEnabled = true;
	private bool _isDead;
	private bool _dropScrap;
	private bool _dropTeCell;
	private bool _isKnockedBack;
	private bool _isPlayerDetected = false;
	protected Player DetectedPlayer { get; private set; }
	private Timer _meleeAffectedKnockBackTimer;
	private DamageNumberGenerator _damageNumberGenerator;
	private DamageIndicator _damageIndicator;

	public override void _Ready()
	{
		base._Ready();
		_meleeAffectedKnockBackTimer = GetNode<Timer>("MeleeAffectedKnockBackTimer");
		_damageNumberGenerator = GetNode<DamageNumberGenerator>("DamageNumberGenerator");
		_damageIndicator = GetNode<DamageIndicator>("DamageIndicator");
	}

	public virtual void OnDie()
	{
	}

	public void AttackPlayer(uint damage, Player player, Vector2 knockBackDirection)
	{
		if (!IsCurrentlyLethal) return;
		player.TakeDamage(damage, knockBackDirection);
	}
	
	public override void _Process(float delta)
	{
		base._Process(delta);
		if (_isDead)
		{
			var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
			scrap.SetCount(ScrapDropKilled);
			QueueFree();
		}

		if (_dropScrap)
		{
			_dropScrap = false;
			var scrap = ParentWorld.Entities.SpawnEntityDeferred<Scrap>(ScrapScene, Position);
			scrap.SetCount(ScrapDropHit);
		}

		if (_dropTeCell)
		{
			_dropTeCell = false;
			var cell = ParentWorld.Entities.SpawnEntityDeferred<FuelCollectible>(TeCellScene, Position);
			cell.Count = TeCellDrop;
		}

		if (!_isAiEnabled) return;
		var distance = Mathf.Abs(ParentWorld.PlayerNode.Position.x - Position.x);
		if (distance < PlayerDetectionRange)
		{
			if (DetectedPlayer is null)
				DetectedPlayer = ParentWorld.PlayerNode;
			if (!_isPlayerDetected)
			{
				OnPlayerDetected(DetectedPlayer);
				_isPlayerDetected = true;
			}
			_ProcessWhenPlayerDetected(ParentWorld.PlayerNode);
		}
		else
		{
			if (_isPlayerDetected)
			{
				OnPlayerVisualLost(DetectedPlayer);
				_isPlayerDetected = false;
			}
			_ProcessWhenPlayerNotSeen();
		}
	}

	protected virtual void OnPlayerDetected(Player player)
	{
		
	}
	
	protected virtual void OnPlayerVisualLost(Player player)
	{
		
	}

	public override void TakeDamage(uint damage, Vector2 direction, VulnerableHitbox.DamageType damageType,
		bool isCritical = false)
	{
		if (damage >= Health)
		{
			if (!_isDead)
			{
				OnDie();
				// Assuming the player gave damage to the enemy
				ParentWorld.PlayerNode.PlayerInventory.IncrementKillCount();
				_isDead = true;
				_damageNumberGenerator.ShowDamageNumber(Health, Position, ParentWorld, Colors.Red);
				Health = 0;
				if (isCritical && DropTeCells)
				{
					_dropTeCell = true;
				}
			}
		}
		else
		{
			if (DropScrapWhenDamaged)
				_dropScrap = true;
			Health -= damage;
			_damageNumberGenerator.ShowDamageNumber(damage, Position, ParentWorld);
			var color = !isCritical ? new Color(255, 255, 255) : Colors.Red;
			_damageIndicator.Indicate(color);
			if (direction.x != 0 || direction.y != 0)
			{
				KnockBack(direction);
			}

			if (damageType == VulnerableHitbox.DamageType.RamSlide)
				VelocityX = 0;

			if (isCritical && DropTeCells)
			{
				_dropTeCell = true;
			}
		}
	}

	public void KnockBack(Vector2 direction, float speed)
	{
		if (!CanBeKnockedBack) return;
		ApplyForce(direction * speed);
		CanMove = false;
		_isKnockedBack = true;
		_meleeAffectedKnockBackTimer.Start();
	}

	public void KnockBack(Vector2 direction)
	{
		KnockBack(direction, KnockBackSpeed);
	}

	private void _OnVulnerableHitboxHit(uint damage, Vector2 knockBackDirection, VulnerableHitbox.DamageType damageType)
	{
		TakeDamage(damage, knockBackDirection, damageType);
	}


	private void _OnCriticalHitboxHit(uint damage, Vector2 knockBackDirection, VulnerableHitbox.DamageType damageType)
	{
		TakeDamage(damage, knockBackDirection, damageType, true);
	}


	protected virtual void _ProcessWhenPlayerDetected(Player player)
	{
	}

	protected virtual void _ProcessWhenPlayerNotSeen()
	{
	}

	private void _OnKnockBackEnd()
	{
		if (_isKnockedBack)
		{
			CanMove = true;
		}

		_isKnockedBack = false;
	}

	private void _OnDamageIndicationTimeout()
	{
		Modulate = new Color(1, 1, 1);
	}

	/// <summary>
	/// Called when AI is enabled. Recommend all Enemy derived scenes/classes
	/// override this for consistent behaviour.
	/// </summary>
	public virtual void OnEnableAi()
	{
		
	}
	
	/// <summary>
	/// Called when AI is disabled. Recommend all Enemy derived scenes/classes
	/// override this for consistent behaviour.
	/// </summary>
	public virtual void OnDisableAi()
	{
		
	}
}
