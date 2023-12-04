using Godot;
using System;
using System.Xml.Schema;

public partial class LevelMarker : TextureButton
{
    // Corresponding level number
    public int Level_number;
    // Corresponding Level
    Level _Level;

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
}
