using Godot;
using System;
using Godot.Collections;

public class StaticEntityData<T> : EntityData<T> where T : StaticEntity
{
    public StaticEntityData(T entity) : base(entity)
    {
        
    }

    public StaticEntityData(Dictionary<string, object> data) : base(data)
    {
        
    }
}
