using Godot;
using System;
using System.Text;
using Newtonsoft.Json;
using Snakes.Models;

public partial class LoginScene : MarginContainer
{
	[Export] private LineEdit UsernameBox;
	[Export] private LineEdit PasswordBox;
	[Export] private HttpRequest HTTPRequest;
	private static readonly string url = "http://localhost:3000/";
	
	public void GoToMain()
	{
		GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
	}

	public void GoToRegister()
	{
		GetTree().ChangeSceneToFile("res://Scenes/RegisterScene.tscn");
	}

	public void LogIn()
	{
		string[] headers = ["Content-Type: application/json"];
		HTTPRequest.RequestCompleted += OnRequestCompleted;
		string body = JsonConvert.SerializeObject(new
		{
			username = UsernameBox?.Text,
			password = PasswordBox?.Text
		});
		GD.Print(body);
		HTTPRequest.Request($"{url}players/login", headers, HttpClient.Method.Post, body) ;
	}
	
	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode == 200)
		{
			GD.Print($"Success, the request returned {responseCode}");
			GetTree().ChangeSceneToFile("res://Scenes/MainScene.tscn");
		}
		else
		{
			GD.Print($"Error, the request returned {responseCode}");
		}
	}
}
