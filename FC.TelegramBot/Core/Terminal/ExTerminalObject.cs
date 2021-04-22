using Extensions.Object;
using Extensions.Object.Attributes;
using FC.TelegramBot.Core.Terminal.Behaviours;

namespace FC.TelegramBot.Core.Terminal
{
    public enum WriteBehaviourType
    {
        Info = 7,
        Error = 12,
        Success = 10,
        Warning = 14,
        Debug = 11
    }

    [RequiredBehaviour( typeof( ReadBehaviour ) )]
    [RequiredBehaviour( typeof( WriteBehaviour ) )]
    public class ExTerminalObject : ExObject
    {
        public WriteBehaviour Write()
            => GetComponent<WriteBehaviour>();

        public ReadBehaviour Read()
            => GetComponent<ReadBehaviour>();
    }
}
