using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NGram
{
	public abstract class ModelBase<T, TRoot, TNGram, TWindow, TSequence> : ScriptableObject, IModel<T> where TRoot : IRoot<T> where TNGram : INGram<T> where TWindow : IWindow<T, TRoot>
	{
		[Header( "Parameters" )]

		[SerializeField]
		[Range( 1f, 8f )]
		[Tooltip( "N of the highest order N-gram used by this predictor." )]
		protected int n = 3;

		/// <summary>
		/// N of the highest order N-gram used by this predictor.
		/// </summary>
		public int N
		{
			get
			{
				return n;
			}
		}

		[SerializeField]
		[Tooltip( "Possible values." )]
		protected List<T> domain;

		private ReadOnlyCollection<T> domainReadOnly;

		/// <summary>
		/// Possible values.
		/// </summary>
		public ReadOnlyCollection<T> Domain
		{
			get
			{
				return domainReadOnly == null ? domainReadOnly = domain.AsReadOnly() : domainReadOnly;
			}
		}

		[Header( "Training" )]

		[SerializeField]
		[Tooltip( "Training sequence." )]
		protected TSequence trainingSequence;

		[SerializeField]
		private bool train;
		
		[Header( "Model" )]

		[SerializeField]
		protected TNGram[] nGrams;

		[SerializeField]
		[HideInInspector]
		protected TWindow[] windows;

		protected virtual void OnValidate ()
		{
			n = Mathf.Clamp( n, 1, 8 );

			if ( nGrams == null || nGrams.Length != n )
			{
				windows = new TWindow[n];
				nGrams = new TNGram[n];

				for ( int i = 0; i < n; i++ )
				{
					windows[i] = WindowFactory( i );
					nGrams[i] = NGramFactory( i + 1 );
				}
			}

			if ( train )
			{
				train = false;

				Train( ReadTrainingSequenceAt, GetTrainingSequenceLength() );
			}
		}

		/// <summary>
		/// Train this predictor on a particular sequence. Note that training should be performed offline (in Edit mode) whenever possible, as it may create a considerable amount of garbage.
		/// </summary>
		/// <param name="sequence">Training sequence.</param>
		public void Train ( System.Func<int, T> sequence, int sequenceLength )
		{
			ClearWindows();

			for ( int i = 0; i < n; i++ )
			{
				nGrams[i].Train( domain, sequence, sequenceLength, windows[i] );
			}
		}

		public RootData GetRootData ( IRoot<T> root )
		{
			if ( root.Size >= n )
			{
				throw new ArgumentException( "Root is too long for this model." );
			}

			return nGrams[root.Size].GetRootData( root );
		}

		private void ClearWindows ()
		{
			for ( int i = 0; i < windows.Length; i++ )
			{
				windows[i].Clear();
			}
		}

		protected abstract TNGram NGramFactory ( int n );
		protected abstract TWindow WindowFactory ( int capacity );
		protected abstract T ReadTrainingSequenceAt ( int index );
		protected abstract int GetTrainingSequenceLength ();
	}
}