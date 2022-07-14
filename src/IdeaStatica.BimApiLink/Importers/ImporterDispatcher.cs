using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	internal class ImporterDispatcher
	{
		private readonly Dictionary<Type, object> _importers = new();

		public void AddImporter<T>(IImporter<T> importer)
			where T : IIdeaObject
		{
			_importers.Add(typeof(T), importer);
		}

		public T Get<T>(IIdentifier<T> identifier)
			where T : IIdeaObject
		{
			if (_importers.TryGetValue(typeof(T), out object? obj))
			{
				IImporter<T> importer = (IImporter<T>)obj;
				return importer.Get(identifier);
			}
		}
	}
}