using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Not_Defteri
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			// Ana formu belirtmeden uygulamayı başlat
			NotDefteri anaForm = new NotDefteri();
			anaForm.Show();
			Application.Run();
		}
	}
}
