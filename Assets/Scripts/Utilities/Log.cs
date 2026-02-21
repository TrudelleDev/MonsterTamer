using System.Diagnostics;

namespace MonsterTamer.Utilities
{
    /// <summary>
    /// Custom logger that wraps Unity Debug logic. 
    /// </summary>
    internal static class Log
    {
        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        internal static void Info(string tag, string message)
        {
            UnityEngine.Debug.Log(Format(tag, message));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        internal static void Warning(string tag, string message)
        {
            UnityEngine.Debug.LogWarning(Format(tag, message));
        }

        [Conditional("UNITY_EDITOR")]
        [Conditional("DEVELOPMENT_BUILD")]
        internal static void Error(string tag, string message)
        {
            UnityEngine.Debug.LogError(Format(tag, message));
        }

        private static string Format(string tag, string message)
        {
            string label = string.IsNullOrWhiteSpace(tag) ? "[Unknown]" : $"[{tag}]";
            return $"{label} {message}";
        }
    }
}
