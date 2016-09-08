using UnityEngine;
using System.Collections;

public class HealthPotion : MonoBehaviour {

    [SerializeField]
    protected float healingAmount = 50;

    [SerializeField]
    public int numPotions = 1;

    [SerializeField]
    protected float cooldown = 1;

    public delegate void HealthPotionEvent();

    /// <summary>
    /// UI hook
    /// </summary>
    public event HealthPotionEvent healthPotionEvent = delegate { };
    Health health;
    IInput input;
    float readyTime = 0;

    void Start()
    {
        input = GetComponentInParent<IInput>();
        health = GetComponentInParent<Health>();
    }

    void Update()
    {
        if (input.potion && Time.time > readyTime && numPotions > 0)
        {
            health.Heal(healingAmount);
            numPotions--;
            readyTime = Time.time + cooldown;
            healthPotionEvent();
        }
    }
}
