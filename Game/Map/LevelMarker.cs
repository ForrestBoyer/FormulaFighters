using Godot;
using System;
using System.Xml.Schema;

public partial class LevelMarker : TextureButton
{
    // Corresponding level number
    public int Level_number;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        Map _Map = (Map)GetNode("/root/World/Map");
        if(Level_number == _Map.Current_Level + 1){
            Disabled = false;
        } else {
            Disabled = true;
        }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public void _on_level_marker_pressed()
    {
        PackedScene levelScene = GD.Load<PackedScene>("res://Game/Levels/level.tscn");
        Level level = levelScene.Instantiate<Level>();

        GetNode("/root/World/Map").AddChild(level);
    }
}
