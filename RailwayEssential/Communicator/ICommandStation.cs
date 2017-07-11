namespace Communicator
{
    public delegate void ConnectedDelegator(object sender);
    public delegate void DisconnectedDelegator(object sender);
    public delegate void FailureDelegator(object sender);

    interface ICommandStation
    {
        event ConnectedDelegator Connected;
        event DisconnectedDelegator Disconnected;
        event FailureDelegator Failure;

        string Name { get; set; }

        bool Connect();
        bool Disconnect();
    }
}
