using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;

namespace TicTacToe
{
	internal class TicTacToeRuntime : Runtime
	{
		const int BOARD_SIZE = 3;
		const int X_INDEX = 0;
		const int Y_INDEX = 1;

		public new static TicTacToeRuntime Instance => Runtime.Instance as TicTacToeRuntime;

		public Player X => Actors.ElementAt(X_INDEX) as Player;
		public Player O => Actors.ElementAt(Y_INDEX) as Player;

		public override bool ExitCondition => false; // TODO Implement exit condition

		public override IInput Input => IInput.ConsoleInput;

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
	}
}
