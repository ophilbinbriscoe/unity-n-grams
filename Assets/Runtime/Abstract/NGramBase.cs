using System.Collections.Generic;
using UnityEngine;

namespace NGram
{
	public abstract class NGramBase<T> : INGram<T>
	{
		[SerializeField]
		protected int n;

		public int N
		{
			get
			{
				return n;
			}
		}

		public NGramBase ( int n )
		{
			this.n = n;
		}

		/// <summary>
		/// Train this N-gram on a particular sequence. Note that training should be performed offline (in Edit mode) whenever possible, as it may create a considerable amount of garbage.
		/// </summary>
		/// <param name="domain">Possibile values.</param>
		/// <param name="sequence">Training sequence.</param>
		/// <param name="window"><see cref="WindowBase{T, Root}"/> with Capacity of <see cref="N"/> less one.</param>
		public abstract void Train<Root> ( IList<T> domain, System.Func<int, T> sequence, int sequenceLength, IWindow<T, Root> window ) where Root : IRoot<T>;

		/// <summary>
		/// Lookup the <see cref="RootData"/> for a given <see cref="RootBase{T}"/>.
		/// </summary>
		/// <param name="root">Root of size <see cref="N"/> less one.</param>
		/// <returns><see cref="RootData"/> (sample count, prediction index).</returns>
		public abstract RootData GetRootData ( IRoot<T> root );
	}
}