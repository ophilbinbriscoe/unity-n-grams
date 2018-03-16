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
	public abstract class RootBuilder<T,R> : IRootBuilder<T,R> where R : IList<T>
	{
		[SerializeField]
		private T[] array;

		[SerializeField]
		private int foot;

		[SerializeField]
		private int head;

		[SerializeField]
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

		public RootBuilder ( int capacity )
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
			if ( size == array.Length )
			{
				foot++;
				head++;

				if ( foot == array.Length )
				{
					foot = 0;
				}

				array[head] = terminal;
			}
			else
			{
				size++;
				head++;

				array[head] = terminal;
			}

			if ( head == array.Length )
			{
				head = 0;
			}
		}

		public void Reset ( int capacity = -1 )
		{
			if ( array.Length != capacity )
			{
				array = new T[capacity];
			}

			head = 0;
			foot = 0;
		}

		public abstract R ToRoot ();
	}
}