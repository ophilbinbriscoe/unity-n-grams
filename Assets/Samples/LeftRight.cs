using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using NGram.Samples;

public class LeftRight : MonoBehaviour
{
	public CharPredictor predictor;

	public Text text;

	private void OnGUI ()
	{
		if ( Event.current.type == EventType.KeyDown )
		{
			switch ( Event.current.keyCode )
			{
			case KeyCode.LeftArrow:
				predictor.Push( 'L' );
				break;

			case KeyCode.RightArrow:
				predictor.Push( 'R' );
				break;

			default:
				return;
			}

			Print();
		}
	}

	private void Start ()
	{
		Print();
	}

	private void Print ()
	{
		var p = predictor.Prediction;

		text.text = string.Format( "Prediction: {0}...<b>{1}</b> ({2} samples)", p.Root, p.Value, p.SampleCount > -1 ? p.SampleCount.ToString() : "no" );
	}
}
