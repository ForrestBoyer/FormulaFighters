using Godot;
using Godot.NativeInterop;
using System;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class card_container : Node2D
{
	protected Godot.Collections.Array<new_card> cardList;
	protected int size;

	// Initializer for card container with empty card list
	public void InitCardContainer(){
		cardList = new Godot.Collections.Array<new_card>();
		size = cardList.Count();
	}
	// Initializer for card container with list of cards
	public void InitCardContainer(Godot.Collections.Array<new_card> Cards){
		cardList = new Godot.Collections.Array<new_card>();
		cardList.AddRange(Cards);
		size = cardList.Count();
	}
	// Add Card to End
	public void AddCard(new_card Card){
		cardList.Append(Card);
		size = cardList.Count();
	}
	// Replace Cards
	public void ReplaceCards(Godot.Collections.Array<new_card> Cards){
		cardList.Clear();
		cardList = Cards;
		size = cardList.Count();
	}
	// Remove Card from end
	public new_card RemoveCard(){		
		new_card ret;
		ret = cardList[cardList.Count() - 1];
		cardList.RemoveAt(cardList.Count() - 1);
		size = cardList.Count();
		return ret;
	}
	// Empty Container
	public void EmptyContainer() {
		cardList.Clear();
		size = cardList.Count();
	}
	// Shuffle
	public void ShuffleCards(){
		cardList.Shuffle();
	}

	// Draw
	public Godot.Collections.Array<new_card> DrawCards(int Num){
		Godot.Collections.Array<new_card> ret;
		if (cardList.Count() < Num) {
			return null;
		}
		ret = cardList.Slice(0, Num);
		// for(int i = 0; i < Num; i++) {
		// 	cardList.RemoveAt(0);
		//  size = cardList.Count();
		// }
		
		return ret;
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