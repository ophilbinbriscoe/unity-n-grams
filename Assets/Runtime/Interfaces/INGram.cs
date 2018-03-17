using System.Collections.Generic;

namespace NGram
{
	/// <summary>
	/// An N-gram can be trained to learn the probability of a given element occuring after a given root.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public interface INGram<T>
	{
		int N { get; }
		void Train ( IList<T> domain, System.Func<int, T> sequence, int sequenceLength, IWindow<T> builder );
		RootData GetRootData ( IRoot<T> root );
	}
}