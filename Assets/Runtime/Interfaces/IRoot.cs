namespace NGram
{
	/// <summary>
	/// A string of elements.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public interface IRoot<T> : IIndex<T>, ISize { }
}