using System.Runtime.InteropServices;

string? comanda;
bool allower = true;
while (allower)
{
    Console.WriteLine("Введите команду:");
    Console.Write(">");
    comanda = Console.ReadLine();
    comanda ??= "null";
    switch (comanda.ToUpper())
    {
        case "SHINE":
        case "S":
            StateController.PreventMonitorPowerdown();
            System.Diagnostics.Process.Start("CMD.exe", "/C pause").WaitForExit();
            StateController.Continuous();
            break;

        case "AWAKE":
        case "A":
            StateController.PreventSleep();
            System.Diagnostics.Process.Start("CMD.exe", "/C pause").WaitForExit();
            StateController.Continuous();
            break ;

        case "QUIT":
        case "Q":
            allower = false;
            Console.WriteLine("Выход");
            break;
        case "HELP":
        case "H":
            Console.WriteLine("Команды:\nshine или s - запрещает отключение монитора;\nawake или a - запрещает спящий режим;\nquit или q - выход из программы.\nhelp или h - данная справка\nПри вводе всякой отсебятины программа будет требовать ввода корректной команды.");
             break ;
        
        default:
            Console.WriteLine("Неверный ввод.");
            break;
    }


}

Thread.Sleep(1500);

class StateController
{
    [FlagsAttribute]
    public enum EXECUTION_STATE : uint
    {
        /// <summary>
        /// Enables away mode. This value must be specified with ES_CONTINUOUS.
        /// Away mode should be used only by media-recording and media-distribution applications
        /// that must perform critical background processing on desktop computers
        /// while the computer appears to be sleeping.
        /// </summary>
        ES_AWAYMODE_REQUIRED = 0x00000040,
        /// <summary>
        /// Informs the system that the state being set should remain in effect until
        /// the next call that uses ES_CONTINUOUS and one of the other state flags is cleared.
        /// </summary>
        ES_CONTINUOUS = 0x80000000,
        /// <summary>
        /// Forces the display to be on by resetting the display idle timer. 
        /// </summary>
        ES_DISPLAY_REQUIRED = 0x00000002,
        /// <summary>
        /// Forces the system to be in the working state by resetting the system idle timer.
        /// </summary>
        ES_SYSTEM_REQUIRED = 0x00000001
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);

    internal static void PreventMonitorPowerdown()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_DISPLAY_REQUIRED | EXECUTION_STATE.ES_CONTINUOUS);
        Console.WriteLine("Блокировка экрана при простое отключена.");
    }

    internal static void Continuous()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS);
        Console.WriteLine("Режим питания приведен в изначальное состояние.");
    }
    internal static void AwayMode()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_AWAYMODE_REQUIRED);
    }

    internal static void PreventSleep()
    {
        SetThreadExecutionState(EXECUTION_STATE.ES_CONTINUOUS | EXECUTION_STATE.ES_SYSTEM_REQUIRED);
        Console.WriteLine("Переход в спящий режим при простое отключен.");
    }
    
}
