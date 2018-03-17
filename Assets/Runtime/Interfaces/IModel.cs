using System.Collections.ObjectModel;

namespace NGram
{
	/// <summary>
	/// A trained model.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public interface IModel<T>
	{
		int N { get; }
		ReadOnlyCollection<T> Domain { get; }
		void Train ( System.Func<int, T> sequence, int sequenceLength );
		RootData GetRootData ( IRoot<T> root );
	}
}