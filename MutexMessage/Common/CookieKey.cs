using System;

namespace MutexMessage.Common
{
    /// <summary>
    /// Cookie-指定的一些Cookie键
    /// </summary>
    public class CookieKey
    {
        public const string SignalRId = "srid";
        public const string DefaultSignalRChannel = "BackSignalR";
        public static Func<string> DefaultAppName;

        static CookieKey()
        {
            DefaultAppName = () => { return DefaultSignalRChannel; };
        }
    }
}
