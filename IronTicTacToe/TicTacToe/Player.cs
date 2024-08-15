using IronEngine;
using static IronEngine.ICommandAble;

namespace TicTacToe
{
	public class Player : Actor
	{
		public char PlayerMarker => this == TicTacToeRuntime.Instance.X ? 'X' : 'O';

		public TileObject LastPlaced { get; set; }

		public override string ToString() => PlayerMarker.ToString();

		protected override IEnumerable<ICommandAble> FilterCommandAble(IEnumerable<ICommandAble> source)
		{
			source = source.Append(new PlaceMarker());
			if (TicTacToeRuntime.Instance.CommandLog.Count > 0)
				source = source.Append(new Undo());
			return source;
		}

		private class PlaceMarker : ICommandAble, IHasKey
		{
			public Actor? Actor => TicTacToeRuntime.Instance.CurrentActor;

			public Player? Player => Actor as Player;

			public string? Key => "Place";

			public string Description => $"Place {Player.PlayerMarker} on a tile.";

			public IEnumerable<Command> GetAvailableActions()
			{
				var board = Runtime.Instance.TileMap;
				foreach (var tile in board.Where(t => !t.HasObject).Cast<Tile>())
					yield return new Command(() => PlaceMarker(tile), $"Place '{Player.PlayerMarker}' at {tile.ToStringLong()}.", tile.Key, undo: () => RemoveMarker(tile));

				void PlaceMarker(Tile tile) => tile.Object = new Marker(Player);

				void RemoveMarker(Tile tile) => tile.Object = null;
			}
		}

		private class Undo : ICommandAble, IHasKey
		{
			public Actor? Actor => TicTacToeRuntime.Instance.CurrentActor;

			public string? Key => "Undo";

			public string Description => $"Undo previous action.";

			public IEnumerable<Command> GetAvailableActions()
			{
				yield return new Command(() => { TicTacToeRuntime.Instance.CommandLog.Pop().undo(); }, $"Undo \"{TicTacToeRuntime.Instance.CommandLog.Peek().Description}\".", "Undo");
			}
		}
	}
}
