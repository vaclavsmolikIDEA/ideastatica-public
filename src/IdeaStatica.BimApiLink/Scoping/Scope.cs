using System.Collections.Concurrent;

namespace IdeaStatica.BimApiLink.Scope
{
	internal class Scope : IDisposable
	{
		private static readonly AsyncLocal<Scope?> _current = new();

		public static Scope Current
		{
			get
			{
				return _current.Value ?? throw new InvalidOperationException();
			}
		}

		private readonly ConcurrentDictionary<string, object> _data = new();

		public Scope()
		{
			_current.Value = this;
		}

		public void Dispose() => _current.Value = null;

		public T GetOrAdd<T>(string key, Func<T> factory)
		{
			if (factory is null)
			{
				throw new ArgumentNullException();
			}

			return (T)_data.GetOrAdd(key, _ =>
			{
				T res = factory();

				if (res == null)
				{
					throw new InvalidOperationException();
				}

				return res;
			});
		}

		public T Get<T>(string key)
		{
			if (_data.TryGetValue(key, out var res))
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