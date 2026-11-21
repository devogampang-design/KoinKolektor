using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class PemancarEvent : MonoBehaviour
{
    public static event Action ButtonOnclick;
    public static event Action OnTekanSpasi;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Pemancar: spasi ditekan, kirim event.");
            ButtonOnclick?.Invoke();
            OnTekanSpasi?.Invoke();
        }
    }
}
