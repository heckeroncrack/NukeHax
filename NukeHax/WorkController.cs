using System;
using System.Collections.Generic;
using System.Linq;

namespace LithiumNukerV2
{
	// Token: 0x0200000B RID: 11
	public class WorkController
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00002BDC File Offset: 0x00000DDC
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
