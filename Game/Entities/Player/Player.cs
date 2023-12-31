using Godot;
using System;

public partial class Player : Entity
{
	[Signal]
	public delegate void DeathEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
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
