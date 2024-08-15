using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;
using static IronEngine.ICommandAble;

namespace TicTacToe
{
	public class TicTacToeRuntime : Runtime
	{
		#region CONSTS
		public const int BOARD_SIZE = 3;
		const int X_INDEX = 0;
		const int Y_INDEX = 1;
		#endregion

		#region FIELDS_AND_PROPERTIES
		public Stack<Command> CommandLog { get; set; }
		public new static TicTacToeRuntime Instance => Runtime.Instance as TicTacToeRuntime;
		public Player X => Actors.ElementAt(X_INDEX) as Player;
		public Player O => Actors.ElementAt(Y_INDEX) as Player;
		public override bool ExitCondition
		{
			get
			{
				_completedChain = TileMap.GetAllPossibleChains().FirstOrDefault(AllSameActor);
				return _completedChain != null || AllMarkersPlaced();

				bool AllSameActor(IEnumerable<Tile> chain)
				{
					if (chain.First().TryGetObject<Marker>(out var marker))
					{
						var actor = marker!.Actor;
						return chain.All(t => t.TryGetObject<Marker>(out var marker) && marker.Actor == actor);
					}
					return false;
				}

				bool AllMarkersPlaced() => TileMap.All(t => t.HasObject);
			}
		}
		public override IInput Input => IInput.ConsoleInput;


		private IEnumerable<Tile>? _completedChain;
		#endregion

		#region	METHODS
		public TicTacToeRuntime()
		{
			CommandLog = new(BOARD_SIZE * BOARD_SIZE);
		}

		protected override IEnumerable<Actor> CreateActors()
		{
			yield return new Player();
			yield return new Player();
		}

		protected override IRenderer CreateRenderer() => new ConsoleRenderer(TileMap);

		protected override TileMap CreateTileMap()
		{
			var map = new TileMap(BOARD_SIZE, BOARD_SIZE, new Tile());
			foreach (var tile in map.Checkerboard().Cast<Tile>())
				tile.BgColor = (byte)ConsoleColor.Gray;
			foreach (var tile in map.Checkerboard(1).Cast<Tile>())
				tile.BgColor = (byte)ConsoleColor.DarkGray;
			return map;
		}

		protected override void OnExit()
		{
			if (_completedChain != null)
			{
				Player winner = _completedChain.First().Object.Actor as Player;
				foreach (var tile in _completedChain.Cast<Tile>())
					tile.BgColor = (byte)ConsoleColor.Yellow;
				Console.WriteLine($"Game ended.\n{winner} wins.");
			}
		}

		protected override void OnCommandSelected(Command command)
		{
			if (command.Key != "Undo" && command.Key != "Deselect")
				CommandLog.Push(command);
		}
		#endregion
	}
}
