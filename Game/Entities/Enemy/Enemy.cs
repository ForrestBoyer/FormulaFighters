using Godot;
using System;
using System.Collections.Generic;

class EnemyStats
{
    public string Name;
    public int Attack;
    public int Defense;
    public int Health;
    public string TexturePath;

    public EnemyStats(string initName, int initAttack, int initDefense, int initHealth, string initTexture){
        Name = initName;
        Attack = initAttack;
        Defense = initDefense;
        Health = initHealth;
        TexturePath = initTexture;
    }
}

public partial class Enemy : Entity
{
	[Signal]
	public delegate void DeathEventHandler();

    public Texture2D Texture;
    
    public int Attack;
    public int Defense;

	private Random rand;
    private List<EnemyStats> GenerationStats = new List<EnemyStats>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();

        GenerationStats.Add(new EnemyStats("Skeleton", 10, 5, 3, "res://Game/Entities/Enemy/Sprites/skeleton.png"));
        GenerationStats.Add(new EnemyStats("Hobgoblin", 5, 10, 5, "res://Game/Entities/Enemy/Sprites/hobgoblin.png"));
        GenerationStats.Add(new EnemyStats("Slime", 3, 7, 10, "res://Game/Entities/Enemy/Sprites/slime.png"));

		// Loads random enemy
        rand = GetNode<World>("/root/World").RNG;
		int index = rand.Next(GenerationStats.Count);
		Texture = (Texture2D)GD.Load(GenerationStats[index].TexturePath);

        // Get level depth
        int depth = GetParent<Level>().Depth;
        
        // Set stats
        if(depth == 9){
            MaxHealth = GenerationStats[index].Health * 5 + 30;
            CurrentHealth = GenerationStats[index].Health * 5 + 30;
            Attack = GenerationStats[index].Attack * 2 + 5;
            Defense = GenerationStats[index].Defense * 2 + 5;
            UpdateHealthBar();
        } else {
            MaxHealth = GenerationStats[index].Health * 5 + depth * 2;
            CurrentHealth = GenerationStats[index].Health * 5 + depth * 2;
            Attack = GenerationStats[index].Attack + depth;
            Defense = GenerationStats[index].Defense + depth;
            UpdateHealthBar();
        }

		if (Texture != null)
		{
			var sprite = GetNode<Sprite2D>("Sprite2D");
			sprite.Texture = Texture;
            if(depth == 9){
                sprite.Scale = new Vector2(5, 5);
                _healthBar.Position = _healthBar.Position + new Vector2(0, -40);
            }
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
