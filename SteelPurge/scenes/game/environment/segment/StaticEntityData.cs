using Godot;
using System;

public class StaticEntityData<T> : EntityData<T> where T : StaticEntity
{
    public StaticEntityData(T entity) : base(entity)
    {
        
    }
}
