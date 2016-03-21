using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    class WaitCursor : IDisposable
    {
        public WaitCursor()
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        public void Dispose()
        {
            Cursor.Current = Cursors.Default;
        }
    }
}
