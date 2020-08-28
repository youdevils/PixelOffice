using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button_HeldBehaviour : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool buttonPressed;
    public bool ButtonIsPressed {
        get {
            return buttonPressed;
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        buttonPressed = false;
    }
}
