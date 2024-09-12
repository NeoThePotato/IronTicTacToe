using IronEngine;
using IronEngine.IO;
using IronEngine.DefaultRenderer;
using static IronEngine.ICommandAble;

namespace TicTacToe
{
	/// <summary>
	/// The execution <see cref="Runtime"/> of the <see cref="TicTacToe"/> game.
	/// </summary>
	public class TicTacToeRuntime : Runtime
	{
		#region CONSTS
		public const int BOARD_SIZE = 3;
		const int X_INDEX = 0;
		const int Y_INDEX = 1;
		#endregion

		#region FIELDS_AND_PROPERTIES
		public Stack<Command> UndoLog { get; set; }
		public Stack<Command> RedoLog { get; set; }
		public new static TicTacToeRuntime Instance => Runtime.Instance as TicTacToeRuntime;
		public Player X => Actors.ElementAt(X_INDEX) as Player;
		public Player O => Actors.ElementAt(Y_INDEX) as Player;

		/// <summary>
		/// Exits the game once a player completes a 3-<see cref="Marker"/> long chain. Or once all <see cref="Tile"/>s are occupied.
		/// </summary>
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
		public override IInput Input => _input;

		private readonly ConsoleInput _input;
		private IEnumerable<Tile>? _completedChain;
		#endregion

		#region	METHODS
		public TicTacToeRuntime()
		{
			UndoLog = new(BOARD_SIZE * BOARD_SIZE);
			RedoLog = new(BOARD_SIZE * BOARD_SIZE);
			_input = new ConsoleInput();
			_input.SelectCommandAblePrompt = "Select action:";
		}

		/// <summary>
		/// Creates 2 <see cref="Player"/>s for the <see cref="Runtime"/>.
		/// </summary>
		/// <returns>The newly-created <see cref="Player"/>s.</returns>
		protected override IEnumerable<Actor> CreateActors()
		{
			yield return new Player();
			yield return new Player();
		}

		protected override IRenderer CreateRenderer() => new ConsoleRenderer(TileMap);

		/// <summary>
		/// Create an empty <see cref="TileMap"/> and feeds it back to the <see cref="Runtime"/>.
		/// </summary>
		/// <returns>The created <see cref="TileMap"/>.</returns>
		protected override TileMap CreateTileMap()
		{
			var map = new TileMap(BOARD_SIZE, BOARD_SIZE, new Tile());
			foreach (var tile in map.Checkerboard().Cast<Tile>())
				tile.BgColor = (byte)ConsoleColor.Gray;
			foreach (var tile in map.Checkerboard(1).Cast<Tile>())
				tile.BgColor = (byte)ConsoleColor.DarkGray;
			return map;
		}

		/// <summary>
		/// When the <see cref="Runtime"/> exits, print the result of the game to the <see cref="Console"/>.
		/// Highlight the winning chain, if exists.
		/// </summary>
		protected override void OnExit()
		{
			if (_completedChain != null)
			{
				HighlightChain(_completedChain, ConsoleColor.Yellow);
				var winner = _completedChain.First().Object.Actor;
				Console.WriteLine($"Game ended.\n{winner} wins.");
			}
			else
				Console.WriteLine("Game ended.\nIt's a draw.");
			Input.GetString("Press any key to exit.");

			void HighlightChain(IEnumerable<Tile> chain, ConsoleColor color)
			{
				foreach (var tile in chain.Cast<Tile>())
					tile.BgColor = (byte)color;
				Renderer.UpdateFrame();
			}
		}

		/// <summary>
		/// Pushes <paramref name="command"/> to the undo/redo logs. Once it is selected by the user.
		/// </summary>
		protected override void OnCommandSelected(ref Command command)
		{
			if (command.Key == "Deselect")
				return;
			else if (command.Key == Player.UndoRedo.UNDO_KEY)
				RedoLog.Push(Invert(UndoLog.Pop()));
			else if (command.Key == Player.UndoRedo.REDO_KEY)
				UndoLog.Push(Invert(RedoLog.Pop()));
			else
			{
				RedoLog.Clear();
				UndoLog.Push(Invert(command));
			}

			static Command Invert(Command command)
			{
				string key = command.Key == Player.UndoRedo.UNDO_KEY ? Player.UndoRedo.REDO_KEY : Player.UndoRedo.UNDO_KEY;
				return new(command.undo, command.description, key, command.endsTurn, command.action);
			}
		}
		#endregion
	}
}
