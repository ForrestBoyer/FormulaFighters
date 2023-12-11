using Godot;
using System;
using System.Xml.Schema;

public partial class LevelMarker : TextureButton
{
    // Corresponding level number
    public int Depth { get; set; }

    public bool IsBoss { get; set; } = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
            PackedScene levelScene = GD.Load<PackedScene>("res://Game/Levels/level.tscn");
            Level level = levelScene.Instantiate<Level>();

            GetNode("/root/World/Map").AddChild(level);
        }
    }
}
