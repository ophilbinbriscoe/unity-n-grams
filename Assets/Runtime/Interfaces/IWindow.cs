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
	/// <summary>
	/// A builder used to generate roots, behaving as both a ring buffer and a stack.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	/// <typeparam name="TRoot">Generated root type.</typeparam>
	public interface IWindow<T, TRoot> : IRoot<T> where TRoot : IRoot<T>
 	{
		int Capacity { get; }
		void Push ( T terminal );
		void Pop ();
		void Clear ();
		TRoot ToRoot ();
	}
}