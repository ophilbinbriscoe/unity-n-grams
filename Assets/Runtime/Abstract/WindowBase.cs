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
	[System.Serializable]
	public abstract class WindowBase<T, TRoot> : IWindow<T, TRoot> where TRoot : IRoot<T>
	{
		[SerializeField]
		[HideInInspector]
		private T[] array;

		[SerializeField]
		[HideInInspector]
		private int head;

		[SerializeField]
		[HideInInspector]
		private int foot;

		[SerializeField]
		[HideInInspector]
		private int size;

		public T this[int index]
		{
			get
			{
				if ( index < size )
				{
					return array[(foot + index) % array.Length];
				}
				else
				{
					throw new IndexOutOfRangeException();
				}
			}
		}

		public int Size
		{
			get
			{
				return size;
			}
		}

		public int Capacity
		{
			get
			{
				return array.Length;
			}
		}

		public WindowBase ( int capacity )
		{
			array = new T[capacity];
		}

		public void Pop ()
		{
			if ( size > 0 )
			{
				if ( head == 0 )
				{
					head = array.Length;
				}

				head--;
				size--;
			}
			else
			{
				throw new InvalidOperationException();
			}
		}

		public void Push ( T terminal )
		{
			if ( array.Length == 0 )
			{
				return;
			}

			array[head] = terminal;

			if ( size == array.Length )
			{
				foot++;
				head++;

				if ( foot == array.Length )
				{
					foot = 0;
				}
			}
			else
			{
				size++;
				head++;
			}

			if ( head == array.Length )
			{
				head = 0;
			}
		}

		public void Clear ()
		{
			head = 0;
			foot = 0;
			size = 0;
		}

		public abstract TRoot ToRoot ();

		public override string ToString ()
		{
			string str = string.Empty;

			for ( int i = 0; i < size; i++ )
			{
				str += this[i];
			}

			return str;
		}
	}
}