using UnityEngine;
using System.Collections;

public class SpawnBase : MonoBehaviour {

    void Start()
    {
        GetComponent<Interactable>().InteractEventPublisher += OnInteract;
    }

    public void OnInteract()
    {
        if (!BaseManagement.baseBuilt)
        {
            BaseManagement.Main.BuildBase();
        }
        else
        {
            BaseManagement.Main.DestroyBase();
        }
    }
}
