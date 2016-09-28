using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class GameManagement : MonoBehaviour {

    private static GameManagement main;
    public static GameManagement Main { 
        get { return main; }
        private set { main = value; }
    }

    [SerializeField]
    protected PlayerManagement playerManagement;
    public PlayerManagement PlayerManagement { get { return playerManagement; } }

    [SerializeField]
    protected BaseManagement baseManagement;
    public BaseManagement BaseManagement { get { return baseManagement; } }

    [SerializeField]
    protected WaveManagement waveManagement;
    public WaveManagement WaveManagement { get { return waveManagement; } }

    void Awake()
    {
        Assert.IsNull(main);
        main = this;
        PlayerManagement.BuildPlayer();
    }
}
