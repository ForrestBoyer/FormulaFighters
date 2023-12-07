using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class hand_manager : card_container
{
	private Vector2 leftmostCardPosition;
	private PackedScene cardScene;



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		leftmostCardPosition = new Vector2(408f, 600f);
		cardScene = GD.Load<PackedScene>("res://Game/Cards/new_card.tscn");

		for(int i = 0; i < cardList.Length; i++) {
			//cardList[i] = cardScene.Instantiate<new_card>();
			cardList[i].SetPos(leftmostCardPosition);
			AddChild(cardList[i]);
			leftmostCardPosition.X += 80f;
		}

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
