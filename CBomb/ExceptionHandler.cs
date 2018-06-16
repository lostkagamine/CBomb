using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBomb
{
    namespace ExceptionHandler
    {
        class Crasher
        {
            public void Crash(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "don't you love it when your virus crashes", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                Environment.Exit(1);
            }
        }
    }
}
