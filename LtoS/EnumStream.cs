using System;
using System.Collections.Generic;
using System.IO;

namespace LtoS
{
	public class EnumStream<T>
	{
		private readonly IEnumerator<T> _source;
		public EnumStream(IEnumerable<T> source)
		{
			_source = source.GetEnumerator();
		}
		public T Read()
		{
			if (!_source.MoveNext())
			{
				return default;
			}
			return _source.Current;
		}
	}
}
