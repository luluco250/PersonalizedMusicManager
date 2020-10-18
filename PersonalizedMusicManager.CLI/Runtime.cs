using System;

namespace PersonalizedMusicManager.CLI
{
	sealed class Runtime
	{
		readonly CommandParser _parser;

		public Runtime(CommandParser parser)
			=> _parser = parser;

		public void RegisterCommands()
		{
			_parser.CommandMap["help"] = _ => Help();
			_parser.CommandMap["init"] = _ => Init();
			_parser.CommandMap["sync"] = _ => Sync();
		}

		public void Parse(ReadOnlySpan<string> args)
			=> _parser.Parse(args);

		public void Help()
		{
			Console.WriteLine("Available commands:");

			foreach (var command in _parser.CommandMap.Keys)
				Console.WriteLine("  " + command);
		}

		public void Init()
		{

		}

		public void Sync()
		{

		}
	}
}
