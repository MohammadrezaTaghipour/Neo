namespace Neo.Application;

public class MassTransitOptions
{
    public string Host { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public int PrefetchCount { get; set; }
    public string RedisRepository { get; set; }
    public string StreamEventTypeStateMachineAddress { get; set; }
    public string StreamContextStateMachineAddress { get; set; }
    public string LifeStreamStateMachineAddress { get; set; }
}