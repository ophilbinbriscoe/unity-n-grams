using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGram
{
	public static class NGramUtility
	{
		/// <summary>
		/// Compute the number of roots of a given size in a domain.
		/// </summary>
		/// <param name="domain">Possible values.</param>
		/// <param name="size">Root size.</param>
		/// <returns>Number of roots.</returns>
		public static int NumRootsInDomain<T> ( IList<T> domain, int size )
		{
			var r = domain.Count;

			if ( size > 1 )
			{
				for ( int i = 1; i < size; i++ )
				{
					r *= domain.Count;
				}
			}
			else if ( size == 0 )
			{
				r = 1;
			}

			return r;
		}

		public static void BuildRoots<T, TRoot> ( int n, IList<T> domain, IWindow<T, TRoot> window, TRoot[] roots ) where TRoot : IRoot<T>
		{
			var index = 0;
			var stack = n - 1;

			BuildRoots( domain, window, roots, ref index, stack );
		}

		private static void BuildRoots<T, TRoot> ( IList<T> domain, IWindow<T, TRoot> window, TRoot[] roots, ref int index, int stack ) where TRoot : IRoot<T>
		{
			if ( stack == 0 )
			{
				roots[index++] = window.ToRoot();

				return;
			}

			stack--;

			for ( int j = 0; j < domain.Count; j++ )
			{
				window.Push( domain[j] );

				BuildRoots( domain, window, roots, ref index, stack );

				window.Pop();
			}
		}
	}
}