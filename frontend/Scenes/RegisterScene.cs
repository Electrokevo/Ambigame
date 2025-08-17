using Godot;
using System;

public partial class RegisterScene : MarginContainer
{
	public void GoToLogin()
	{
		GetTree().ChangeSceneToFile("res://Scenes/LoginScene.tscn");
	}
}
