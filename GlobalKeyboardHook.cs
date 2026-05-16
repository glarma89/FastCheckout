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
            // TODO — see the interview brief (dotnet-fastCO-tasks.html).
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            // TODO — see the interview brief (dotnet-fastCO-tasks.html).
            throw new NotImplementedException();
        }
    }
}
