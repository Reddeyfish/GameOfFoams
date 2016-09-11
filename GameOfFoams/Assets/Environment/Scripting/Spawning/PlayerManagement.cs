using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[System.Serializable]
public class PlayerData
{
    public float maxHealth = 1f;
}

public class PlayerManagement : MonoBehaviour
{
    [SerializeField]
    protected Transform playerPrefab;

    [SerializeField]
    protected Transform[] modules;

    [SerializeField]
    protected Transform healthBarPrefab;

    [SerializeField]
    protected Transform[] deathActions;

    [SerializeField]
    protected Bindings bindings;

    [SerializeField]
    protected PlayerData playerData;
    public static PlayerData PlayerData { 
        get { return GameManagement.Main.PlayerManagement.playerData; }
    }

    private Transform player;
    public static Transform Player
    {
        get { return GameManagement.Main.PlayerManagement.player; }
        private set { GameManagement.Main.PlayerManagement.player = value; }
    }

    public void BuildPlayer()
    {
        Assert.IsNull(Player);
        Player = Instantiate(playerPrefab);
        IInput input;
        switch (bindings.inputType)
        {
            case Bindings.InputType.MOUSE:
                input = Player.AddComponent<MouseInput>();
                break;
            case Bindings.InputType.CONTROLLER:
                input = Player.AddComponent<ControllerInput>();
                break;
            default:
                input = Player.AddComponent<NullInput>();
                break;

        }
        input.Construct(bindings);

        Health health = Player.GetComponent<Health>();
        health.healthDisplayHolder = (Instantiate(healthBarPrefab) as Transform);
        health.healthDisplayHolder.Reset();

        foreach (Transform deathAction in deathActions)
        {
            (Instantiate(deathAction, health.deathActionsHolder) as Transform).Reset();
        }
        health.Construct();

        Transform weaponsHolder = Player.Find("CameraRotator/WeaponsHolder");
        foreach (Transform weapon in modules)
        {
            (Instantiate(weapon, weaponsHolder) as Transform).Reset();
        }
    }

    public void DestroyPlayer()
    {
        Destroy(Player.gameObject);
        Player = null;
    }

    public void RebuildPlayer()
    {
        DestroyPlayer();
        BuildPlayer();
    }
}
