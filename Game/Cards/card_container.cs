using Godot;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;

public partial class card_container : Node2D
{
	public List<new_card> Cards = new List<new_card>();

	protected int size;

	// Initializer for card container with empty card list
	public void InitCardContainer()
	{
		size = Cards.Count;
	}

	// Initializer for card container with list of cards
	public void InitCardContainer(List<new_card> Cards)
	{
		this.Cards.AddRange(Cards);
        size = this.Cards.Count;
	}

	// Add Card to End
	public void AddCard(new_card Card)
	{
		Cards.Add(Card);
		size = Cards.Count;
	}

	// Replace Cards
	public void ReplaceCards(List<new_card> Cards){
		this.Cards.Clear();
		this.Cards = Cards;
        size = this.Cards.Count;
	}

	// Remove Card from end
	public new_card RemoveCard()
	{		
		new_card ret;
		ret = Cards[Cards.Count - 1];
		Cards.RemoveAt(Cards.Count - 1);
		size = Cards.Count;
		return ret;
	}

	// Empty Container
	public void EmptyContainer() 
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
	public List<new_card> DrawCards(int Num)
	{
		List<new_card> ret;

		if (Cards.Count < Num) 
		{
			return null;
		}

		ret = Cards.Take(Num).ToList();
		
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
