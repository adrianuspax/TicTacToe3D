using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace TicTacToe3D.GamePlay.Cube
{
    public class Pointer : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        public event UnityAction ClickHandler;
        public event UnityAction DownHandler;
        public event UnityAction UpHandler;

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickHandler?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UpHandler?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            DownHandler?.Invoke();
        }
    }
}
