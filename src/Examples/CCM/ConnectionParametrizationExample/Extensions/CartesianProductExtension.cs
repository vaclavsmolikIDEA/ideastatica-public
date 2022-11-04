using System.Collections.Generic;
using System.Linq;

namespace ConnectionParametrizationExample.Extensions
{
	static class CartesianProductExtension
	{
		public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(this IEnumerable<IEnumerable<T>> sequences)
		{
			// https://stackoverflow.com/questions/19075173/getting-all-possible-combinations-of-a-list-of-keyvalue-pairs-in-c-sharp
			IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
			return sequences.Aggregate(
			  emptyProduct,
			  (accumulator, sequence) =>
				from accseq in accumulator
				from item in sequence
				select accseq.Concat(new[] { item }));
		}
	}
}
