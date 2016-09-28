using UnityEngine;
using System.Collections;

public class TrapPlacement : MonoBehaviour {

    [SerializeField]
    protected Transform trapPlacementCameraHolder;

    [SerializeField]
    protected GameObject trapPrefab;

    [SerializeField]
    protected float cooldown = 1;

    IInput playerInput;

    bool trapPlacementActive = false;
    Transform playerCameraHolder;
    Coroutine inputCheck = null;
    float readyTime = 0;

	// Use this for initialization
	void Start () {
        GetComponent<Interactable>().InteractEventPublisher += OnInteract;
        playerInput = PlayerManagement.Player.GetComponent<IInput>();
	}

    public void OnInteract()
    {
        ActivateTrapPlacement();
    }

    void ActivateTrapPlacement()
    {
        if (trapPlacementActive)
        {
            return;
        }

        playerCameraHolder = Camera.main.transform.parent;
        Camera.main.transform.SetParent(trapPlacementCameraHolder);
        Camera.main.transform.Reset();
        inputCheck = StartCoroutine(CheckInput());

        trapPlacementActive = true;
    }

    void DeactivateTrapPlacement()
    {
        if (!trapPlacementActive)
        {
            return;
        }

        Camera.main.transform.SetParent(playerCameraHolder);
        Camera.main.transform.Reset();
        playerCameraHolder = null;
        StopCoroutine(inputCheck);
        inputCheck = null;

        trapPlacementActive = false;
    }

    IEnumerator CheckInput()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Escape)) //todo: use player keybinds
            {
                DeactivateTrapPlacement();
            }
            if (playerInput.basicAttack)
            {
                PlaceTrap();
            }
            yield return null;
        }
    }

    void PlaceTrap()
    {
        if (!BaseManagement.baseBuilt)
        {
            BaseManagement.Main.BuildBase();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit info;
        if (Time.time > readyTime && Physics.Raycast(ray, out info, 9999f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore)) 
        {
            BasicBaseBuildingData buildingData = (Instantiate(trapPrefab) as GameObject).GetComponent<BasicBaseBuildingData>();
            buildingData.position = info.point;

            BaseManagement.BaseData.buildings.Add(buildingData);
            BaseManagement.Main.AddBuiltBuilding(buildingData.Build());

            readyTime = Time.time + cooldown;
        }
    }
}
