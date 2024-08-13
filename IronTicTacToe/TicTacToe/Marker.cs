using DefaultRenderer.Defaults;

namespace TicTacToe
{
	internal class Marker : RenderableTileObject
	{
		private static readonly string[] X_MARKER = [
				@"\  /",
				@" || ",
				@"/  \"
				];
		private static readonly string[] O_MARKER = [
				@"/--\",
				@"|  |",
				@"\__/"
				];

		public Marker(Player player) : base(player) => Chars = player.PlayerMarker == 'X' ? X_MARKER : O_MARKER;
	}
}
