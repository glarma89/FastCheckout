using System.Runtime.InteropServices;

namespace FastCheckout
{
    /// <summary>
    /// Installs a global low-level keyboard hook and raises <see cref="KeyPressed"/>
    /// on each key-down. See the interview brief (dotnet-fastCO-tasks.html) for the
    /// task description and gotchas.
    /// </summary>
    public class GlobalKeyboardHook : IDisposable
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_SYSKEYDOWN = 0x0104;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(
            int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(
            IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string? lpModuleName);

        private IntPtr hookHandle = IntPtr.Zero;
        private LowLevelKeyboardProc? proc;

        /// <summary>Fired on key-down. The hook does NOT suppress the key.</summary>
        public event Action<Keys>? KeyPressed;

        public void Install()
        {
            if (hookHandle != IntPtr.Zero)
                return;

            proc = (nCode, wParam, lParam) =>
            {
                if (nCode >= 0 &&
                    (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    int virtualKeyCode = Marshal.ReadInt32(lParam);
                    KeyPressed?.Invoke((Keys)virtualKeyCode);
                }

                return CallNextHookEx(hookHandle, nCode, wParam, lParam);
            };

            using var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            using var currentModule = currentProcess.MainModule;

            hookHandle = SetWindowsHookEx(
                WH_KEYBOARD_LL,
                proc,
                GetModuleHandle(currentModule?.ModuleName),
                0);

            if (hookHandle == IntPtr.Zero)
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }

        public void Dispose()
        {
            if (hookHandle == IntPtr.Zero)
                return;

            if (!UnhookWindowsHookEx(hookHandle))
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

            hookHandle = IntPtr.Zero;
            proc = null;
        }
    }
}
