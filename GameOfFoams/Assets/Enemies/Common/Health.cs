using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using System.Collections.Generic;

public class Health : MonoBehaviour, ITeamReference {

    public delegate void OnHit(HitData data);

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

    [SerializeField]
    protected Team team;
    public Team Team { get { return team; } set { team = value; } }

    [SerializeField, ReadOnly]
    private float hitpoints;
    public float Hitpoints { get { return hitpoints; }
        private set
        {
            hitpoints = value;
            HealthPercentage = hitpoints / maxHealth;
            CheckDeath();
        }
    }

    public event OnHit OnHitPublisher = delegate {};

    private IHealthDisplay healthDisplay;
    private PlayerData playerData;

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
        playerData = PlayerManagement.PlayerData;
        healthDisplay = healthDisplayHolder.GetComponentInChildren<IHealthDisplay>();
        deathActions = deathActionsHolder.GetComponentsInChildren<IDeathAction>();
        Hitpoints = startingHealthPercentage * maxHealth;
        SetMaxHealth(playerData.maxHealth);
    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        Hitpoints = maxHealth;
    }

    public void Heal(float amount) { Hitpoints = Mathf.Min(maxHealth, hitpoints + amount); }

    public void Damage(float amount) { Hitpoints = Mathf.Max(0, hitpoints - amount); }

    /// <summary>
    /// If you don't know which one to use, use this one
    /// </summary>
    /// <param name="healingAmount"></param>
    /// <param name="teamReference"></param>
    /// <returns></returns>
    public bool Damage(float amount, ITeamReference teamReference)
    {
        Assert.IsNotNull(teamReference);
        if (teamReference.Team != Team)
        {
            Damage(amount);
            HitData data = new HitData(amount, teamReference);
            OnHitPublisher(data);
            return true;
        }
        return false;
    }

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

    public void OnDestroy()
    {
        if (healthDisplayHolder != null && healthDisplayHolder.root != null && healthDisplayHolder.root.gameObject != null)
        {
            Destroy(healthDisplayHolder.root.gameObject);
        }
    }
}

public class HitData
{
    public readonly float damage;
    public readonly ITeamReference source;

    public HitData(float damage, ITeamReference source)
    {
        this.damage = damage;
        this.source = source;
    }
}

public enum Team { PLAYER, ENEMY}

public interface ITeamReference
{
    Team Team { get; }
}

public interface IHealthDisplay
{
    float healthPercentage { set; }
}

public interface IDeathAction
{
    void Die();
}