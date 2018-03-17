using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace NGram
{
	/// <summary>
	/// Helper struct used for training N-grams.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public struct TrainingData<T>
	{
		public TerminalCollection<T> terminalCount;
		public int sampleCount;
		public T prediction;

		public TrainingData ( IList<T> domain )
		{
			this.sampleCount = 0;
			this.prediction = default( T );

			terminalCount = new TerminalCollection<T>( domain );
		}
	}
}