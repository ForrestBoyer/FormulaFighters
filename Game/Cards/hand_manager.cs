using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class hand_manager : card_container
{
	private Vector2 leftmostCardPosition;
	private PackedScene cardScene;
	private Godot.Collections.Array<Godot.Node> removedCards;
	private new_card tempCard;

	// Called whenever cards in hand are changed and need to be redrawn
	public void updateHand(){
		PackedScene cardScene = GD.Load<PackedScene>("res://Game/Cards/new_card.tscn");
		leftmostCardPosition = new Vector2(408f, 600f);

		// delete all current cards
		removedCards = GetChildren();
		foreach(new_card card in removedCards) {
			RemoveChild(card);
			card.QueueFree();
		}
		// for each card in cardList
		for(int i = 0; i < size; i++) {
			if (cardList[i].cardType == "Number") {
				tempCard = cardScene.Instantiate<new_card>();
				tempCard.InitCard(cardList[i].intVal, i);
				tempCard.SetPos(leftmostCardPosition);
				AddChild(tempCard);
			} else if (cardList[i].cardType == "Operator") {
				tempCard = cardScene.Instantiate<new_card>();
				tempCard.InitCard(cardList[i].opVal, i);
				tempCard.SetPos(leftmostCardPosition);
				AddChild(tempCard);
			}
			leftmostCardPosition.X += 80f;
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
