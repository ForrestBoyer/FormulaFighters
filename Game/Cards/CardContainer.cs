using Godot;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

public partial class CardContainer : Node2D
{
	public List<Card> Cards = new List<Card>();

	public int size;

	// Initializer for card container with list of cards
	public void SetCards(List<Card> cards)
	{
		Cards = cards;
        size = Cards.Count;
	}

	// Add Card to End
	public void AddCard(Card card)
	{
		Cards.Add(card);
		size = Cards.Count;
	}

	public void AddCards(List<Card> cards)
	{
		foreach (Card card in cards)
		{
			Cards.Add(card);
		}

		size = Cards.Count;
	}

	// Replace Cards
	public void ReplaceCards(List<Card> cards){
		Cards.Clear();
		Cards = cards;
        size = Cards.Count;
	}

	// Empty Container
	public void Empty() 
	{
		Cards.Clear();
		size = Cards.Count;
	}

	// Shuffle
	public void ShuffleCards()
	{
		Cards.Shuffle();
	}

	// Draw
	public List<Card> DrawCards(int Num)
	{
		List<Card> ret;

		if (Cards.Count < Num) 
		{
			return null;
		}

		ret = Cards.Take(Num).ToList();

		foreach (Card card in ret)
		{
			Cards.Remove(card);
		}

		size = Cards.Count;
		
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
