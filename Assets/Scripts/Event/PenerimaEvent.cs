using UnityEngine;

public class PenerimaEvent : MonoBehaviour
{
    void OnEnable()
    {
        PemancarEvent.ButtonOnclick += Reaksi;
    }

    void OnDisable()
    {
        PemancarEvent.ButtonOnclick -= Reaksi;
    }

    void Reaksi()
    {
        Debug.Log("Penerima: aku dengar event spasi! Lu di terima njir");
    }
}
