using MessagePipe;
using UnityEngine;
using UniRx;
using System;
namespace MyGame
{
    public class MyData
    { 

    }
    public class TestPopup01 : MonoBehaviour
    {

        private CompositeDisposable _disposables = new CompositeDisposable();
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GlobalMessagePipe.GetSubscriber<EMessageType, MyMessage>().Subscribe(EMessageType.A, message =>
                        {
                            Debug.Log($"[A] 수신: {message.Content}");
                        }).AddTo(_disposables);

            GlobalMessagePipe.GetSubscriber<EMessageType, int>().Subscribe(EMessageType.B, message =>
            {
                Debug.Log($"[B] 수신: {message}");
            }).AddTo(_disposables);

            MessageDispatcher.Subscribe<EMessageType, MyData>(EMessageType.A).Subscribe(_ =>
            {

            });
            // .Subscribe(() =>
            // {
            //     Debug.Log($"[C] 수신: ");
            // }).AddTo(_disposables);
        }
    }


}
