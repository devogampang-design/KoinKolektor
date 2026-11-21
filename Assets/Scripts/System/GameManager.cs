using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int coins;
    public int totalKoin { get => coins; set => coins = value; }

    public PlayerMovement player;
    private int collected = 0;
    public int koinTerkumpul => collected;

    public TMP_Text coinsText;
    public GameObject winPanel;
    private int hitung = 0;

    void Start()
    {
        if (player == null)
            player = FindAnyObjectByType<PlayerMovement>();

        if (winPanel != null)
            winPanel.SetActive(false);

        collected = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdetTeks();
    }

    public void CollectCoin()
    {
        if (player != null)
            collected = player.score;
        else
            collected++;

        UpdetTeks();

        if (collected >= 5)
        {
            Win();
        }
        else
        {
            Debug.Log("Collected: " + collected);
        }
    }

    // Alias untuk kompatibilitas lama
    public void AmbilKoin()
    {
        CollectCoin();
    }

    public void UpdetTeks()
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + collected.ToString();
    }

    void Win()
    {
        if (winPanel != null)
            winPanel.SetActive(true);

        Debug.Log("Anjay menang");

        if (coinsText != null)
            coinsText.text = "Win coy";
    }

    void OnGUI()
    {
        // Tampilkan teks di pojok layar jika coinsText belum di-assign di Inspector
        if (coinsText == null)
        {
            GUI.skin.label.fontSize = 22;
            GUI.Label(new Rect(16, 16, 480, 36), "Coins: " + collected);
        }
    }
}
