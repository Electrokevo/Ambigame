using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace Snake;

public partial class SnakeBody : Sprite2D
{
	[Signal]
	public delegate void GameOverEventHandler();

	[Export] DualGridTilemap DualGrid;

	private LinkedList<Vector2I> _body;
	private bool _crash;

	private Direction _direction;
	private bool _eat;

	private double _time;

	public override void _Ready()
	{
		_direction = Direction.RIGHT;
		_body = new([new(1, 0), new(0, 0)]) ;
		ZIndex = 1;
	}

	public override void _Draw()
	{
		foreach (var pos in _body)
		{
			GD.Print(pos);
			Vector2I coords = new() { X = pos.X, Y = pos.Y };
			DualGrid.SetTile(coords, DualGrid.dirtPlaceholderAtlasCoord);
		}
	}

	public bool TryEat(Apple apple)
	{
		Debug.Assert(_body != null, nameof(_body) + " != null");
		if (_body.First.Value.X == apple.Position.X && _body.First.Value.Y == apple.Position.Y)
		{
			GD.Print("Eat Apple!");
			_eat = true;
		}
		return _eat;
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
		if (_time > 0.2)
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
					newVect = new Vector2I(34, newVect.Y);
				if (newVect.X > 34)
					newVect = new Vector2I(0, newVect.Y);
				if (newVect.Y < 0)
					newVect = new Vector2I(newVect.X, 21);
				if (newVect.Y > 21)
					newVect = new Vector2I(newVect.X, 0);

				_body.AddFirst(newVect);
				if (!_eat)
				{
					var last = _body.Last.Value;
					_body.RemoveLast();
					DualGrid.SetTile(last, DualGrid.grassPlaceholderAtlasCoord);
				}
				if (Crash())
				{
					GD.Print("CRASH! Game Over");
					_crash = true;
					EmitSignal(SignalName.GameOver);
				}
			}
			if (!_crash)
				QueueRedraw();
			_eat = false;
			_time = 0;
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
