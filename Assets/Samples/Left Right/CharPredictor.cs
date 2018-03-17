namespace NGram.Samples
{
	[System.Serializable]
	public class CharPrediction : Prediction<char> { }

	public class CharPredictor : PredictorBase<char, CharRoot, CharNGram, CharWindow, CharModel, CharPrediction>
	{
		protected override CharWindow WindowFactory ( int capacity )
		{
			return new CharWindow( capacity );
		}
	}
}