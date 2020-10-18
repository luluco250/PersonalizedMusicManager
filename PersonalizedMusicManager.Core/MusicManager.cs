using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace PersonalizedMusicManager.Core
{
	public sealed class MusicManager
	{
		public const string PlaylistsFilename = "playlists.jsonc";

		public const string MusicFilename = "music.jsonc";

		Dictionary<string, Playlist>? _playlistMap;

		public IDictionary<string, Playlist>? PlaylistsData => _playlistMap;

		public MusicData? MusicData { get; private set; }

		public async Task<bool> TryLoadMusicDataAsync()
		{
			if (!File.Exists(MusicFilename))
				return false;

			using (var file = File.OpenRead(MusicFilename))
				MusicData = await Json.ReadAsync<MusicData>(file);

			return true;
		}

		public async Task CreateMusicDataAsync()
		{
			await Task.Run(() => MusicData = new MusicData());
		}

		public async Task SaveMusicDataAsync()
		{
			if (MusicData is null)
				throw new InvalidOperationException(
					"Music data must be created or loaded before saving");

			using (var file = File.OpenWrite(MusicFilename))
				await Json.WriteAsync(file, MusicData);
		}

		public async Task<bool> TryLoadPlaylistsDataAsync()
		{
			if (!File.Exists(PlaylistsFilename))
				return false;

			using (var file = File.OpenRead(PlaylistsFilename))
				_playlistMap = await Json.ReadAsync<Dictionary<string, Playlist>>(file);

			return true;
		}

		public async Task CreatePlaylistsDataAsync()
		{
			if (MusicData is null)
				throw new InvalidOperationException(
					"Cannot create playlist data without music data");

			await Task.Run(() =>
			{
				var playlists = Playlist.GenerateList(MusicData.PlaylistsPath);
				_playlistMap = new Dictionary<string, Playlist>();

				foreach (var p in playlists)
					_playlistMap[p.GetKey()] = p;
			});
		}

		public async Task SavePlaylistsDataAsync()
		{
			if (PlaylistsData is null)
				throw new InvalidOperationException(
					"Playlists data must be created or loaded before saving");

			using (var file = File.OpenWrite(PlaylistsFilename))
				await Json.WriteAsync(file, _playlistMap);
		}

		public async Task SyncFoldersAsync()
		{
			if (MusicData is null)
				throw new InvalidOperationException(
					"Music data must be created or loaded before syncing folders");

			if (PlaylistsData is null)
				throw new InvalidOperationException(
					"Playlists data must be created or loaded before syncing folders");

			var tasks = new List<Task>();

			foreach (var (name, music) in MusicData.Playlists)
				tasks.Add(Task.Run(() =>
				{
					if (!PlaylistsData.TryGetValue(name, out var info))
						throw new Exception($"Playlist \"{name}\" doesn't exist");

					if (music.Count > info.MaxTracks)
						throw new Exception(
							$"Playlist \"{name}\" exceeds the maximum track " +
							$"amount allowed of {info.MaxTracks}");

					var folder = Path.Combine(MusicData.PlaylistsPath, info.Folder);
					DeleteMusicFromPlaylistFolder(folder, info.Filename);
					CopyMusicToPlaylistFolder(MusicData.MusicPath, folder, info.Filename, music);
				}));

			await Task.WhenAll(tasks);
		}

		static void DeleteMusicFromPlaylistFolder(string folder, string filename)
		{
			foreach (var f in Directory.GetFiles(folder))
			{
				var name = Path.GetFileName(f);

				if (name.StartsWith(filename))
					File.Delete(f);
			}
		}

		static void CopyMusicToPlaylistFolder(
			string musicFolder,
			string playlistFolder,
			string filenameBase,
			IReadOnlyList<string> musicFiles)
		{
			for (var i = 0; i < musicFiles.Count; ++i)
			{
				var file = musicFiles[i];
				var ext = Path.GetExtension(file);
				var filename = $"{filenameBase}{i + 1:00}{ext}";

				var source = Path.Combine(musicFolder, file);
				var dest = Path.Combine(playlistFolder, filename);

				File.Copy(source, dest);
			}
		}
	}
}
