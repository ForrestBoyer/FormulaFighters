using Godot;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public partial class card_container : Node2D
{
	public int maxSize;
	// Current size of card list
	private int size;
	// List of cards
	private new_card[] cardList;
	// Max size of card list / container
	private Random rand;

	// Initializer for card container with empty card list
	public void InitCardContainer(int MaxSize){
		maxSize = MaxSize;
		cardList = new new_card[maxSize];
		size = 0;
		rand = new Random();
	}
	// Initializer for card container with list of cards
	public void InitCardContainer(int MaxSize, new_card[] Cards){
		maxSize = MaxSize;
		if (Cards.Length <= maxSize) {
			cardList = Cards;
			size = Cards.Length;
		}
		rand = new Random();
	}
	// Add Card to End
	public void AddCard(new_card Card){
		if (size < maxSize) {
			cardList[size] = Card;
			size++;
		}
	}
	// Add Card at Index
	public void AddCard(new_card Card, int Index){
		if (Index < maxSize) {
			cardList[Index] = Card;
			size = cardList.Length;
		}
	}
	// Add Multiple Cards
	public void AddCard(new_card[] Cards){
		if (Cards.Length <= maxSize) {
			cardList = Cards;
			size = Cards.Length;
		}
	}
	// Remove Card from end
	public new_card RemoveCard(){		
		size--;
		return cardList[cardList.Length];
	}
	// Remove Card with ID
	public new_card RemoveCard(int CardID){
		for(int i = 0; i < size; i++) {
			if(cardList[i].GetID() == CardID) {
				// swap card to end of list
				new_card temp;
				temp = cardList[size];
				cardList[size] = cardList[i];
				cardList[i] = temp;
				size--;
				return cardList[cardList.Length];
			}
		}
		return null;
	}
	// Remove List of Cards
	public new_card[] RemoveCard(new_card[] Cards){
		if (Cards.Length == size) {
			size -= Cards.Length;
			return cardList;
		}
		return null;
	}
	// Shuffle
	public void Shuffle(){
		int i = cardList.Length;
		while (i > 1) {
			// generate random index
			int index = rand.Next(i--);
			// swap current pos (i) with random index
			new_card temp = cardList[i];
			cardList[i] = cardList[index];
			cardList[index] = temp;
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
