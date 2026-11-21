using UnityEngine;
using TMPro;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;
    protected Transform player;
    [SerializeField] private float jarakDeteksi = 6f;
    [SerializeField] private float jarakSerang = 1.2f;

    [Header("Sentuhan")]
    [SerializeField] private int damageSentuh = 20;
    private bool sudahMeledak = false;

    public static event Action<Enemy> OnZombieMati;

    private StateZombie currentState = StateZombie.IDLE;

    public TMP_Text status;

    protected virtual void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        PeriksaTransisi();

        switch (currentState)
        {
            case StateZombie.IDLE:
                PerilakuIdle();
                break;
            case StateZombie.PATROL:
                PerilakuPatrol();
                break;
            case StateZombie.CHASE:
                PerilakuChase();
                break;
            case StateZombie.ATTACK:
                PerilakuAttack();
                break;
        }
    }

    // ===== Sentuhan player =====

    void OnCollisionEnter2D(Collision2D collision)
    {
        TabrakPlayer(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        TabrakPlayer(other.gameObject);
    }

    void TabrakPlayer(GameObject objek)
    {
        if (sudahMeledak) return;
        if (!objek.CompareTag("Player")) return;

        IDamageable target = objek.GetComponentInParent<IDamageable>();
        if (target == null) return;

        sudahMeledak = true;
        target.KenaDamage(damageSentuh);
        Mati();
    }

    // ===== Gerak =====

    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    // Dibiarkan kosong supaya subclass yang override Serang() tidak error
    public virtual void Serang()
    {
    }

    public void KenaDamage(int jumlah)
    {
        hp -= jumlah;
        Debug.Log($"{gameObject.name} kena damage {jumlah}, HP sisa: {hp}");

        if (hp <= 0)
        {
            Mati();
        }
    }

    protected virtual void Mati()
    {
        Debug.Log($"{gameObject.name} mati!");
        OnZombieMati?.Invoke(this);
        Destroy(gameObject);
    }

    // ===== Perilaku state =====

    void PerilakuIdle()
    {
        Debug.Log("Enemy idle");
    }

    void PerilakuPatrol()
    {
        Debug.Log("Enemy patroli");
    }

    void PerilakuChase()
    {
        Kejar();
        Debug.Log("Enemy ngejar");
    }

    void PerilakuAttack()
    {
        // Terus mengejar sampai benar-benar menyentuh player
        Kejar();
        Debug.Log("Enemy menyerang");
    }

    // ===== Helper =====

    public float JarakPlayer()
    {
        if (player == null) return Mathf.Infinity;

        return Vector2.Distance(transform.position, player.position);
    }

    public float JarakKePlayer() => JarakPlayer();

    void PeriksaTransisi()
    {
        float jarak = JarakPlayer();

        if (jarak <= jarakSerang)
        {
            currentState = StateZombie.ATTACK;
        }
        else if (jarak <= jarakDeteksi)
        {
            currentState = StateZombie.CHASE;
        }
        else
        {
            currentState = StateZombie.PATROL;
        }
    }
}