using Godot;
using System;
using Newtonsoft.Json;

public partial class LoginScene : MarginContainer
{
	private static readonly string url = "http://localhost:3000/";
	private TextEdit _username;
	private TextEdit _password;
	public override void _Ready()
	{
		_username = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/UsernameBox");
		_password = GetNode<TextEdit>("HBoxContainer/VBoxContainer/Fields/PasswordBox");
	}
	public void CheckLogIn()
	{
		string[] headers = ["Content-Type: application/json"];
		HttpRequest httpRequest = GetNode<HttpRequest>("HTTPRequest");
		string body = JsonConvert.SerializeObject(
			new
			{
				username = _username.Text,
				password = _password.Text
			});	
		httpRequest.Request(url, headers, HttpClient.Method.Post, body) ;
		Godot.GD.Print(body);
	}
}
