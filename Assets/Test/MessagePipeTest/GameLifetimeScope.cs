using UnityEngine;
using VContainer;
using VContainer.Unity;
using MessagePipe;
using MessagePipe.VContainer;
using UniRx;

namespace MyGame
{
    enum EMessageType
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
            builder.RegisterEntryPoint<MessagePipeDemo>(Lifetime.Singleton);
        }
    }

    public class MessagePipeDemo : VContainer.Unity.IStartable
    {
        private readonly ISubscriber<EMessageType, MyMessage> _subscriber;
        private readonly CompositeDisposable _disposables = new();

        public MessagePipeDemo()
        {
            _subscriber = GlobalMessagePipe.GetSubscriber<EMessageType, MyMessage>();
        }

        public void Start()
        {
            _subscriber.Subscribe(EMessageType.A, message =>
            {
                Debug.Log($"[A] 수신: {message.Content}");
            }).AddTo(_disposables);

            _subscriber.Subscribe(EMessageType.B, message =>
            {
                Debug.Log($"[B] 수신: {message.Content}");
            }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
    public class MyMessage
    {
        public string Content;
        public MyMessage(string content)
        {
            Content = content;
        }
    }

}
