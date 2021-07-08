using Godot;
using Godot.Collections;

public class EntityData
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public Vector2 WorldPosition
    {
        set => _data["position"] = new Dictionary()
        {
            ["x"] = value.x,
            ["y"] = value.y,
        };
    }

    public string ScenePath
    {
        set => _data["scenePath"] = value;
    }

    public EntityData(KinematicEntity entity)
    {
        WorldPosition = entity.Position;
        ScenePath = entity.Filename;
    }
    
    public EntityData(StaticEntity entity)
    {
        WorldPosition = entity.Position;
        ScenePath = entity.Filename;        
    }

    public Dictionary<string, object> GetJson()
    {
        return _data;
    }
}