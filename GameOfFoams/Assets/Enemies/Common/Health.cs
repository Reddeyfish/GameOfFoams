using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Health : MonoBehaviour {

    /// <summary>
    /// Allows prefabs to be combined at runtime. (health bar prefab can't be nested at build-time)
    /// </summary>
    [SerializeField]
    public Transform healthDisplayHolder;

    public Transform deathActionsHolder { get { return this.transform; } }

    [SerializeField]
    protected float maxHealth;

    [SerializeField, Range(0, 1)]
    protected float startingHealthPercentage;

    [SerializeField, ReadOnly]
    private float hitpoints;
    public float Hitpoints { get { return hitpoints; }
        set
        {
            hitpoints = value;
            HealthPercentage = hitpoints / maxHealth;
            CheckDeath();
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

    private IDeathAction[] deathActions;

    public void Construct()
    {
        healthDisplay = healthDisplayHolder.GetComponentInChildren<IHealthDisplay>();
        deathActions = deathActionsHolder.GetComponentsInChildren<IDeathAction>();
        Hitpoints = startingHealthPercentage * maxHealth;
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Hitpoints = maxHealth;
    }

    public void Heal(float amount) { Hitpoints = Mathf.Min(maxHealth, hitpoints + amount); }

    public void Damage(float amount) { Hitpoints = Mathf.Max(0, hitpoints - amount); }

    public void CheckDeath()
    {
        if (hitpoints <= 0)
        {
            foreach (IDeathAction death in deathActions)
            {
                death.Die();
            }
        }
    }
}

public interface IHealthDisplay
{
    float healthPercentage { set; }
}

public interface IDeathAction
{
    void Die();
}