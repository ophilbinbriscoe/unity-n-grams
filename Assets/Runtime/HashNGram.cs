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
	public abstract class HashNGram<T, R> : NGramBase<T, R>, IEqualityComparer<R>, ISerializationCallbackReceiver where R : IList<T>
	{
		private struct TrainingData
		{
			public Dictionary<T, int> terminalCount;
			public int sampleCount;
			public T prediction;

			public TrainingData ( IList<T> domain )
			{
				this.sampleCount = 0;
				this.prediction = default( T );

				terminalCount = new Dictionary<T, int>( domain.Count );

				for ( int i = 0; i < domain.Count; i++ )
				{
					terminalCount.Add( domain[i], 0 );
				}
			}
		}

		[SerializeField]
		[HideInInspector]
		private List<R> roots;

		private Dictionary<R, RootData<T>> dictionary;

		public HashNGram ( int n, IList<T> domain, IRootBuilder<T,R> builder ) : base ( n )
		{
			var m = n - 1;
			var k = m * m;

			roots = new List<R>( k );
			dictionary = new Dictionary<R, RootData<T>>( k );

			BuildRoots( domain, m, roots, builder );
		}

		private void BuildRoots ( IList<T> domain, int k, IList<R> roots, IRootBuilder<T, R> builder )
		{
			if ( k == 0 )
			{
				roots.Add( builder.ToRoot() );

				return;
			}

			k--;

			for ( int i = 0; i < domain.Count; i++ )
			{
				builder.Push( domain[i] );

				BuildRoots( domain, k, roots, builder );

				builder.Pop();
			}
		}

		public void Train ( IList<T> domain, IList<T> data, IRootBuilder<T,R> builder )
		{
			var m = N - 1;
			var k = m * m;

			var trainingData = new Dictionary<R, TrainingData>( k, this );

			for ( int i = 0; i < roots.Count; i++ )
			{
				trainingData.Add( roots[i], new TrainingData( domain ) );
			}

			builder.Reset( m );

			for ( int i = 0; i < m; i++ )
			{
				builder.Push( data[i] );
			}

			var stop = data.Count - m;

			for ( int i = m; i < stop; i++ )
			{
				var terminal = data[i];

				builder.Push( terminal );

				var root = builder.ToRoot();

				TrainingData td = trainingData[root];

				td.sampleCount++;
				td.terminalCount[terminal]++;

				trainingData[root] = td;
			}

			foreach ( var root in roots )
			{
				var td = trainingData[root];

				var maxCount = -1;

				T prediction = default( T );

				for ( int i = 0; i < domain.Count; i++ )
				{
					var c = domain[i];
					var count = td.terminalCount[c];

					if ( count > maxCount )
					{
						maxCount = count;
						prediction = c;
					}
				}

				dictionary.Add( root, new RootData<T>( td.sampleCount, prediction ) );
			}
		}

		public virtual bool Equals ( R x, R y )
		{
			if ( x.Count != y.Count )
			{
				return false;
			}

			for ( int i = 0; i < x.Count; i++ )
			{
				if ( !x[i].Equals( y[i] ) )
				{
					return false;
				}
			}

			return true;
		}

		public virtual int GetHashCode ( R obj )
		{
			int hc = obj[0].GetHashCode();

			for ( int i = 1; i < obj.Count; i++ )
			{
				hc ^= obj[i].GetHashCode();
			}

			return hc;
		}

		[SerializeField]
		[HideInInspector]
		private R[] keys;

		[SerializeField]
		[HideInInspector]
		private RootData<T>[] values;

		void ISerializationCallbackReceiver.OnBeforeSerialize ()
		{
			keys = dictionary.Keys.ToArray();
			values = dictionary.Values.ToArray();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			if ( keys != null && values != null )
			{
				dictionary = new Dictionary<R, RootData<T>>();

				for ( int i = 0; i < keys.Length; i++ )
				{
					dictionary.Add( keys[i], values[i] );
				}
			}
		}
	}
}