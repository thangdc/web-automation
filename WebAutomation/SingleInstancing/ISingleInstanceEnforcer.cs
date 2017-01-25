using System;
namespace WebAutomation.SingleInstancing
{
    public interface ISingleInstanceEnforcer
    {
        void OnMessageReceived(MessageEventArgs e);
        void OnNewInstanceCreated(EventArgs e);
    }
}
