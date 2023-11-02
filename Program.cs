using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;

class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    public static extern void mouse_event(uint dwFlags, uint dx, uint dy, int cButtons, uint dwExtraInfo);

    private const int MOUSEEVENTF_LEFTDOWN = 0x02;
    private const int MOUSEEVENTF_LEFTUP = 0x04;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Auto-clicker is ready. Press 'A' to start/stop.");

        bool autoClickerActive = false;
        object lockObject = new object();
        InputSimulator inputSimulator = new InputSimulator();

        inputSimulator.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LWIN);
        inputSimulator.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.VK_D);
        inputSimulator.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LWIN);

        Task.Run(() =>
        {
            while (true)
            {
                var key = inputSimulator.InputDeviceState.IsKeyDown(WindowsInput.Native.VirtualKeyCode.VK_A);

                if (key)
                {
                    lock (lockObject)
                    {
                        autoClickerActive = !autoClickerActive;
                        Console.WriteLine("Auto-clicker is " + (autoClickerActive ? "ON" : "OFF"));
                    }
                }

                Thread.Sleep(100);
            }
        });

        while (true)
        {
            if (autoClickerActive)
            {
                // Simulate a left mouse click
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Thread.Sleep(100); // Adjust this delay for the click rate
            }
            else
            {
                Thread.Sleep(100);
            }
        }
    }
}
