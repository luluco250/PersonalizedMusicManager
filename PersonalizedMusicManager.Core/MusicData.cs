using System.Collections.Generic;

namespace PersonalizedMusicManager.Core
{
	public sealed class MusicData
	{
		public string MusicPath { get; set; } = "";

		public string PlaylistsPath { get; set; } = "";

		public IDictionary<string, List<string>> Playlists { get; set; }
			= new Dictionary<string, List<string>>();
	}
}
