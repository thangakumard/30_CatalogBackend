using Serilog;


namespace SharedLibrary
{
    public class Logger
    {
        public static void LogError(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        private static void LogToFile(string message)
        {
            Log.Information(message);
        }
        private static void LogToConsole(string message)
        {
            Log.Warning(message);
        }
        private static void LogToDebugger(string message)
        {
            Log.Debug(message);
        }
    }
}
