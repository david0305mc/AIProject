using System;
using MessagePipe;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MyGame
{
    public class HelloWorldService
    {
        public void Hello()
        {
            Debug.Log("Hello world");
        }
    }

    public class GamePresenter : IStartable
    {
        readonly HelloWorldService helloWorldService;
        readonly HelloScreen helloScreen;

        public GamePresenter(HelloWorldService helloWorldService, HelloScreen helloScreen)
        {
            this.helloWorldService = helloWorldService;
            this.helloScreen = helloScreen;
        }

        public void Start()
        {
            helloScreen.HelloButton.onClick.AddListener(() => helloWorldService.Hello());
        }

    }
    public class AnyReceiver : IStartable, IDisposable
{
    private readonly CompositeDisposable _disposables = new();

    public void Start()
    {
        Debug.Log("[AnyReceiver] 전역 메시지 구독 시작");

        GlobalMessagePipe.GetSubscriber<string>()
            .Subscribe(OnMessageReceived)
            .AddTo(_disposables);
            
    }

    private void OnMessageReceived(string message)
    {
        Debug.Log($"[AnyReceiver] 메시지 수신: {message}");
    }

    public void Dispose()
    {
        _disposables.Dispose(); // 모든 구독 해제
    }
}
}
