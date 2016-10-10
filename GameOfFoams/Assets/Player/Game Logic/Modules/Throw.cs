using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public interface Throwable {
    void Instantiate(ITeamReference teamReference);
}

public class Throw : MonoBehaviour {

    [SerializeField]
    protected GameObject prefab;

    [SerializeField]
    public int numObjects = 1;

    [SerializeField]
    protected float cooldown = 1;

    public delegate void ThrowEvent();

    /// <summary>
    /// UI hook
    /// </summary>
    public event ThrowEvent throwEvent = delegate { };
    IInput input;
    ITeamReference teamReference;

    float readyTime = 0;

    void Start() {
        input = GetComponentInParent<IInput>();
        teamReference = GetComponentInParent<ITeamReference>();
    }

    void Update() {
        if (input.beanBag && Time.time > readyTime && numObjects > 0) {
            GameObject thrown = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
            foreach (Throwable t in thrown.GetComponents<Throwable>()) {
                t.Instantiate(teamReference);
            }
            numObjects--;
            readyTime = Time.time + cooldown;
            throwEvent();
        }
    }
}
