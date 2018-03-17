using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace NGram
{
	[System.Serializable]
	public struct HashedNGramPair
	{
		public int key;
		public RootData value;
	}

	/// <summary>
	/// Implements <see cref="INGram{T}"/>, using a <see cref="Dictionary{IRoot{T}, {RootData}}"/> to lookup <see cref="RootData"/> for a given root, once trained.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	/// <typeparam name="TRoot">Root type.</typeparam>
	[System.Serializable]
	public abstract class HashNGram<T, TRoot> : NGramBase<T>, IEqualityComparer<IRoot<T>>, ISerializationCallbackReceiver where TRoot : IRoot<T>
	{
		[SerializeField]
		//[HideInInspector]
		private TRoot[] roots;

		private Dictionary<IRoot<T>, RootData> dictionary;

		public HashNGram ( int n, IList<T> domain, IWindow<T, TRoot> window ) : base( n )
		{
			var numRoots = NGramUtility.NumRootsInDomain( domain, n - 1 );

			roots = new TRoot[numRoots];
			dictionary = new Dictionary<IRoot<T>, RootData>( numRoots, this );

			NGramUtility.BuildRoots( n, domain, window, roots );
		}
		
		public override void Train<R> ( IList<T> domain, System.Func<int,T> sequence, int sequenceLength, IWindow<T, R> window )
		{
#if DEBUG
			if ( window.Capacity != n - 1 )
			{
				throw new ArgumentException( "Builder Capacity must be N less one." );
			}

			if ( window.Size > 0 )
			{
				throw new ArgumentException( "Builder must be in the initial state." );
			}

			if ( sequenceLength < n )
			{
				throw new ArgumentException( "Insufficient training sequence." );
			}
#endif

			var m = n - 1;
			var k = m * m;

			var trainingData = new Dictionary<IRoot<T>, TrainingData<T>>( k, this );

			for ( int i = 0; i < roots.Length; i++ )
			{
				trainingData.Add( roots[i], new TrainingData<T>( domain ) );
			}

			for ( int i = 0; i < m; i++ )
			{
				window.Push( sequence( i ) );
			}

			var stop = sequenceLength - n;

			for ( int i = m; i < stop; i++ )
			{
				window.Push( sequence( i ) );

				TrainingData<T> td = trainingData[window];

				td.sampleCount++;
				td.terminalCount[sequence( i + 1 )]++;

				trainingData[window] = td;
			}

			foreach ( var root in roots )
			{
				var td = trainingData[root];

				var maxCount = -1;

				var predictionIndex = -1;

				for ( int i = 0; i < domain.Count; i++ )
				{
					var count = td.terminalCount[domain[i]];

					if ( count > maxCount )
					{
						maxCount = count;
						predictionIndex = i;
					}
				}

				dictionary[root] = new RootData( td.sampleCount, predictionIndex );
			}
		}

		public override RootData GetRootData ( IRoot<T> root )
		{
			return dictionary[root];
		}

		bool IEqualityComparer<IRoot<T>>.Equals ( IRoot<T> x, IRoot<T> y )
		{
			if ( x.Size != y.Size )
			{
				return false;
			}

			for ( int i = 0; i < x.Size; i++ )
			{
				if ( !x[i].Equals( y[i] ) )
				{
					return false;
				}
			}

			return true;
		}

		int IEqualityComparer<IRoot<T>>.GetHashCode ( IRoot<T> obj )
		{
			if ( obj.Size == 0 )
			{
				return 0;
			}

			int hc = obj[0].GetHashCode();

			for ( int i = 1; i < obj.Size; i++ )
			{
				hc <<= 1;
				hc ^= obj[i].GetHashCode();
			}

			return hc;
		}

		[SerializeField]
		private HashedNGramPair[] pairs;

		void ISerializationCallbackReceiver.OnBeforeSerialize ()
		{
			var keys = dictionary.Keys.ToArray();
			var values = dictionary.Values.ToArray();

			pairs = new HashedNGramPair[keys.Length];

			for ( int i = 0; i < keys.Length; i++ )
			{
				HashedNGramPair pair;

				pair.key = Array.IndexOf( roots, (TRoot) keys[i] );
				pair.value = values[i];

				pairs[i] = pair;
			}
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			if ( pairs != null )
			{
				dictionary = new Dictionary<IRoot<T>, RootData>( pairs.Length, this );

				for ( int i = 0; i < pairs.Length; i++ )
				{
					dictionary.Add( roots[pairs[i].key], pairs[i].value );
				}
			}
		}
	}
}