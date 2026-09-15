using UnityEngine;
using UnityEngine.Events;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private int hp = 100;
    public float ms = 2f;
    [SerializeField] protected ZombieConfig config;

    protected Transform player;

    [Header("State Machine")]
    [SerializeField] private float JarakDeteksi = 6f;
    [SerializeField] private float JarakSerang = 1.5f;
    [SerializeField] private float JedaSerang = 1f;

    public static event Action<Enemy> OnZombieMati;

    [Header("Event Kematian")]
    [SerializeField] private UnityEvent onZombieMatiVisual;

    private StateZombie state = StateZombie.IDLE;
    private float waktuSerangTerakhir;

    protected virtual void Start()
    {
        if (config != null)
        {
            TerapkanConfig();
        }

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Menyalin nilai dari aset ZombieConfig ke variabel Enemy ini.
    // Aset yang sama bisa dipakai ulang oleh banyak zombie (mis. semua Boss).
    protected virtual void TerapkanConfig()
    {
        hp = config.hp;
        ms = config.kecepatan;
        JarakDeteksi = config.jarakDeteksi;
        JarakSerang = config.jarakSerang;
    }

    void Update()
    {
        PeriksaTransisi();

        switch (state)
        {
            case StateZombie.IDLE: PerilakuIdle(); break;
            case StateZombie.PATROL: PerilakuPatrol(); break;
            case StateZombie.CHASE: PerilakuChase(); break;
            case StateZombie.ATTACK: PerilakuAttack(); break;
        }
    }

    public float JarakKePlayer()
    {
        if (player == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, player.position);
    }

    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= JarakSerang)
        {
            state = StateZombie.ATTACK;
        }
        else if (jarak <= JarakDeteksi)
        {
            state = StateZombie.CHASE;
        }
        else
        {
            state = StateZombie.PATROL;
        }
    }

    public void Kejar()
    {
        if (player == null) return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }

    public virtual void Serang()
    {
        Debug.Log("Enemy Serang");
    }

    // Dipanggil saat zombie kena damage (mis. dari peluru/serangan player).
    public void KenaDamage(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Mati();
        }
    }

    // Zombie "berteriak" saat mati, tanpa perlu tahu siapa yang mendengar.
    protected virtual void Mati()
    {
        Debug.Log(name + " kalah!");

        // '?.Invoke' -> aman walau belum ada yang subscribe (tidak error)
        OnZombieMati?.Invoke(this); // kirim 'this' = info diri sendiri

        // Untuk hal pelengkap yang disambung lewat Inspector (suara, efek, dll)
        onZombieMatiVisual?.Invoke();

        Destroy(gameObject);
    }

    void PerilakuIdle()
    {
        Debug.Log("Enemy sedang IDLE");
    }

    void PerilakuPatrol()
    {
        Debug.Log("Enemy sedang PATROL");
    }

    void PerilakuChase()
    {
        Kejar();
        Debug.Log("Enemy sedang CHASE");
    }

    void PerilakuAttack()
    {
        Debug.Log("Enemy sedang ATTACK");
    }
}