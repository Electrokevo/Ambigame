using Godot;
using System;
using Newtonsoft.Json;
using Snakes.Models;

public partial class RegisterScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	HttpRequest httpRequest;
	private TextEdit _username;
	private TextEdit _password;
	private TextEdit _password2;
	public void GoToLogin()
	{
		GetTree().ChangeSceneToFile("res://Scenes/LoginScene.tscn");
	}
	
	public void Register()
	{
		GD.Print("hola");
		string[] headers = ["Content-Type: application/json"];
		_username = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/UsernameBox");
		_password = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/PasswordBox");
		_password2 = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/PasswordBox2");
		httpRequest = GetNode<HttpRequest>("HTTPRequest");
		httpRequest.RequestCompleted += OnRequestCompleted;
		string body = JsonConvert.SerializeObject(new
		{
			username = _username.Text,
			password = _password.Text,
			password_confirmation = _password2.Text
		});
		httpRequest.Request($"{url}players/register", headers, HttpClient.Method.Post, body) ;
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
