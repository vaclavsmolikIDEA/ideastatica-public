using IdeaStatiCa.BimImporter.Persistence;

namespace IdeaStatica.BimApiLink.Persistence
{
	internal class JsonProjectStorage : IProjectStorage
	{
		private const string PersistencyStorage = "bimapi-data.json";

		private readonly string _path;
		private readonly JsonPersistence _jsonPersistence;

		public JsonProjectStorage(string workingDirectory)
		{
			_path = Path.Combine(workingDirectory, PersistencyStorage);
			_jsonPersistence = new JsonPersistence();
		}

		public void Save()
		{
			using FileStream fs = File.OpenWrite(_path);
			using StreamWriter streamWriter = new(fs);

			_jsonPersistence.Save(streamWriter);
		}

		public void Load()
		{
			if (File.Exists(_path))
			{
				using FileStream fs = File.OpenRead(_path);
				using StreamReader streamReader = new(fs);

				_jsonPersistence.Load(streamReader);
			}
		}
	}
}