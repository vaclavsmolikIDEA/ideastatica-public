using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	public interface IImporter { }

	public interface IImporter<T>: IImporter
		where T : IIdeaObject
	{
		T Get(IIdentifier<T> identifier);
	}
}