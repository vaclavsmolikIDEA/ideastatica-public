using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	internal class ImporterManager
	{
		private readonly Dictionary<Type, object> _importers = new();
		private readonly List<IImporterProvider> _providers = new();

		public void Register<T>(IImporter<T> importer)
			where T : IIdeaObject
		{
			_importers.Add(typeof(T), importer);
		}

		public void RegisterProvider(IImporterProvider provider)
		{
			_providers.Add(provider);
		}

		public bool IsRegistered<T>()
			where T : IIdeaObject
		{
			if (_importers.ContainsKey(typeof(T)))
			{
				return true;
			}

			if (_providers.Any(x => x.CanProvide<T>()))
			{
				return true;
			}

			return false;
		}
	}
}