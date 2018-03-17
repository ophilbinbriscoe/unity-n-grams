using System;
using System.Collections.Generic;
using UnityEngine;

namespace NGram.Samples
{
	[System.Serializable]
	public class CharRoot : RootBase<char>
	{
		public CharRoot ( IRoot<char> root ) : base( root ) { }
	}

	[System.Serializable]
	public class CharWindow : WindowBase<char, CharRoot>
	{
		public CharWindow ( int capacity ) : base( capacity ) { }

		public override CharRoot ToRoot ()
		{
			return new CharRoot( this );
		}
	}

	[System.Serializable]
	public class CharNGram : HashNGram<char, CharRoot>
	{
		public CharNGram ( int n, IList<char> domain, IWindow<char, CharRoot> window ) : base( n, domain, window ) { }
	}

	[CreateAssetMenu( order = 2500 )]
	public class CharModel : ModelBase<char, CharRoot, CharNGram, CharWindow, string>
	{
		private void Reset ()
		{
			domain = new List<char> { 'L', 'R' };
		}

		protected override CharNGram NGramFactory ( int n )
		{
			return new CharNGram( n, domain, windows[n - 1] );
		}

		protected override CharWindow WindowFactory ( int capacity )
		{
			return new CharWindow( capacity );
		}

		protected override char ReadTrainingSequenceAt ( int index )
		{
			return trainingSequence[index];
		}

		protected override int GetTrainingSequenceLength ()
		{
			return trainingSequence.Length;
		}
	}
}