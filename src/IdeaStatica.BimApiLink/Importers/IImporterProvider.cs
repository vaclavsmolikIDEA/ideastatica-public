using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	public interface IImporterProvider
	{
		bool CanProvide<T>()
			where T : IIdeaObject;

		T Provide<T>()
			where T : IIdeaObject;
	}
}