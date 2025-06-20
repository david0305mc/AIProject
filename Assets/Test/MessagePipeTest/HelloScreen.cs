
using MessagePipe;
using UnityEngine;
using UnityEngine.UI;
using MessagePipe.VContainer;

namespace MyGame
{
    
    public class MyMessage
    {
        public string Content;
        public MyMessage(string content)
        {
            Content = content;
        }
    }

    public class HelloScreen : MonoBehaviour
    {
        public Button HelloButton;
        public Button HelloButton2;
        public Button HelloButton3;

        void Awake()
        {
            HelloButton.onClick.AddListener(() =>
            {
                MessageDispatcher.Publish(EMessageType.A, new MyMessage("aaaaa"));
            });

            HelloButton2.onClick.AddListener(() =>
            {
                MessageDispatcher.Publish(EMessageType.B, 5);
            });

            HelloButton3.onClick.AddListener(() =>
            {
                MessageDispatcher.Publish(EMessageType.C);
            });
        }
    }
}
