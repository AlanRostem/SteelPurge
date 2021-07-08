using Godot;
using System;

public class KinematicEntityData<T> : EntityData<T> where T : KinematicEntity
{
    public KinematicEntityData(T entity) : base(entity)
    {
        Velocity = entity.Velocity;
    }
}
