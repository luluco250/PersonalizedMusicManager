using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PersonalizedMusicManager.Core
{
	public sealed class Playlist
	{
		static Regex TrackFilePattern = new Regex(
			@"Track limit (\d+)",
			RegexOptions.Compiled | RegexOptions.IgnoreCase);

		public string Folder { get; set; } = "";

		public string Filename { get; set; } = "";

		public int MaxTracks { get; set; }

		public string GetKey()
			=> Filename.Substring(2);

		public static List<Playlist> GenerateList(string foldersPath)
		{
			if (!Directory.Exists(foldersPath))
				throw new ArgumentException($"Path \"{foldersPath}\" not found");

			var list = new List<Playlist>();

			foreach (var dir in Directory.GetDirectories(foldersPath))
			{
				if (dir is null)
					continue;

				var maxTracksMatch = Directory
					.GetFiles(Path.Combine(foldersPath, dir))
					.Select(x => TrackFilePattern.Match(x))
					.Where(x => x.Success)
					.FirstOrDefault();

				if (maxTracksMatch is null)
					continue;

				var name = new DirectoryInfo(dir).Name;

				list.Add(new Playlist
				{
					Folder = name,
					Filename = "MM" + string.Join("", name.Split(new[] {' ', '-'})),
					MaxTracks = int.Parse(maxTracksMatch.Groups[1].ToString()),
				});
			}

			return list;
		}
	}
}
