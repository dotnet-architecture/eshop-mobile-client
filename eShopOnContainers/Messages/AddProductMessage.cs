using CommunityToolkit.Mvvm.Messaging.Messages;

namespace eShopOnContainers.Messages;

public class AddProductMessage : ValueChangedMessage<int>
{
    public AddProductMessage(int count) : base(count)
    {
    }
}

