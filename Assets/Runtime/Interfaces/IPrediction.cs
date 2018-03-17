namespace NGram
{
	/// <summary>
	/// A prediction.
	/// </summary>
	public interface IPrediction<T>
	{
		IRoot<T> Root { get; }
		T Value { get; }
		int SampleCount { get; }
	}
}