using System;
using System.Threading.Tasks;
using PersonalizedMusicManager.Core;

namespace PersonalizedMusicManager.CLI
{
	static class Program
	{
		const int ExitSuccess = 0;
		const int ExitError = 1;
		const int ExitConfig = 2;

		static async Task<int> Main()
		{
			try
			{
				var manager = new MusicManager();

				if (!(await manager.TryLoadMusicDataAsync()))
				{
					Console.WriteLine(
						"No music configuration file found, creating one...");

					await manager.CreateMusicDataAsync();
					await manager.SaveMusicDataAsync();

					Console.WriteLine(
						"Done. Configure the paths to your music folder and " +
						"the personalized music folder in " +
						$"\"{MusicManager.MusicFilename}\" and re-run this " +
						"program so that it'll generate the playlists file.");

					return ExitConfig;
				}

				Console.WriteLine("Loaded music data.");
				Console.Out.Flush();

				if (!(await manager.TryLoadPlaylistsDataAsync()))
				{
					Console.WriteLine(
						"No playlists configuration file found, creating one...");

					await manager.CreatePlaylistsDataAsync();
					await manager.SavePlaylistsDataAsync();

					Console.WriteLine(
						"Done. You can now configure your music playlists in " +
						$"\"{MusicManager.MusicFilename}\" according to the " +
						$"playlists defined in \"{MusicManager.PlaylistsFilename}\" " +
						"and re-run this program to synchronize the files.");

					return ExitConfig;
				}

				Console.WriteLine("Loaded playlists data.");
				Console.WriteLine("Synchronizing music files...");
				Console.Out.Flush();

				await manager.SyncFoldersAsync();
				Console.WriteLine("Done, enjoy your music!");

				return ExitSuccess;
			}
			catch (NotImplementedException)
			{
				await Console.Error.WriteLineAsync("Operation not implemented");
			}
			catch (Exception e)
			{
				await Console.Error.WriteLineAsync($"Fatal error: {e.Message}");
			}

			return ExitError;
		}
	}
}
