using Godot;
using System;

public partial class Entity : Node2D
{
	protected ProgressBar _healthBar;
	protected Label _healthLabel;

	protected bool _alive { get; set; } = true;

	[Export]
	public int MaxHealth { get; set; } = 50;

	[Export]
	public int CurrentHealth { get; set; } = 50;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthLabel = GetNode<Label>("HealthBar/HealthLabel");
		UpdateHealthBar();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void _on_hitbox_mouse_entered()
	{
		TakeDamage(1);
	}

	public void TakeDamage(int damage)
	{
        if(damage < 0){
            damage = 0;
        }
        
		CurrentHealth -= damage;
		if (CurrentHealth <= 0 && _alive)
		{
			Die();
		}
		
		UpdateHealthBar();
	}

	public void UpdateHealthBar()
	{
		_healthLabel.Text = CurrentHealth.ToString();
		_healthBar.Value = GetHealthAsAPercentage();
	}

	public float GetHealthAsAPercentage()
	{
		return (float)CurrentHealth / (float)MaxHealth * 100;
	}

	public virtual void Die()
	{
		_alive = false;
		Rotate(-Mathf.Pi / 2);
	}
}
