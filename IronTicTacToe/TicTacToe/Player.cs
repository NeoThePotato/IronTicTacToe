using IronEngine;
using static IronEngine.ICommandAble;

namespace TicTacToe
{
	/// <summary>
	/// Represents a single player in a <see cref="TicTacToe"/> game.
	/// </summary>
	public class Player : Actor
	{
		public char PlayerMarker => this == TicTacToeRuntime.Instance.X ? 'X' : 'O';

		private readonly ICommandAble _placeMarker, _undoRedo;

		public Player()
		{
			_placeMarker = new PlaceMarker();
			_undoRedo = new UndoRedo();
		}

		public override string ToString() => PlayerMarker.ToString();

		protected override IEnumerable<ICommandAble> FilterCommandAble(IEnumerable<ICommandAble> source)
		{
			source = source.Append(_placeMarker);
			if (_undoRedo.GetAvailableActions().Any())
				source = source.Append(_undoRedo);
			return source;
		}

		/// <summary>
		/// Allows the <see cref="Player"/> to place a <see cref="Marker"/> on the <see cref="TileMap"/>.
		/// </summary>
		private class PlaceMarker : ICommandAble, IHasKey
		{
			public Actor? Actor => TicTacToeRuntime.Instance.CurrentActor;

			public Player? Player => Actor as Player;

			public string? Key => "Place";

			public string Description => $"Place {Player.PlayerMarker} on a tile.";

			public IEnumerable<Command> GetAvailableActions()
			{
				return GetAllPlaceMarkerCommands().OrderBy(c => c.Key);

				IEnumerable<Command> GetAllPlaceMarkerCommands()
				{
					var board = Runtime.Instance.TileMap;
					foreach (var tile in board.Where(t => !t.HasObject).Cast<Tile>())
						yield return new Command(() => PlaceMarker(tile), $"Place '{Player.PlayerMarker}' at {tile.ToStringLong()}.", tile.Key, undo: () => RemoveMarker(tile));
				}

				void PlaceMarker(Tile tile) => tile.Object = new Marker(Player);

				void RemoveMarker(Tile tile) => tile.Object = null;
			}
		}

		/// <summary>
		/// Allows the <see cref="Player"/> to undo or redo a previous <see cref="Command"/>.
		/// </summary>
		public class UndoRedo : ICommandAble, IHasKey
		{
			public const string UNDO_KEY = "Undo";
			public const string REDO_KEY = "Redo";

			public Actor? Actor => TicTacToeRuntime.Instance.CurrentActor;

			public string? Key => "Do";

			public string Description => $"Undo or Redo previous action.";

			public IEnumerable<Command> GetAvailableActions()
			{
				if (TicTacToeRuntime.Instance.UndoLog.TryPeek(out var undo))
					yield return undo;
				if (TicTacToeRuntime.Instance.RedoLog.TryPeek(out var redo))
					yield return redo;
			}
		}
	}
}
