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
	public interface IRootBuilder<T,R> where R : IList<T>
	{
		T this[int index] { get; }
		int Size { get; }
		void Push ( T terminal );
		void Pop ();
		void Reset ( int capacity = -1 );
		R ToRoot ();
	}
}