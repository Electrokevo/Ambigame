using Godot;
using System;

public partial class MainScene : MarginContainer
{
	public void Play()
	{
		GetTree().ChangeSceneToFile("res://Scenes/Snake.tscn");
	}

	public void GoToRanking()
	{
		GetTree().ChangeSceneToFile("res://Scenes/RankingScene.tscn");
	}
	
	public void GoToHistorial()
	{
		GetTree().ChangeSceneToFile("res://Scenes/HistorialScene.tscn");
	}

	public void GoToTutorial()
	{
		GetTree().ChangeSceneToFile("res://Scenes/TutorialScene.tscn");
	}

	public void LogOut()
	{
		GetTree().ChangeSceneToFile("res://Scenes/LoginScene.tscn");
	}
}
