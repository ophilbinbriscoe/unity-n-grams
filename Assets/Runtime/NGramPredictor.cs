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
	public class NGramPredictor<T, R> : MonoBehaviour where R : IList<T>
	{
		[Header( "Parameters" )]

		[Tooltip( "N of the highest order N-gram predictor." )]
		public int n = 3;

		[Tooltip( "Sample count under which the fallback output will be used." )]
		public int minSamples = 5;

		[Header( "Input" )]

		public string domain;

		public string trainingData;

		public string inputData;

		public List<NGram> hierarchy;

		[Header( "Output" )]

		public char prediction;

		private string Clean ( string data )
		{
			data = data.Replace( " ", string.Empty );

			for ( int i = 0; i < data.Length; i++ )
			{
				if ( !domain.Contains( data[i] ) )
				{
					data = (i > 0 ? data.Substring( 0, i ) : string.Empty) + (i < data.Length ? data.Substring( i + 1, data.Length - i - 1 ) : string.Empty);
				}
			}

			return data;
		}

		private void OnValidate ()
		{
			if ( !string.IsNullOrEmpty( domain ) )
			{
				domain = domain.Replace( " ", string.Empty );
			}

			if ( !string.IsNullOrEmpty( trainingData ) )
			{
				trainingData = Clean( trainingData );
			}

			if ( !string.IsNullOrEmpty( inputData ) )
			{
				inputData = Clean( inputData );
			}

			if ( string.IsNullOrEmpty( domain ) || string.IsNullOrEmpty( trainingData ) || string.IsNullOrEmpty( inputData ) )
			{
				return;
			}

			hierarchy = new List<NGram>( n );

			for ( int i = 1; i <= n; i++ )
			{
				var ngram = new NGram( domain, i );

				ngram.Train( domain, trainingData );

				hierarchy.Add( ngram );
			}

			prediction = Predict( inputData );
		}

		public char Predict ( string inputData )
		{
			string root;

			for ( int i = n - 1; i >= 0; i-- )
			{
				root = string.Empty;

				for ( int j = i; j >= 1; j-- )
				{
					root += inputData[inputData.Length - j];
				}

				var lookup = hierarchy[i].Lookup( root );

				if ( lookup.samples >= minSamples )
				{
					var terminals = string.Empty;

					for ( int j = 0; j < domain.Length; j++ )
					{
						terminals += string.Format( "{0}:{1}/{2}, ", domain[j], lookup.terminals( domain[j] ), lookup.samples );
					}

					Debug.Log( string.Format( "{0}-gram samples = {1} (>={2}), {3}...{4} ({5})", i + 1, lookup.samples, minSamples, root, prediction, terminals.TrimEnd( ' ', ',' ) ) );

					return lookup.prediction;
				}
				else
				{
					Debug.Log( string.Format( "{0}-gram samples = {1} (<{2})", i + 1, lookup.samples, minSamples ) );
				}
			}

			Debug.Log( "fallback (random prediction)" );

			return domain[Random.Range( 0, domain.Length )];
		}

		public abstract int GetIndex ( T terminal );
	}
}