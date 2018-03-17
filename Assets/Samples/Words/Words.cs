using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using NGram.Samples;
using System.Text;

public class Words : MonoBehaviour
{
	public int minLength = 5;
	public int maxLength = 10;

	public StringPredictor predictor;

	public Text text;

	private void Start ()
	{
		Generate();
	}

	private void Update ()
	{
		if ( Input.GetKeyDown( KeyCode.Return ) )
		{
			Generate();
		}
	}

	public void Generate ()
	{
		predictor.Clear();

		var builder = new StringBuilder();

		var length = Random.Range( minLength, maxLength + 1 );

		for ( int i = 0; i < length; i++ )
		{
			var word = predictor.Prediction.Value;

			predictor.Push( word );

			if ( builder.Length == 0 || char.IsPunctuation( builder[builder.Length - 2] ) && builder[builder.Length - 2] != ',' )
			{
				word = char.ToUpper( word[0] ) + word.Substring( 1, word.Length - 1 );
			}

			builder.Append( word );

			if ( i < length - 1 )
			{
				builder.Append( ' ' );
			}
		}

		if ( char.IsPunctuation( builder[builder.Length - 1] ) )
		{
			builder[builder.Length - 1] = '.';
		}
		else
		{
			builder.Append( '.' );
		}

		text.text = builder.ToString();
	}
}
