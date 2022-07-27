using System.Collections.Concurrent;

namespace IdeaStatica.BimApiLink.Scoping
{
	public class BimLinkScope : IScope, IDisposable
	{
		private static readonly AsyncLocal<BimLinkScope?> _current = new();
		internal static BimLinkScope Current => _current.Value ?? throw new InvalidOperationException();

		private readonly ConcurrentDictionary<string, object> _data = new();

		public BimLinkScope()
		{
			_current.Value = this;
		}

		public void Dispose()
			=> _current.Value = null;

		public T Get<T>(string key)
		{
			if (_data.TryGetValue(key, out object? res))
			{
				return (T)res;
			}

			throw new InvalidOperationException();
		}

		public void Set<T>(string key, T value)
		{
			if (value is null)
			{
				throw new ArgumentNullException();
			}

			_data[key] = value;
		}
	}
}