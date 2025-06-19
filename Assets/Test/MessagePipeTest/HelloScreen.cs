
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using MessagePipe.VContainer;

namespace MyGame
{
    public class HelloScreen : MonoBehaviour
    {
        public Button HelloButton;
        public Button HelloButton2;

        void Awake()
        {
            HelloButton.onClick.AddListener(() =>
            {
                var pub = GlobalMessagePipe.GetPublisher<EMessageType, MyMessage>();
                pub.Publish(EMessageType.A, new MyMessage("aaaaa"));

            });

            HelloButton2.onClick.AddListener(() =>
            {
                var pub = GlobalMessagePipe.GetPublisher<EMessageType, MyMessage>();
                pub.Publish(EMessageType.B, new MyMessage("bbbb"));
            });
        }
    }
}
