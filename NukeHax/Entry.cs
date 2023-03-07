using System;
using System.Drawing;
using System.Net;
using Veylib.CLIUI;

namespace LithiumNukerV2
{
	// Token: 0x02000009 RID: 9
	internal class Entry
	{
		// Token: 0x0600001B RID: 27 RVA: 0x000029B4 File Offset: 0x00000BB4
		private static void parseArgs(string[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				string a = args[i].ToLower();
				if (!(a == "--debug"))
				{
					if (!(a == "--token"))
					{
						if (!(a == "--guild"))
						{
							if (!(a == "--threads"))
							{
								if (!(a == "--connection-limit"))
								{
									Entry.core.WriteLine(new object[]
									{
										Color.Red,
										"Invalid argument: " + args[i].ToLower()
									});
								}
								else
								{
									i++;
									int connectionLimit;
									if (!int.TryParse(args[i], out connectionLimit))
									{
										Entry.core.WriteLine(new object[]
										{
											Color.Red,
											"--connection-limit argument value invalid"
										});
									}
									Settings.ConnectionLimit = connectionLimit;
								}
							}
							else
							{
								i++;
								int threads;
								if (!int.TryParse(args[i], out threads))
								{
									Entry.core.WriteLine(new object[]
									{
										Color.Red,
										"--threads argument value invalid"
									});
								}
								Settings.Threads = threads;
							}
						}
						else
						{
							i++;
							long value;
							if (!long.TryParse(args[i], out value))
							{
								Entry.core.WriteLine(new object[]
								{
									Color.Red,
									"--guild argument value invalid"
								});
							}
							Settings.GuildId = new long?(value);
						}
					}
					else
					{
						i++;
						Settings.Token = args[i];
					}
				}
				else
				{
					Settings.Debug = true;
				}
			}
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B38 File Offset: 0x00000D38
		private static void Main(string[] args)
		{
			Core.StartupProperties startProperties = new Core.StartupProperties
			{
				MOTD = "Lith was discontinued, so I fucked verlox's ass. UNLEASH HELL MY NIGGERS.",
				ColorRotation = 260,
				SilentStart = true,
				LogoString = Settings.Logo,
				DebugMode = Settings.Debug,
				Author = new Core.StartupAuthorProperties
				{
					Url = "verlox.cc, russianheavy.xyz & discord.gg/WjT4ASqcra",
					Name = "verlox, vividsec & ItzYaBoiHeck#8569"
				},
				Title = new Core.StartupConsoleTitleProperties
				{
					Text = "NukeHax"
				}
			};
			Entry.core.Start(startProperties);
			Entry.parseArgs(args);
			ServicePointManager.DefaultConnectionLimit = Settings.ConnectionLimit;
			ServicePointManager.Expect100Continue = false;
			Picker.Choose();
		}

		// Token: 0x0400001F RID: 31
		public static Core core = Core.GetInstance();
	}
}
