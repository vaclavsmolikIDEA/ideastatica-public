namespace IdeaStatica.BimApiLink.Scoping
{
	public interface IScope
	{
		T Get<T>(string key);

		void Set<T>(string key, T value);
	}
}