using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PersonalizedMusicManager.Core
{
	public static class Json
	{
		public static async ValueTask<T> ReadAsync<T>(Stream stream)
		{
			return await JsonSerializer.DeserializeAsync<T>(
				stream,
				new JsonSerializerOptions
				{
					AllowTrailingCommas = true,
					ReadCommentHandling = JsonCommentHandling.Skip,
					PropertyNameCaseInsensitive = true
				});
		}

		public static async Task WriteAsync<T>(Stream stream, T value)
		{
			await JsonSerializer.SerializeAsync<T>(
				stream,
				value,
				new JsonSerializerOptions
				{
					WriteIndented = true
				});
		}

		public static async Task<string> WriteAsync<T>(T value)
		{
			var stream = new MemoryStream();
			await WriteAsync(stream, value);

			stream.Seek(0, SeekOrigin.Begin);

			return await Task.Run(() => Encoding
				.UTF8
				.GetString(stream.GetBuffer(), 0, (int)stream.Length));
		}

		public static string Format(string json)
		{
			return json.Replace("  ", "\t");
		}
	}
}
