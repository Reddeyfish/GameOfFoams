using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

[RequireComponent(typeof(Collider))] //trigger
public class Aggro : MonoBehaviour {

    void OnTriggerEnter(Collider coll)
    {
        IAggroable aggroable = coll.GetComponentInParent<IAggroable>();
        AbstractEnemyAI ai = aggroable as AbstractEnemyAI;
        if (aggroable != null)
        {
            Assert.IsNotNull(ai);
            AbstractEnemyAI.AIState newState = new AbstractEnemyAI.SeekingAggroAIState(ai, this);
            aggroable.pushNewState(newState);
        }
    }
}

public interface IAggroable 
{
    void pushNewState(AbstractEnemyAI.AIState newState);
}
