using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Health : MonoBehaviour {

    /// <summary>
    /// Allows prefabs to be combined at runtime. (health bar prefab can't be nested at build-time)
    /// </summary>
    [SerializeField]
    public Transform healthDisplayHolder;

    [SerializeField]
    protected float maxHealth;

    [SerializeField]
    [Range(0, 1)]
    protected float startingHealthPercentage;

    [ReadOnly]
    private float hitpoints;
    public float Hitpoints { get { return hitpoints; }
        set
        {
            hitpoints = value;
            HealthPercentage = hitpoints / maxHealth;
        }
    }

    private IHealthDisplay healthDisplay;

    float healthPercentage;
    public float HealthPercentage
    {
        get { return healthPercentage; }
        private set
        {
            healthDisplay.healthPercentage = healthPercentage = value;
        }
    }

    public void Construct()
    {
        healthDisplay = healthDisplayHolder.GetComponentInChildren<IHealthDisplay>();
        Hitpoints = startingHealthPercentage * maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Hitpoints = Mathf.Min(Hitpoints, maxHealth);
    }

    public bool IsDead() { return hitpoints <= 0; }

    public void Heal(float amount) { Hitpoints = Mathf.Min(maxHealth, hitpoints + amount); }

    public void Damage(float amount) { Hitpoints = Mathf.Max(0, hitpoints - amount); }
}

public interface IHealthDisplay
{
    float healthPercentage { set; }
}