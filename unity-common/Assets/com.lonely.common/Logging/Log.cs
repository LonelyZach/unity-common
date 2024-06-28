using System;

namespace com.lonely.common.Logging
{
  public static class Log
  {
    private const bool Disable = false;
    private const bool LogMainThreadOnly = true;
    private const bool LogAllErrors = true;
    private static readonly int _mainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;

    public static void Debug(object message)
    {
      LogIt(message, UnityEngine.Debug.Log, false);
    }
    
    public static void Warning(object message)
    {
      LogIt(message, UnityEngine.Debug.LogWarning, false);
    }
    
    public static void Error(object message)
    {
      LogIt(message, UnityEngine.Debug.LogError, LogAllErrors);
    }
    
#pragma warning disable CS0162 // Unreachable Code
    private static void LogIt(object message, Action<object> log, bool force)
    {
      if (Disable)
      {
        return;
      }

      if (!force && LogMainThreadOnly && System.Threading.Thread.CurrentThread.ManagedThreadId != _mainThreadId)
      {
        return;
      }
      
      log(message);
    }
#pragma warning restore CS0162
  }
}