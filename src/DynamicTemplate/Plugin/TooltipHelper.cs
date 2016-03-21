using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    class TooltipHelper
    {
        public static Point GetPointFromIndex(TextBox textbox, int index)
        {
            IntPtr result = SendMessage(textbox.Handle, EM_POSFROMCHAR, new IntPtr(index), IntPtr.Zero);
            int res = result.ToInt32();
            return new Point(res & 0xFFFF, (res >> 16) & 0xFFFF);
        }

        const uint EM_POSFROMCHAR = 0x426;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    }
}
