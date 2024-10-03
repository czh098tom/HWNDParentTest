using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;

using static HWNDParentTest.Win32WindowHelper;

namespace HWNDParentTest
{
    public class WindowsHost : HwndHost
    {
        private readonly string programName;
        private readonly string hwndArgFormat;
        private readonly string arguments;
        private Process? process = null;
        private IntPtr HWND = IntPtr.Zero;

        public WindowsHost(string programName, string hwndArgFormat = "-parentHWND {0}", string arguments = "")
        {
            this.programName = programName;
            this.hwndArgFormat = hwndArgFormat;
            this.arguments = arguments;
        }

        protected override HandleRef BuildWindowCore(HandleRef hwndParent)
        {
            Debug.WriteLine("Going to launch at: " + this.programName + " " + this.arguments);
            process = new Process();
            process.StartInfo.FileName = programName;
            process.StartInfo.Arguments = arguments + (arguments.Length == 0 ? "" : " ") + string.Format(hwndArgFormat, hwndParent.Handle);
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;

            process.Start();
            process.WaitForInputIdle();

            int repeat = 50;
            while (HWND == IntPtr.Zero && repeat-- > 0)
            {
                Thread.Sleep(100);
                EnumChildWindows(hwndParent.Handle, WindowEnum, IntPtr.Zero);
            }
            if (HWND == IntPtr.Zero)
                throw new Exception("Unable to find window");
            Debug.WriteLine("Found window: " + HWND);

            repeat += 150;
            while ((GetWindowLongPtr(HWND, GWLP_USERDATA).ToInt32() & 1) == 0 && --repeat > 0)
            {
                Thread.Sleep(100);
                Debug.WriteLine("Waiting for to initialize... " + repeat);
            }
            if (repeat == 0)
            {
                Debug.WriteLine("Timed out while waiting for to initialize");
            }
            else
            {
                Debug.WriteLine("initialized!");
            }

            return new HandleRef(this, HWND);
        }

        private int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            if (HWND != IntPtr.Zero)
                throw new Exception("Found multiple windows");
            HWND = hwnd;
            return 0;
        }

        protected override void DestroyWindowCore(HandleRef hwnd)
        {
            Debug.WriteLine("Asking to exit...");
            PostMessage(HWND, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);

            int i = 30;
            while (!(process?.HasExited ?? true))
            {
                if (--i < 0)
                {
                    Debug.WriteLine("Process not dead yet, killing...");
                    process.Kill();
                }
                Thread.Sleep(100);
            }
        }
    }
}
