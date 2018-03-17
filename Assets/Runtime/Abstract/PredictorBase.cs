using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace NGram
{
	public abstract class PredictorBase<T, TRoot, TNGram, TWindow, TModel, TPrediction> : MonoBehaviour where TRoot : IRoot<T> where TNGram : INGram<T> where TWindow : IWindow<T, TRoot> where TModel : IModel<T> where TPrediction : Prediction<T>
	{
		[SerializeField]
		[Tooltip( "Model used to generate predictions." )]
		protected TModel model;

		/// <summary>
		/// Model used to generate predictions.
		/// </summary>
		public TModel Model
		{
			get
			{
				return model;
			}
		}

		[SerializeField]
		[Tooltip( "Sample count under which predictions will be random." )]
		protected int minSamples = 5;

		/// <summary>
		/// Sample count under which predictions will be random.
		/// </summary>
		public int MinSamples
		{
			get
			{
				return minSamples;
			}
		}

		[SerializeField]
		[HideInInspector]
		protected TWindow[] windows;

		[SerializeField]
		[HideInInspector]
		private TPrediction prediction;

		[SerializeField]
		[HideInInspector]
		private bool initialized;

		/// <summary>
		/// Predicted value (based on input from <see cref="Push(T)"/>).
		/// </summary>
		public IPrediction<T> Prediction
		{
			get
			{
				return prediction;
			}
		}

		private void OnValidate ()
		{
			if ( model != null )
			{
				if ( windows == null || windows.Length != model.N )
				{
					initialized = false;

					windows = new TWindow[model.N];

					for ( int i = 0; i < model.N; i++ )
					{
						windows[i] = WindowFactory( i );
					}
				}
			}
			else
			{
				initialized = false;

				RandomizePredictionIndex();

				if ( windows != null && windows.Length > 0 )
				{
					windows = new TWindow[0];
				}
			}
		}

		private void Awake ()
		{
			ClearWindows();
			RandomizePredictionIndex();
		}

		/// <summary>
		/// Clear pushed values. This will not undo any training that has taken place, but may modify the return value of <see cref="Prediction"/>.
		/// </summary>
		public void Clear ()
		{
			if ( initialized )
			{
				initialized = false;

				ClearWindows();
				RandomizePredictionIndex();
			}
		}

		private void ClearWindows ()
		{
			for ( int i = 0; i < windows.Length; i++ )
			{
				windows[i].Clear();
			}
		}

		/// <summary>
		/// Push a new value onto the predictor. This may modify the return value of <see cref="Prediction"/>.
		/// </summary>
		/// <param name="value">Value.</param>
		public void Push ( T value )
		{
			initialized = true;

			for ( int i = model.N - 1; i >= 0; i-- )
			{
				var builder = windows[i];

				builder.Push( value );

				if ( builder.Size == builder.Capacity )
				{
					var rootData = model.GetRootData( builder );

					if ( rootData.SampleCount >= minSamples )
					{
						prediction.root = builder;
						prediction.sampleCount = rootData.SampleCount;
						prediction.value = model.Domain[rootData.PredictionIndex];

						return;
					}
				}
			}

			RandomizePredictionIndex();
		}

		private void RandomizePredictionIndex ()
		{
			prediction.root = null;
			prediction.sampleCount = -1;
			prediction.value = model == null || model.Domain == null || model.Domain.Count == 0 ? default( T ) : model.Domain[Random.Range( 0, model.Domain.Count )];
		}

		protected abstract TWindow WindowFactory ( int capacity );
	}
}