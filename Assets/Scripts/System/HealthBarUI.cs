using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthFill;     // bar health (depan)

 

    void OnEnable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged += HandleHealthChanged;
    }

    void OnDisable()
    {
        if (playerHealth != null) playerHealth.OnHealthChanged -= HandleHealthChanged;
    }

    void Start()
    {
        if (playerHealth == null) return;

        float ratio = (float)playerHealth.Hp / playerHealth.MaxHp;
        healthFill.fillAmount = ratio;
    }

    void HandleHealthChanged(int current, int max)
    {
        float ratio = (float)current / max;
        healthFill.fillAmount = ratio;

   
    }

    void Update()
    {
        
    }
}