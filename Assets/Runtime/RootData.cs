using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace NGram
{
	/// <summary>
	/// Tuple storing the domain index of the element predicted to follow a particular root, as well as the number of samples considered to arrive at that prediction.
	/// </summary>
	[System.Serializable]
	public class RootData
	{
		[SerializeField]
		private int sampleCount;

		public int SampleCount
		{
			get
			{
				return sampleCount;
			}
		}

		[SerializeField]
		private int predictionIndex;

		public int PredictionIndex
		{
			get
			{
				return predictionIndex;
			}
		}

		public RootData ( int sampleCount, int predictionIndex )
		{
			this.sampleCount = sampleCount;
			this.predictionIndex = predictionIndex;
		}
	}
}