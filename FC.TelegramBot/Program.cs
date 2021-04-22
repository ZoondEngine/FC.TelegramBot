using System;

namespace FC.TelegramBot
{
    class Program
    {
        static void Main( string[] args )
        {
            var context = Extensions.Object.ExObject.Instantiate<BotContext>();
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
