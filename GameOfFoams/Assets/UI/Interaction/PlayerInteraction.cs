using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Collider))] //trigger
public class PlayerInteraction : MonoBehaviour {

    [SerializeField]
    protected Transform InteractionUIPrefab;

    IInput input;
    Transform interactionUI;

    /// <summary>
    /// The closest interactable collider within our trigger. Null if one is not present.
    /// </summary>
    Collider target;
    Interactable targetInteractable;

    float readyTime = 0;

	// Use this for initialization
	void Start () {
        input = GetComponentInParent<IInput>();
        interactionUI = Instantiate(InteractionUIPrefab);
        interactionUI.gameObject.SetActive(false);
	}

    void OnDestroy()
    {
        if (interactionUI != null)
        {
            Destroy(interactionUI.gameObject);
        }
    }

    void Update()
    {
        if (targetInteractable != null && targetInteractable.IsInteractable)
        {
            interactionUI.gameObject.SetActive(true);
            targetInteractable.Position(interactionUI);
            if (input.interacting)
            {
                targetInteractable.Interact();
                readyTime = Time.time + 1;
            }
        }
        else
        {
            interactionUI.gameObject.SetActive(false);
        }
    }

    void SetTarget(Collider target, Interactable targetInteractable) {
        this.target = target;
        this.targetInteractable = targetInteractable;
        Assert.IsTrue(targetInteractable.IsInteractable);
    }

    void UnsetTarget()
    {
        target = null;
        targetInteractable = null;
    }

    void OnTriggerEnter(Collider col)
    {
        OnTriggerStay(col);
    }

    void OnTriggerStay(Collider col)
    {
        if (Time.time < readyTime) {
            return;
        }

        if (col == target)
        {
            return;
        }

        Interactable interactable = col.GetComponentInParent<Interactable>();
        if (interactable == null || !interactable.IsInteractable)
        {
            return;
        }

        if (target == null || targetInteractable == null || !targetInteractable.IsInteractable || sqrDistance(col) < sqrDistance(target))
        {
            SetTarget(col, interactable);
        }
    }

    float sqrDistance(Collider col)
    {
        return (col.transform.position - this.transform.position).sqrMagnitude;
    }

    void OnTriggerExit(Collider col)
    {
        if (target == col)
        {
            UnsetTarget();
        }
    }
}
