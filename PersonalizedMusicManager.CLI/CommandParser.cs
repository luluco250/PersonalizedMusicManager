using System;
using System.Collections.Generic;

namespace PersonalizedMusicManager.CLI
{
	sealed class CommandParser
	{
		public delegate void CommandAction(ReadOnlySpan<string> args);

		public Dictionary<string, CommandAction> CommandMap { get; }
			= new Dictionary<string, CommandAction>();

		public void Parse(ReadOnlySpan<string> args)
		{
			if (args.Length < 1)
				return;

			var command = args[0]?.ToLower()
				?? throw new ArgumentNullException();

			if (!CommandMap.TryGetValue(command, out var action))
				throw new ArgumentException($"Unknown \"{command}\" command");

			var commandArgs = args[1..];
			action(commandArgs);
		}
	}
}
