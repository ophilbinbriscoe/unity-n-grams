namespace NGram
{
	public class Prediction<T> : IPrediction<T>
	{
		public static implicit operator T ( Prediction<T> prediction )
		{
			return prediction.value;
		}

		public IRoot<T> root;

		public IRoot<T> Root
		{
			get
			{
				return root;
			}
		}

		public T value;

		public T Value
		{
			get
			{
				return value;
			}
		}

		public  int sampleCount;

		public int SampleCount
		{
			get
			{
				return sampleCount;
			}
		}
	}
}