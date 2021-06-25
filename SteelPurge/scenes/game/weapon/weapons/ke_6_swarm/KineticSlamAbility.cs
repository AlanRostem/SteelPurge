using Godot;
using System;

public class KineticSlamAbility : TacticalAbility
{
	private static readonly PackedScene BlastScene =
		GD.Load<PackedScene>("res://scenes/game/weapon/weapons/ke_6_swarm/SeismicBlast.tscn"); 
	
	[Export] public uint DirectHitBaseDamage = 110;
	
	public float SlamSpeed = 300;

	private bool _isJumping = false;
	private bool _isSlamming = false;
	private Timer _jumpTimer;
	private CollisionShape2D _slamAreaShape;

	public override void _Ready()
	{
		base._Ready();
		_jumpTimer = GetNode<Timer>("JumpTimer");
		_slamAreaShape = GetNode<CollisionShape2D>("SlamHitDetectionArea/CollisionShape2D");
	}

	public override void OnActivate()
	{
		var player = GetWeapon().OwnerPlayer;
		player.CanMove = false;

		if (!player.IsOnFloor())
		{
			Slam();
		}
		else
		{
			_isJumping = true;
			player.VelocityY = -Player.JumpSpeed;
			_jumpTimer.Start();
		}
	}

	private void Slam()
	{
		var player = GetWeapon().OwnerPlayer;
		player.VelocityY = SlamSpeed;
		_isSlamming = true;
		_isJumping = false;
		_slamAreaShape.SetDeferred("disabled", false);
	}

	public override void OnUpdate()
	{
		var player = GetWeapon().OwnerPlayer;

		if (player.IsOnCeiling() && !player.IsOnFloor())
			Slam();
		
		if (!_isJumping && _isSlamming && player.IsOnFloor())
		{
			DeActivate();
		}
	}

	public override void OnEnd()
	{
		var player = GetWeapon().OwnerPlayer;
		player.CanMove = true;
		_slamAreaShape.SetDeferred("disabled", true);
		player.ParentWorld.Entities.SpawnStaticEntityDeferred<SeismicBlast>(BlastScene, player.Position);
	}

	private void _OnJumpEnd()
	{
		Slam();
	}
	
	// TODO: Remember to set the collision mask for normal hitboxes after this dev-phase
	private void _OnSlamHitDetectionAreaHitBoxEntered(object area)
	{
		if (area is CriticalHitbox criticalHitbox)
		{
			criticalHitbox.TakeHit(DirectHitBaseDamage);
			if (criticalHitbox.GetParent() is Enemy enemy)
			{
				enemy.ApplyStatusEffect(LivingEntity.StatusEffectType.Stun);
			}
			// return;
		}

		// var hitBox = (VulnerableHitbox) area;
	}
}
