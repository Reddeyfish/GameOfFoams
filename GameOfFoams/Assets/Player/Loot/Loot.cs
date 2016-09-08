using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Collider))] //trigger
public class Loot : MonoBehaviour {

    [SerializeField]
    protected Collider collision;

    [SerializeField]
    public int value = 1;

    [SerializeField]
    protected float pickupTime = 1;

    Rigidbody rigid;

    bool seeking = false;
    bool Seeking { 
        get { return seeking; }
        set
        {
            seeking = value;
            collision.enabled = !seeking;
        }
    }

    void Start()
    {
        rigid = GetComponentInParent<Rigidbody>();
    }

    void OnTriggerEnter(Collider coll)
    {
        MoneyBag moneyBag = coll.transform.root.GetComponentInChildren<MoneyBag>();

        if (Seeking || moneyBag == null)
        {
            return;
        }
        else
        {
            StartCoroutine(Seek(moneyBag));
        }
    }

    IEnumerator Seek(MoneyBag target)
    {
        Assert.IsFalse(Seeking);
        Seeking = true;
        Vector3 startPosition = transform.position;

        float pickupTimeRemaining = pickupTime;
        while (pickupTimeRemaining > 0)
        {
            rigid.MovePosition(Vector3.Lerp(startPosition, target.transform.position, 1 - (pickupTimeRemaining / pickupTime)));
            yield return null;
            pickupTimeRemaining -= Time.deltaTime;
            if (target == null)
            {
                Seeking = false;
                yield break;
            }
        }

        target.Money += value;
        Destroy(this.transform.root.gameObject);
    }
}
