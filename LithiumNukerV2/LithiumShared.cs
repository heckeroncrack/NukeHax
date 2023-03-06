using System;
using System.Reflection;

namespace LithiumNukerV2
{
	// Token: 0x02000005 RID: 5
	public class LithiumShared
	{
		// Token: 0x0600000F RID: 15 RVA: 0x00002AE3 File Offset: 0x00000CE3
		public static Version GetVersion(Assembly sender = null)
		{
			if (sender == null)
			{
				sender = Assembly.GetCallingAssembly();
			}
			return sender.GetName().Version;
		}
	}
}
