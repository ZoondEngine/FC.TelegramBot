using Extensions.Object;
using Extensions.Object.Attributes;
using FC.TelegramBot.Core.Commerce.Behaviours;

namespace FC.TelegramBot.Core.Commerce
{
    [RequiredBehaviour(typeof(CartBehaviour))]
    public class ExCommerceObject : ExObject
    {
        public CartBehaviour Cart()
            => GetComponent<CartBehaviour>();
    }
}
