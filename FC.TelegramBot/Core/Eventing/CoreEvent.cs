using System.Collections.Generic;

namespace FC.TelegramBot.Core.Eventing
{
    public class CoreEvent
    {
        public object Sender;
        public List<object> Args;

        public CoreEvent(object sender, List<object> args)
        {
            Sender = sender;
            Args = args;
        }
    }
}
