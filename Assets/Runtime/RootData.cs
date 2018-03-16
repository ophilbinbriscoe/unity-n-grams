using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Text;

namespace NGram
{
	[System.Serializable]
	public class RootData<T>
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
		private T prediction;

		public T Prediction
		{
			get
			{
				return prediction;
			}
		}

		public RootData ( int sampleCount, T prediction )
		{
			this.sampleCount = sampleCount;
			this.prediction = prediction;
		}
	}
}