using UnityEngine;
using System.Collections;

public class SpawnWave : MonoBehaviour
{

    void Start()
    {
        GetComponent<Interactable>().InteractEventPublisher += OnInteract;
    }

    public void OnInteract()
    {
        if (!WaveManagement.waveSpawned)
        {
            WaveManagement.Main.BuildWave();
        }
        else
        {
            WaveManagement.Main.DestroyWave();
        }
    }
}
