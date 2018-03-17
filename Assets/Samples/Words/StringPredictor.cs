namespace NGram.Samples
{
	[System.Serializable]
	public class StringPrediction : Prediction<string> { }

	public class StringPredictor : PredictorBase<string, StringRoot, StringNGram, StringWindow, StringModel, StringPrediction>
	{
		protected override StringWindow WindowFactory ( int capacity )
		{
			return new StringWindow( capacity );
		}
	}
}