using UnityEngine;

namespace NGram
{
	[System.Serializable]
	public abstract class RootBase<T> : IRoot<T>
	{
		[SerializeField]
		//[HideInInspector]
		private T[] array;

		public T this[int index]
		{
			get
			{
				return array[index];
			}
		}

		public int Size
		{
			get
			{
				return array.Length;
			}
		}

		public RootBase ( IRoot<T> root )
		{
			array = new T[root.Size];

			for ( int i = 0; i < root.Size; i++ )
			{
				array[i] = root[i];
			}
		}

		public override string ToString ()
		{
			string str = string.Empty;

			for ( int i = 0; i < array.Length; i++ )
			{
				str += array[i];
			}

			return str;
		}
	}
}