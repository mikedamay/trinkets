using System;
using System.Collections.Generic;
using PureDI;
using PureDI.Attributes;

namespace ting
{
	[Bean]
	internal class TagsHandler
	{
		[BeanReference] private ILogger logger = null;
		private IDictionary<string, int> _tagSet = new Dictionary<string,int>();
		internal void RecordTags(string tagsString)
		{
			logger.Log($"tagsString has a length of {tagsString.Length}");
			string[] tags = tagsString.Split(" ");
			foreach (string tag in tags)
			{
				if (!_tagSet.ContainsKey(tag))
				{
					_tagSet.Add(new KeyValuePair<string, int>(tag, 0));
				}
				_tagSet[tag] = _tagSet[tag] + 1;
			}
		}
		internal IDictionary<string, int> GetTags()
		{
			return _tagSet;
		}
	}
}

