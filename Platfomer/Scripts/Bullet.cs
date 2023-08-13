//By: James Singleton
//Date: 8/12/2023
using Godot;
using System;

public partial class Bullet : Area2D
{
	/// <summary>
	/// The speed the bullet flys
	/// </summary>
	[Export]
	public int Speed {get; set;} = 400;
	/// <summary>
	/// If true the bullet will fly to the right
	/// </summary>
	[Export]
	public bool GoesRight {get; set;}= true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//In the Engine, +X is Right and -X is Left
		float direction = GoesRight ? 1.0f : -1.0f;
		//Creates a vector of length 1 pointing in the firection the bullet should fly
		Vector2 velocity = new Vector2(direction, 0).Normalized();
		//makes the bullet move in the direction it should
		Position += velocity * Speed * (float) delta;

	}

	/// <summary>
	/// When the bullet encounters an instance of an Area2D, Respond appropriately
	/// </summary>
	/// <param name="area"></param>
	private void OnAreaEntered(Area2D area)
	{	
		//If the bullet encounters anything that isn't the Player or the Goal, remove itself from the scene
		if(!area.IsInGroup("Player") && !area.IsInGroup("Goal"))
		{
			QueueFree();
		}
	}
}
