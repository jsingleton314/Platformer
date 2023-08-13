//By: James Singleton
//Date: 8/12/2023
using Godot;
using System;

public partial class Mob : Area2D
{
	/// <summary>
	/// The speed at which the mob moves across the screen
	/// </summary>
	[Export(PropertyHint.Range, "100,500,")]
	public int Speed {get; set;} = 300;
	/// <summary>
	/// If true the Mob will begin its movement going right, otherwise the mob will begin its movement going left
	/// </summary>
	[Export]
	public bool GoesRight {get; set;}= true;
	
	/// <summary>
	/// How much the score will be increased if the player kills this mob
	/// </summary>
	[Export(PropertyHint.Range, "10,50,5")]
	public int ScoreValue {get; set;} = 10;
	
	
	/// <summary>
	/// The path the mob moves along
	/// </summary>
	private Path2D _path;
	/// <summary>
	/// How far along the path the mob has moved
	/// </summary>
	private PathFollow2D _pathFollow;
	
	/// <summary>
	/// When the mob dies tell main which mob died
	/// </summary>
	/// <param name="mob">The mob that died</param>
	[Signal]
	public delegate void MobSlainEventHandler(Mob mob);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//makes _path refer to MobPath
		_path = GetNode<Path2D>("MobPath");
		//makes _pathFollow refre to MobPathFollow
		_pathFollow = GetNode<PathFollow2D>("MobPath/MobPathFollow");
		_pathFollow.ProgressRatio = .5f;//Makes the _pathFollow be where the Mob's origin is on the path
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//In the Engine, +X is Right and -X is Left
		int direction = GoesRight ? 1 : -1;
		//gets the spot that _pathFollow is currently at
		Vector2 lastPosition = new Vector2(_pathFollow.Position.X, _pathFollow.Position.Y);
		//moves _pathFollow along _path
		_pathFollow.Progress += (float)delta * direction * Speed;
		//Finds the vector diference between where _pathfollow was and where it is now
		//this is the vector the mob needs to move with
		Vector2 velocity = _pathFollow.Position - lastPosition;

		//If _pathFollow has reached either end of the path
		if((_pathFollow.ProgressRatio == 1) || (_pathFollow.ProgressRatio == 0))
		{
			GoesRight = !GoesRight;//toggle the direction, so that the mob will go back and forth along the path
		}
		//make the mob's position match _pathfollows
		Position += velocity;
	}

	/// <summary>
	/// When the bullet encounters an instance of an Area2D, Respond appropriately
	/// </summary>
	/// <param name="area"></param>
	private void OnAreaEntered(Area2D area)
	{
		//If the mob is hit with a bullet let main know, and then remove itself from the scene
		if(area.IsInGroup("Bullet"))
		{
			EmitSignal(SignalName.MobSlain, this);
			QueueFree();

		}
	}

}
