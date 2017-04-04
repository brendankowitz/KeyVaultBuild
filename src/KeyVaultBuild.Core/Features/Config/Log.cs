using System;

namespace KeyVaultBuild.Features.Config
{
    public class Log
    {
        public static Action<Exception, string> Error { get; set; } = (e, s) => Console.WriteLine(s + Environment.NewLine + e);
        public static Action<string> Information { get; set; } = s => Console.WriteLine(s);
    }
}