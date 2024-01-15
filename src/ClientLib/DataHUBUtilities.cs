using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Audalia.DataHUBClient
{
    public class DataHUBUtilities
    {
        public static T Synchronize<T>(Func<T> func)
        {
            var result = default(T);

            ThreadPool.QueueUserWorkItem(delegate
            {
                Application.OpenForms[0].Invoke((MethodInvoker)delegate
                {
                    result = func();
                });
            }, null);

            return result;
        }
    }
}
