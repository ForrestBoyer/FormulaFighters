using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class hand_manager : card_container
{
	const int HANDSIZE = 7;
	private new_card[] initHand;
	private Vector2 position;
	private PackedScene cardScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cardScene = GD.Load<PackedScene>("res://Game/Cards/new_card.tscn");

		position = new Vector2(408f, 600f);

		// this will be replaced by drawfromdeck(7)
		initHand = new new_card[HANDSIZE];
		for(int i = 0; i < HANDSIZE; i++) {
			initHand[i] = cardScene.Instantiate<new_card>();
			initHand[i].InitCard("*", i, position);
			AddChild(initHand[i]);
			position.X += 80f;
		}
		// InitCardContainer(7, initHand);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
