using System;
using Extensions.Object;

namespace FC.TelegramBot
{
    class Program
    {
        static void Main( string[] args )
        {
            var context = ExObject.Instantiate<BotContext>();
            context.Load();

            Lock();
        }

        static void Lock()
        {
            while(true)
            {
                var line = Console.ReadLine();
                line = line.ToLower();

                if(line.Contains("quit")
                    || line.Contains("exit"))
                {
                    break;

                }
            }
        }
    }
}
