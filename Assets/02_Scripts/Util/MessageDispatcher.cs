
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using MessagePipe;
using UniRx;

public static class MessageDispatcher
{
    public static void Publish(EMessageType message)
    {
        GlobalMessagePipe
            .GetPublisher<EMessageType, Unit>()
            .Publish(message, Unit.Default);
    }
    public static void Publish<T>(EMessageType message, T param)
    {
        GlobalMessagePipe
            .GetPublisher<EMessageType, T>()
            .Publish(message, param);
    }

    public static IObservable<Unit> Subscribe<TKey>(TKey key)
    {
        return GlobalMessagePipe.GetSubscriber<TKey, Unit>().AsObservable(key);
    }
    public static IObservable<TMessage> Subscribe<TKey, TMessage>(TKey key)
    {
        return GlobalMessagePipe.GetSubscriber<TKey, TMessage>().AsObservable(key);
    }
    public static IObservable<TMessage> Subscribe<TMessage>(EMessageType key)
    {
        return GlobalMessagePipe.GetSubscriber<EMessageType, TMessage>().AsObservable(key);
    }
    public static IAsyncSubscriber<EMessageType, TMessage> GetAsynSubscriber<TMessage>()
    {
        return GlobalMessagePipe.GetAsyncSubscriber<EMessageType, TMessage>();
    }
    public static UniTask PublishAsync<TMessage>(TMessage message, CancellationToken ct = default)
    {
        return GlobalMessagePipe.GetAsyncPublisher<TMessage>()?.PublishAsync(message, ct) ?? UniTask.CompletedTask;
    }
    public static UniTask PublishAsync<TKey, TMessage>(TKey key, TMessage message, CancellationToken ct = default)
    {
        return GlobalMessagePipe.GetAsyncPublisher<TKey, TMessage>()?.PublishAsync(key, message, ct) ?? UniTask.CompletedTask;
    }
}