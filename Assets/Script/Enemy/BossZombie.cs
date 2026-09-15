using UnityEngine;

public class BossZombie : Enemy
{
    public override void Serang()
    {
        Debug.Log(name + " (BOSS) melakukan serangan berat!");
    }
}