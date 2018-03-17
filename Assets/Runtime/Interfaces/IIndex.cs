namespace NGram
{
	/// <summary>
	/// Interface for types which support indexing.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public interface IIndex<T>
	{
		T this[int index] { get; }
	}
}