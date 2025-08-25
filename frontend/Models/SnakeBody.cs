using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;
using Snakes.Models;

namespace Snake;

public partial class SnakeBody : Sprite2D
{
	[Signal] public delegate void GameOverEventHandler();
	[Export] DualGridTilemap DualGrid;
	[Export] Label puntiacionLabel;
	[Export] Label recicladosLabel;
	[Export] Label timerLabel;

	[Export] Label statsLabel;
	[Export] CanvasLayer gameOverScreen;
	[Export] HttpRequest httpRequest;
	private string url = "http://localhost:3000/";

	private LinkedList<Vector2I> _body;
	private bool _crash;
	private Direction _direction;
	private double _time;

	private int reciclados = 0;
	public int Reciclados
	{
		get => reciclados;
		set
		{
			reciclados = value;
			UpdateRecicladosLabel();
		}
	}

	private void UpdateRecicladosLabel()
	{
		if (recicladosLabel != null)
			recicladosLabel.Text = $"Reciclados: {Reciclados}";
	}

	private double puntuacionBase = 100.0;
	private int puntuacion = 0;
	public int Puntuacion
	{
		get => puntuacion;
		set
		{
			puntuacion = value;
			UpdatePuntuacionLabel();
		}
	}

	private void UpdatePuntuacionLabel()
	{
		if (puntiacionLabel != null)
			puntiacionLabel.Text = $"Puntos: {Puntuacion}";
	}

	private double elapsedTime = 0;
	private double juegoTime = 0;
	private void UpdateTimerLabel()
	{
		if (timerLabel != null)
			timerLabel.Text = $"Tiempo: {juegoTime}";
	}



	public override void _Ready()
	{
		_direction = Direction.RIGHT;
		_body = new([new(1, 0), new(0, 0)]);
		ZIndex = 1;
		gameOverScreen.Visible = false;
	}

	public override void _Draw()
	{
		foreach (var pos in _body)
		{
			Vector2I coords = new() { X = pos.X, Y = pos.Y };
			DualGrid.SetTile(coords, DualGrid.grassPlaceholderAtlasCoord);
		}
	}

	public bool TryEat()
	{
		Debug.Assert(_body != null, nameof(_body) + " != null");
		var headPosition = _body.First.Value;
		if (DualGrid.HasTrashAt(headPosition))
		{
			Reciclados++;
			Puntuacion += (int)(puntuacionBase * (_body.Count / 10.0));
			DualGrid.RemoveTrashAt(headPosition);
			return true;
		}
		return false;
	}

	public bool Crash()
	{
		return _body
			.Skip(1)
			.Any(t =>
			{
				return t.X == _body.First.Value.X && t.Y == _body.First.Value.Y;
			});
	}

	public override void _Process(double delta)
	{
		_time += delta;
		elapsedTime += delta;
		if (elapsedTime > 1 && !_crash)
		{
			juegoTime++;
			UpdateTimerLabel();
			elapsedTime = 0;
		}
		if (_time > 0.2 && !_crash)
		{
			var translation = _direction switch
			{
				Direction.RIGHT => new Vector2I(1, 0),
				Direction.LEFT => new Vector2I(-1, 0),
				Direction.UP => new Vector2I(0, -1),
				Direction.DOWN => new Vector2I(0, 1),
				_ => new Vector2I(0, 0)
			};
			if (_body.Count > 0)
			{
				var newVect = new Vector2I(_body.First.Value.X, _body.First.Value.Y);
				newVect += translation;
				if (newVect.X < 0)
					newVect = new Vector2I(33, newVect.Y);
				if (newVect.X > 33)
					newVect = new Vector2I(0, newVect.Y);
				if (newVect.Y < 0)
					newVect = new Vector2I(newVect.X, 21);
				if (newVect.Y > 21)
					newVect = new Vector2I(newVect.X, 0);

				_body.AddFirst(newVect);
				if (!TryEat())
				{
					var last = _body.Last.Value;
					_body.RemoveLast();
					DualGrid.SetTile(last, DualGrid.dirtPlaceholderAtlasCoord);
				}
				if (Crash())
				{
					_crash = true;
					gameOverScreen.Visible = true;
					statsLabel.Text = $"Puntuacion: {Puntuacion}\nReciclados: {Reciclados}\nTiempo: {juegoTime} segundos";
					SendMatchToBackend();
					EmitSignal(SignalName.GameOver);
				}
			}
			if (!_crash)
				QueueRedraw();
			_time = 0;
		}
	}

	private void SendMatchToBackend()
	{
		string[] headers = ["Content-Type: application/json"];
		httpRequest.RequestCompleted += OnRequestCompleted;
		Match match = new ()
		{
			score = Puntuacion,
			player_id = Convert.ToInt32(double.Parse(Player.GetInstance().id, System.Globalization.CultureInfo.InvariantCulture)),
			date = DateTime.Now.ToString("yyyy-MM-dd"),
			level = 1,
			time = (int)juegoTime,
			recolected = reciclados,
		};
		string body = JsonConvert.SerializeObject(match);
		GD.Print(body);
		httpRequest.Request($"{url}matches", headers, HttpClient.Method.Post, body);
	}

	private void OnRequestCompleted(long result, long responseCode, string[] headers, byte[] body)
	{
		if (responseCode == 200)
		{
			GD.Print($"Success, the request returned {responseCode}");
		}
		else
		{
			GD.Print($"Error, the request returned {responseCode}");
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsAction("ui_left") && _direction != Direction.RIGHT)
		{
			_direction = Direction.LEFT;
			return;
		}

		if (@event.IsAction("ui_right") && _direction != Direction.LEFT)
		{
			_direction = Direction.RIGHT;
			return;
		}

		if (@event.IsAction("ui_up") && _direction != Direction.DOWN)
		{
			_direction = Direction.UP;
			return;
		}

		if (@event.IsAction("ui_down") && _direction != Direction.UP)
			_direction = Direction.DOWN;
	}

	private enum Direction
	{
		LEFT,
		RIGHT,
		UP,
		DOWN,
	}
}
