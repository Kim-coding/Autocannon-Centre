using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class Logger
{
    [Conditional("ENABLE_LOGS")]
    public static void Log(string message)
    {
        Debug.Log(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(string message)
    {
        Debug.LogWarning(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(string message)
    {
        Debug.LogError(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void Log(UnityEngine.Object context, string message)
    {
        Debug.Log(message, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(UnityEngine.Object context, string message)
    {
        Debug.LogWarning(message, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(UnityEngine.Object context, string message)
    {
        Debug.LogError(message, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarning(object message)
    {
        Debug.LogWarning(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogError(object message)
    {
        Debug.LogError(message);
    }


    [Conditional("ENABLE_LOGS")]
    public static void LogException(System.Exception exception)
    {
        Debug.LogException(exception);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogFormat(string format, params object[] args)
    {
        Debug.LogFormat(format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        Debug.LogWarningFormat(format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        Debug.LogErrorFormat(format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogException(System.Exception exception, UnityEngine.Object context)
    {
        Debug.LogException(exception, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogAssertion(string message)
    {
        Debug.LogAssertion(message);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogAssertionFormat(string format, params object[] args)
    {
        Debug.LogAssertionFormat(format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogAssertionFormat(string format, UnityEngine.Object context, params object[] args)
    {
        Debug.LogAssertionFormat(format, context, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogAssertion(UnityEngine.Object context, string message)
    {
        Debug.LogAssertion(message, context);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogAssertionFormat(UnityEngine.Object context, string format, params object[] args)
    {
        Debug.LogAssertionFormat(format, context, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        Debug.LogFormat(context, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        Debug.LogWarningFormat(context, format, args);
    }

    [Conditional("ENABLE_LOGS")]
    public static void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
    {
        Debug.LogErrorFormat(context, format, args);
    }

}