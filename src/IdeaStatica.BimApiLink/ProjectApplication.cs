using IdeaRS.OpenModel;
using IdeaStatica.BimApiLink.Identifiers;
using IdeaStatica.BimApiLink.Persistence;
using IdeaStatica.BimApiLink.Scope;
using IdeaStatiCa.BimApi;
using IdeaStatiCa.BimImporter;
using IdeaStatiCa.Plugin;

namespace IdeaStatica.BimApiLink
{
	public abstract class ProjectApplication : ApplicationBIM
	{
		private readonly IProject _project;
		private readonly IProjectStorage _projectStorage;

		protected ProjectApplication(IProject project, IProjectStorage projectStorage)
		{
			_project = project;
			_projectStorage = projectStorage;

			projectStorage.Load();
		}

		public override void ActivateInBIM(List<BIMItemId> items)
		{
			using (new Scope.Scope())
			{
				List<IIdeaPersistenceToken> tokens = items
					.Where(x => x.Type != BIMItemType.BIMItemsGroup)
					.Select(x => _project.GetPersistenceToken(x.Id))
					.ToList();

				IEnumerable<IIdentifier<IIdeaNode>> nodes = tokens
					.OfType<IIdentifier<IIdeaNode>>();

				IEnumerable<IIdentifier<IIdeaMember1D>> members = tokens
					.OfType<IIdentifier<IIdeaMember1D>>();

				Select(nodes, members);
			}
		}

		protected override ModelBIM ImportActive(CountryCode countryCode, RequestedItemsType requestedType)
		{
			using (new Scope.Scope())
			{
				try
				{
					return ImportSelection(countryCode, requestedType);
				}
				finally
				{
					ImportFinished();
				}
			}
		}

		protected override List<ModelBIM> ImportSelection(CountryCode countryCode, List<BIMItemsGroup> items)
		{
			using (new Scope.Scope())
			{
				try
				{
					return Synchronize(countryCode, items);
				}
				finally
				{
					ImportFinished();
				}
			}
		}

		protected abstract void Select(IEnumerable<IIdentifier<IIdeaNode>> nodes, IEnumerable<IIdentifier<IIdeaMember1D>> members);

		protected abstract ModelBIM ImportSelection(CountryCode countryCode, RequestedItemsType requestedType);

		protected abstract List<ModelBIM> Synchronize(CountryCode countryCode, List<BIMItemsGroup> items);

		private void ImportFinished()
			=> _projectStorage.Save();
	}
}