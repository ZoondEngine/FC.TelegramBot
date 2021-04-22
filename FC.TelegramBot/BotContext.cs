using Extensions.Object;
using Extensions.Object.Attributes;
using FC.TelegramBot.Core.CoreBehaviours;

namespace FC.TelegramBot
{
    [RequiredBehaviour(typeof(LoadObjectsBehaviour))]
    public class BotContext : ExObject
    {
        public BotContext()
            : base()
        { }

        public BotContext(string tag)
            : base(tag)
        { }

        public void Load()
            => GetComponent<LoadObjectsBehaviour>().Load();
        public T Child<T>() where T : ExObject
            => GetComponent<LoadObjectsBehaviour>().Find<T>();
        public T[] Childs<T>() where T : ExObject
            => GetComponent<LoadObjectsBehaviour>().All<T>();
    }
}
