using System;
using System.Collections.Generic;
using System.Linq;

namespace LithiumNukerV2
{
	// Token: 0x02000006 RID: 6
	public class WorkController
	{
		// Token: 0x06000011 RID: 17 RVA: 0x00002B00 File Offset: 0x00000D00
		public static List<List<T>> Seperate<T>(List<T> items, int loadCount)
		{
			List<List<T>> list = new List<List<T>>();
			for (int i = 0; i < loadCount; i++)
			{
				list.Add(new List<T>());
			}
			for (int j = 0; j < items.Count; j++)
			{
				list[j % loadCount].Add(items[j]);
			}
			return (from load in list
			where load.Count > 0
			select load).ToList<List<T>>();
		}
	}
}
