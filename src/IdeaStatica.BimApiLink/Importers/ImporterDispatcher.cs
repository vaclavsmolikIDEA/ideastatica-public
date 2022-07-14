using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	internal class ImporterDispatcher
	{
		private Dictionary<Type, object> _importers = new();

		public void AddImporter<T>(IImporter<T> importer)
			where T : IIdeaObject
		{
			_importers.Add(typeof(T), importer);
		}

		public IIdeaObject Get(IIdentifier<IIdeaObject> identifier)
		{
			throw new NotImplementedException();
		}

		public T Get<T>(IIdentifier<T> importer)
			where T : IIdeaObject
		{
			throw new NotImplementedException();
		}
	}
}