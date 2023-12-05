using Godot;
using System;
using System.Collections.Generic;

public partial class Level : Node2D
{
	public TextureRect Background { get; set; }
	public Random rand = new Random();
	public Enemy Enemy { get; set; }
	public Player Player { get; set; }
	public int LevelNumber { get; set; }

    // TODO: public Card[] Rewards;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Background = GetNode<TextureRect>("Background");

		PackedScene enemyScene = GD.Load<PackedScene>("res://Game/Entities/Enemy/enemy.tscn");
		PackedScene playerScene = GD.Load<PackedScene>("res://Game//Entities/Player/player.tscn");

		Player = playerScene.Instantiate<Player>();
		Player.Position = new Vector2(360f, 480f);
		AddChild(Player);

		Enemy = enemyScene.Instantiate<Enemy>();
		Enemy.Position = new Vector2(920f, 450f);
		AddChild(Enemy);


		string[] texturePaths = 
		{
			"res://Game/Levels/Backgrounds/brownBricksRoom.png",
			"res://Game/Levels/Backgrounds/darkBlueRoom.png",
			"res://Game/Levels/Backgrounds/throneRoom.png",
		};

		int index = rand.Next(texturePaths.Length);
		var texture = (Texture2D)GD.Load(texturePaths[index]);

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
