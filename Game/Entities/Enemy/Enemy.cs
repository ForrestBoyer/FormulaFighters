using Godot;
using System;

public partial class Enemy : Entity
{
	[Signal]
	public delegate void DeathEventHandler();

	private Random rand = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

		string[] texturePaths = 
		{
			"res://Game/Entities/Enemy/Sprites/hobgoblin.png",
			"res://Game/Entities/Enemy/Sprites/skeleton.png",
			"res://Game/Entities/Enemy/Sprites/slime.png"
		};

		// Loads random texture
		int index = rand.Next(texturePaths.Length);
		var texture = (Texture2D)GD.Load(texturePaths[index]);

		if (texture != null)
		{
			var sprite = GetNode<Sprite2D>("Sprite2D");
			sprite.Texture = texture;
		}
		else
		{
			GD.Print("Faied to load Enemy texture");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public override void Die()
	{
		base.Die();
		EmitSignal(SignalName.Death);
	}
}
