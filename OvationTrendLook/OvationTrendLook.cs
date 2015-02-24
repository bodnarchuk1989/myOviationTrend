using System;
using System.Windows.Forms;

namespace OvationTrendLook
{
	static public class OvationTrendLook
	{
		[STAThread]
		static void Main()
		{

			Application.EnableVisualStyles ();
			Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MDIMainForm ());
		}
	}
}

