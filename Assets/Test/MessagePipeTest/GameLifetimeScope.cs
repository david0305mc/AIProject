using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using MessagePipe.VContainer;
using UniRx;
using System;

namespace MyGame
{
    public enum EMessageType
    {
        A,
        B,
        C
    }

    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] HelloScreen helloScreen;
        protected override void Configure(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterBuildCallback(c =>
            {
                GlobalMessagePipe.SetProvider(c.AsServiceProvider());
            });
        }
    }

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

        public static IObservable<TMessage> Subscribe<TMessage>()
        {
            return GlobalMessagePipe.GetSubscriber<TMessage>().AsObservable();
        }

        public static IObservable<TMessage> Subscribe<TKey, TMessage>(TKey key)
        {
            return GlobalMessagePipe.GetSubscriber<TKey, TMessage>().AsObservable(key);
        }
    }
}
