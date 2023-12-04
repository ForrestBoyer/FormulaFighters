using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	public TextureRect Background { get; set; }
	public Random rand = new Random();

	public List<Enemy> LevelEnemies = new List<Enemy>();
    // TODO: public Card[] Rewards;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Background = GetNode<TextureRect>("Background");

		string[] texturePaths = 
		{
			"res://Game/Levels/Backgrounds/brownBricksRoom.png",
			"res://Game/Levels/Backgrounds/darkBlueRoom.png",
			"res://Game/Levels/Backgrounds/throneRoom.png",
		};

		int index = rand.Next(texturePaths.Length);
		var texture = (Texture2D)GD.Load(texturePaths[index]);

		GD.Print(texture.ResourcePath);

		if (texture != null)
		{
			Background.Texture = texture;
		}
		else
		{
			GD.Print("Faied to load texture");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}