using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int totalKoin;
    private int koinTerkumpul = 0;
    private int jumlahZombieMati = 0;

    void Start()
    {
        totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    // Berlangganan (+=) ke event statis Enemy saat GameManager aktif.
    void OnEnable()
    {
        Enemy.OnZombieMati += SaatZombieMati;
    }

    // WAJIB berhenti berlangganan (-=) saat nonaktif, biar tidak memory leak.
    void OnDisable()
    {
        Enemy.OnZombieMati -= SaatZombieMati;
    }

    void SaatZombieMati(Enemy zombie)
    {
        jumlahZombieMati++;
        Debug.Log("GameManager dengar event. Zombie mati: " + jumlahZombieMati + " (" + zombie.name + ")");
    }

    public void AmbilKoin()
    {
        koinTerkumpul++;
        if (koinTerkumpul == totalKoin)
        {
            Menang();
        }
    }

    void Menang()
    {
        Debug.Log("Selamat anda menang");
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 22;
        GUI.Label(new Rect(16, 16, 480, 36), "Koin: " + koinTerkumpul + " / " + totalKoin);
    }
}
