using Godot;
using System;

public partial class Enemy : Entity
{
	[Signal]
	public delegate void DeathEventHandler();

    public Texture2D Texture;

	private Random rand;

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
        rand = GetNode<World>("/root/World").RNG;
		int index = rand.Next(texturePaths.Length);
		Texture = (Texture2D)GD.Load(texturePaths[index]);

		if (Texture != null)
		{
			var sprite = GetNode<Sprite2D>("Sprite2D");
			sprite.Texture = Texture;
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
