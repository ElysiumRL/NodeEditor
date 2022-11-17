using System.IO;
using Newtonsoft.Json;
using ElysiumAttributes;

namespace ElysiumUtilities
{
	public static class JSONTools
	{
		[ExcludeFromNodeBuild]
		private static T ImportFromJsonStream<T>(this Stream stream)
		{
			JsonSerializer serializer = new JsonSerializer();
			T data;
			using (StreamReader streamReader = new StreamReader(stream))
			{
				data = (T)serializer.Deserialize(streamReader, typeof(T));
			}
			return data;
		}
		[ExcludeFromNodeBuild]
		public static T ImportFromJsonString<T>(this string json)
		{
			T data;
			using (MemoryStream stream = new MemoryStream(System.Text.Encoding.Default.GetBytes(json)))
			{
				data = ImportFromJsonStream<T>(stream);
			}
			return data;
		}
		[ExcludeFromNodeBuild]
		public static T ImportFromJsonFile<T>(this string fileName)
		{
			T data;
			using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
			{
				data = ImportFromJsonStream<T>(fileStream);
			}
			return data;
		}
		[ExcludeFromNodeBuild]
		private static void ExportJsonFromClassStream(this Stream stream, object classToExport)
		{
			JsonSerializer serializer = new JsonSerializer();
			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				serializer.Serialize(streamWriter, classToExport);
			}
		}
		[ExcludeFromNodeBuild]
		public static void ExportJsonFromClassFile(this string fileName, object classToExport)
		{
			JsonSerializer serializer = new JsonSerializer();
			using (StreamWriter streamWriter = new StreamWriter(fileName)) 
			{
				serializer.Serialize(streamWriter, classToExport);
			}
		}
	}
}
