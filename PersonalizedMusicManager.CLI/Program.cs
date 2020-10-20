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

		static int Main()
		{
			try
			{
				var manager = new MusicManager();

				if (!manager.TryLoadMusicDataAsync().Result)
				{
					Console.WriteLine(
						"No music configuration file found, creating one...");

					manager.CreateMusicDataAsync().Wait();
					manager.SaveMusicDataAsync().Wait();

					Console.WriteLine(
						"Done. Configure the paths to your music folder and " +
						"the personalized music folder in " +
						$"\"{MusicManager.MusicFilename}\" and re-run this " +
						"program so that it'll generate the playlists file.");

					return ExitConfig;
				}

				Console.WriteLine("Loaded music data.");
				Console.Out.Flush();

				if (!manager.TryLoadPlaylistsDataAsync().Result)
				{
					Console.WriteLine(
						"No playlists configuration file found, creating one...");

					manager.CreatePlaylistsDataAsync().Wait();
					manager.SavePlaylistsDataAsync().Wait();

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

				manager.SyncFoldersAsync().Wait();
				Console.WriteLine("Done, enjoy your music!");

				return ExitSuccess;
			}
			catch (NotImplementedException)
			{
				Console.Error.WriteLine("Operation not implemented");
			}
			catch (Exception e)
			{
				Console.Error.WriteLine($"Fatal error: {e.Message}");
			}

			return ExitError;
		}
	}
}
