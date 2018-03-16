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
	public abstract class NGramBase<T, R> : INGram<T, R> where R : IList<T>
	{
		[SerializeField]
		private int n;

		public abstract int N { get; }

		public NGramBase ( int n )
		{
			this.n = n;
		}

		public abstract void Train ( IList<T> domain, IList<T> data );

		public abstract void GetPrediction ( R root );
		public abstract RootData<T> GetRootData ( R root );
		public abstract void GetSampleCount ( R root );
	}
}