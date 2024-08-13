using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;
using static IronEngine.ICommandAble;

namespace TicTacToe
{
	internal class TicTacToeRuntime : Runtime
	{
		public const int BOARD_SIZE = 3;
		const int X_INDEX = 0;
		const int Y_INDEX = 1;

		public Stack<Command> CommandLog { get; set; }

		public new static TicTacToeRuntime Instance => Runtime.Instance as TicTacToeRuntime;

		public Player X => Actors.ElementAt(X_INDEX) as Player;
		public Player O => Actors.ElementAt(Y_INDEX) as Player;

		public override bool ExitCondition
		{
			get
			{
				return false; // TODO Implement Exit condition
			}
		}

		public override IInput Input => IInput.ConsoleInput;

		public TicTacToeRuntime()
		{
			CommandLog = new(BOARD_SIZE * BOARD_SIZE);
		}

		protected override IEnumerable<Actor> CreateActors()
		{
			yield return new Player(Input.GetString("X, please enter your name: "));
			yield return new Player(Input.GetString("O, please enter your name: "));
		}

		protected override IRenderer CreateRenderer() => new ConsoleRenderer(TileMap);

		protected override TileMap CreateTileMap()
		{
			var map = new TileMap(BOARD_SIZE, BOARD_SIZE, new Tile());
			foreach (var tile in map.Checkerboard().Cast<Tile>())
				tile.BgColor = ConsoleRenderer.COLOR_WHITE;
			return map;
		}

		protected override void OnExit()
		{
			if (CurrentActor is Player winner)
				Console.WriteLine($"Game ended.\n{winner} wins.");
		}

		protected override void OnCommandSelected(Command command)
		{
			if (command.Key != "Undo")
				CommandLog.Push(command);
		}
	}
}
