using Godot;
using System;
using System.Xml.Schema;

public partial class LevelMarker : TextureButton
{
    // Corresponding level number
    public int Depth { get; set; }

    public bool IsBoss { get; set; } = false;

    public Level Level;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        PackedScene levelScene = GD.Load<PackedScene>("res://Game/Levels/level.tscn");
        Level = levelScene.Instantiate<Level>();
        Level.Depth = Depth;
        GetNode("/root/World/Map").AddChild(Level);
        Level.LevelMarker = this;
        Connect("pressed", new Callable(Level, nameof(Level._on_level_selected)));
        GetNode("/root/World/Map").RemoveChild(Level);
        Sprite2D Preview = GetNode<Sprite2D>("Sprite2D");
        Preview.Texture = Level.Enemy.Texture;
        Preview.Position = new Vector2(15, 0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_level_marker_pressed()
    {
        Map map = GetNode<Map>("/root/World/Map");
        if (Depth == map.Current_Depth)
        {
            map.AddChild(Level);
        }
    }
}
