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
	public class TerminalCollection<T>
	{
		private Dictionary<T, int> dictionary;

		public TerminalCollection ( IList<T> domain )
		{
			dictionary = new Dictionary<T, int>( domain.Count );

			for ( int i = 0; i < domain.Count; i++ )
			{
				dictionary.Add( domain[i], 0 );
			}
		}

		public void Increment ( T terminal )
		{
			dictionary[terminal]++;
		}
	}
}