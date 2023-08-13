//By: James Singleton
//Date: 8/12/2023
using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
	[Signal]
    public delegate void StartLevelEventHandler(int levelNum);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	/// <summary>
	/// When player hits the Level 1 Button, tells main to load Level_1
	/// </summary>
	private void OnLevel1ButtonPressed()
	{
		EmitSignal(SignalName.StartLevel, 1);
	}

	// <summary>
	/// When player hits the Level 2 Button, tells main to load Level_2
	/// </summary>
	private void OnLevel2ButtonPressed()
	{
		EmitSignal(SignalName.StartLevel, 2);
	}

	/// <summary>
	/// Exits the game
	/// </summary>
	private void OnExitButtonPressed()
	{
		GetTree().Quit();
	}
}
