using System;
using System.Collections.Generic;
using System.Text;
using Extensions.Object;

namespace FC.TelegramBot.Core.Messaging.Behaviours
{
    public class LoadFromAssembliesBehaviour : ExBehaviour
    {
        private ExMessagingObject Parent;

        public override void Awake()
        {
            Parent = ParentObject.Unbox<ExMessagingObject>();

            LoadFromAssemblies();
        }

        private void LoadFromAssemblies()
        {
            //TODO: Load from assemblies
        }
    }
}
