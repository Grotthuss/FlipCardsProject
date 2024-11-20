namespace FlipCardProject.logs;

using System;
using System.IO;
public static class ErrorLogger
{
    private static readonly string logFilePath = "errorLog.txt"; 

    public static void LogError(Exception ex)
    {
        try
        {
            
            using (var writer = new StreamWriter(logFilePath, append: true))
            {
                writer.WriteLine("=====================================");
                writer.WriteLine($"Date/Time: {DateTime.Now}");
                writer.WriteLine($"Exception Type: {ex.GetType().FullName}");
                writer.WriteLine($"Message: {ex.Message}");
                writer.WriteLine($"Stack Trace: {ex.StackTrace}");
                writer.WriteLine("-------------------------------------");
                writer.WriteLine();
            }
        }
        catch (Exception logEx)
        {
            
            Console.WriteLine($"Error logging failed: {logEx.Message}");
        }
    }
}
    
    
