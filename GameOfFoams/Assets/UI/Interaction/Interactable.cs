using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Interactable : MonoBehaviour {

    [SerializeField]
    protected float interactionCooldown = 1;

    [SerializeField]
    protected float billboardHeightFromCenter = 1;

    [SerializeField]
    protected float billboardDepthFromCenter = 1;

    private bool isInteractable = true;
    public bool IsInteractable {
        get { return isInteractable; }
        private set { isInteractable = value; }
    }

    public delegate void InteractEvent();
    public event InteractEvent InteractEventPublisher = delegate { };

    public virtual void Position(Transform toPosition)
    {
        Vector3 viewDir = Camera.main.transform.position - transform.position;
        viewDir.y = 0;
        viewDir.Normalize();
        toPosition.position = transform.position
            + (billboardHeightFromCenter * Vector3.up)
            + (billboardDepthFromCenter * viewDir);
        toPosition.rotation = Quaternion.LookRotation(-viewDir, Vector3.up);
    }

    public void Interact()
    {
        Assert.IsTrue(IsInteractable);
        IsInteractable = false;
        Callback.FireAndForget(() => IsInteractable = true, interactionCooldown, this);
        InteractEventPublisher();
    }
}
