using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatiCa.BimApi;

namespace IdeaStatica.BimApiLink.Importers
{
	public interface IImporter<T>
		where T : IIdeaObject
	{
		T Get(IIdentifier<T> identifier);
	}
}