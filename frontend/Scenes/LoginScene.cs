using Godot;
using System;
using System.Text;
using Newtonsoft.Json;
using Snakes.Models;

public partial class LoginScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	HttpRequest httpRequest;
	private TextEdit _username;
	private TextEdit _password;

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
		_username = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/UsernameBox");
		_password = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/PasswordBox");
		httpRequest = GetNode<HttpRequest>("HTTPRequest");
		httpRequest.RequestCompleted += OnRequestCompleted;
		string body = JsonConvert.SerializeObject(new
		{
			username = _username.Text,
			password = _password.Text
		});
		httpRequest.Request($"{url}players/login", headers, HttpClient.Method.Post, body) ;
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
