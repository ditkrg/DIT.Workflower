namespace DIT.Workflower.Tests;

public enum PhoneState
{
    Idle,
    Ringing,
    Connected,
    Declined,
    OnHold,
}

public enum PhoneCommand
{
    IncomingCall,
    Accept,
    Decline,
    Hold,
    Resume,
    Disconnect,
}

public record PhoneCall(bool Active = true);
