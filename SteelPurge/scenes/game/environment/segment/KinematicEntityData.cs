using Godot;
using System;
using Godot.Collections;

public class KinematicEntityData<T> : EntityData<T> where T : KinematicEntity
{
    public KinematicEntityData(T entity) : base(entity)
    {
        Velocity = entity.Velocity;
    }

    public KinematicEntityData(Dictionary<string, object> data) : base(data)
    {
        
    }
}
