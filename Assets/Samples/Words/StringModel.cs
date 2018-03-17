using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NGram.Samples
{
	[System.Serializable]
	public class StringRoot : RootBase<string>
	{
		public StringRoot ( IRoot<string> root ) : base( root ) { }

		public override string ToString ()
		{
			var builder = new StringBuilder();

			builder.Append( '(' );

			for ( int i = 0; i < Size; i++ )
			{
				builder.Append( this[i] );
				builder.Append( ' ' );
			}

			builder.Remove( builder.Length - 1, 1 );
			builder.Append( ')' );

			return builder.ToString();
		}
	}

	[System.Serializable]
	public class StringWindow : WindowBase<string, StringRoot>
	{
		public StringWindow ( int capacity ) : base( capacity ) { }

		public override StringRoot ToRoot ()
		{
			return new StringRoot( this );
		}

		public override string ToString ()
		{
			var builder = new StringBuilder();

			builder.Append( '(' );

			for ( int i = 0; i < Size; i++ )
			{
				builder.Append( this[i] );
				builder.Append( ' ' );
			}

			builder.Remove( builder.Length - 1, 1 );
			builder.Append( ')' );

			return builder.ToString();
		}
	}

	[System.Serializable]
	public class StringNGram : HashNGram<string, StringRoot>
	{
		public StringNGram ( int n, IList<string> domain, IWindow<string, StringRoot> window ) : base( n, domain, window ) { }
	}

	[CreateAssetMenu( order = 2500 )]
	public class StringModel : ModelBase<string, StringRoot, StringNGram, StringWindow, TextAsset>
	{
		[SerializeField]
		[HideInInspector]
		private string[] tokens;

		[SerializeField]
		[HideInInspector]
		private TextAsset tokenizedAsset;

		protected override void OnValidate ()
		{
			if ( tokenizedAsset != trainingSequence )
			{
				if ( trainingSequence != null )
				{
					tokens = trainingSequence.text.Split( new char[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries );

					for ( int i = 0; i < tokens.Length; i++ )
					{
						tokens[i] = tokens[i].ToLower();
					}

					domain = new HashSet<string>( tokens ).ToList();
				}
				else
				{
					domain = new List<string>();
					tokens = new string[0];
				}

				nGrams = new StringNGram[0];
				windows = new StringWindow[0];

				tokenizedAsset = trainingSequence;
			}

			base.OnValidate();
		}

		private void Reset ()
		{
			domain = new List<string> { "ABC", "123" };
		}

		protected override StringNGram NGramFactory ( int n )
		{
			return new StringNGram( n, domain, windows[n - 1] );
		}

		protected override StringWindow WindowFactory ( int capacity )
		{
			return new StringWindow( capacity );
		}

		protected override string ReadTrainingSequenceAt ( int index )
		{
			return tokens[index];
		}

		protected override int GetTrainingSequenceLength ()
		{
			return tokens.Length;
		}
	}
}