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
	public interface INGram<T,R> where R : IList<T>
	{
		int N { get; }
		void Train ( IList<T> domain, IList<T> data );
		RootData<T> GetRootData ( R root );
		void GetPrediction ( R root );
		void GetSampleCount ( R root );
	}
}