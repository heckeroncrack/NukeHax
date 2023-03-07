using System;
using System.Reflection;

namespace LithiumNukerV2
{
	// Token: 0x0200000A RID: 10
	public class LithiumShared
	{
		// Token: 0x0600001F RID: 31 RVA: 0x00002183 File Offset: 0x00000383
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
