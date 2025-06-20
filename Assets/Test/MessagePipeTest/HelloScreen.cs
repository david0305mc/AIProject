
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using MessagePipe.VContainer;
using UniRx;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public class HelloScreen : MonoBehaviour
    {
        public Button HelloButton;
        public Button HelloButton2;
        public Button HelloButton3;
        public Button HelloButton4;
        private CompositeDisposable _disposables = new CompositeDisposable();

        void OnDestroy()
        {
            _disposables.Clear();
        }
        void Awake()
        {
            HelloButton.onClick.AddListener(() =>
            {
                MessageDispatcher.Publish(EMessageType.A);
            });

            HelloButton2.onClick.AddListener(() =>
            {
                MessageDispatcher.Publish(EMessageType.B, 5);
            });

            HelloButton3.onClick.AddListener(() =>
            {
                UniTask.Create(async () =>
                {
                    CancellationTokenSource cts = new CancellationTokenSource();
                    var pubTask = MessageDispatcher.PublishAsync(EMessageType.C, 119, cts.Token);
                    await pubTask;
                    Debug.Log("PublishAsync.Done");
                });
            });
            HelloButton4.onClick.AddListener(() =>
            {
                SceneManager.LoadSceneAsync("Intro");
            });

            MessageDispatcher.Subscribe(EMessageType.A).Subscribe(message =>
            {
                Debug.Log($"[A] 수신: {message}");
            }).AddTo(_disposables);

            MessageDispatcher.Subscribe<int>(EMessageType.B).Subscribe(message =>
            {
                Debug.Log($"[B] 수신: {message}");
            }).AddTo(_disposables);

            MessageDispatcher.Subscribe<int>(EMessageType.B).Subscribe(async message =>
            {
                await UniTask.WaitForSeconds(1f);
                Debug.Log($"[B] 수신: {message}");
            }).AddTo(_disposables);

            MessageDispatcher.Subscribe<int>(EMessageType.C).Subscribe(_ =>
            {
                Debug.Log($"[C] 수신: ");
            });

            MessageDispatcher.GetAsynSubscriber<int>().Subscribe(EMessageType.C, async (v, ct) =>
            {
                Debug.Log($"[C] Async 수신 스타트: ");
                await UniTask.WaitForSeconds(2f, cancellationToken: ct);
                Debug.Log($"[C] Async 수신: ");
            });
        }
    }
}
