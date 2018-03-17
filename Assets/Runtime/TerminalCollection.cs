using System.Collections.Generic;

namespace NGram
{
	/// <summary>
	/// A collection mapping terminal elements to a counter.
	/// </summary>
	/// <typeparam name="T">Element type.</typeparam>
	public class TerminalCollection<T> : Dictionary<T, int>
	{
		public TerminalCollection ( IList<T> domain ) : base( domain.Count )
		{
			for ( int i = 0; i < domain.Count; i++ )
			{
				Add( domain[i], 0 );
			}
		}
	}
}