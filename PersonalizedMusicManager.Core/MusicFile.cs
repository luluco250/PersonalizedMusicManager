namespace PersonalizedMusicManager.Core
{
	public sealed class MusicFile
	{
		public string FilePath { get; }

		public MusicFile(string filePath)
			=> FilePath = filePath;
	}
}
