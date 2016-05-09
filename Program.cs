using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PRDB_Visual_Management.PresentationLayer;

namespace PRDB_Visual_Management
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form_Main());
        }
    }
}
