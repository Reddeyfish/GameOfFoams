using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;

public abstract class AbstractEnemyAI : MonoBehaviour
{

    [System.Serializable]
    public class AIParameters
    {
        public float aggroTime = 5f;
    }

    public abstract class AIState
    {
        protected AbstractEnemyAI stateMachine;
        public AIState(AbstractEnemyAI stateMachine)
        {
            this.stateMachine = stateMachine;
        }
        public abstract void Update();
        public abstract void OnExit();
        public abstract void OnSuspend();
        public abstract void OnResume();
    }

    public abstract class AtomicAIState : AIState
    {
        public AtomicAIState(AbstractEnemyAI stateMachine) : base(stateMachine) { }
        public override void OnSuspend()
        {
            stateMachine.popState(this);
            stateMachine.currentState.OnSuspend();
        }

        public override void OnResume() { }
    }

    public class SeekingPlayerAIState : AtomicAIState
    {
        readonly float aggroLossTime;
        public SeekingPlayerAIState(AbstractEnemyAI stateMachine)
            : base(stateMachine)
        {
            stateMachine.navigation.SeekPlayer();
            aggroLossTime = Time.time + stateMachine.aiParameters.aggroTime;
        }

        public override void Update()
        {
            if (Time.time > aggroLossTime)
            {
                stateMachine.popState(this);
                return;
            }

            stateMachine.navigation.SeekPlayer();
        }

        public override void OnExit() { }
    }

    public class SeekingQueenAIState : AIState
    {
        public SeekingQueenAIState(AbstractEnemyAI stateMachine)
            : base(stateMachine)
        {
            stateMachine.navigation.SeekQueen();
        }

        public override void Update()
        {
            stateMachine.navigation.SeekQueen();
        }

        public override void OnExit() { }

        public override void OnSuspend() { }

        public override void OnResume()
        {
            Update();
        }
    }

    public class SeekingAggroAIState : AIState
    {
        Aggro target;

        public SeekingAggroAIState(AbstractEnemyAI stateMachine, Aggro target)
            : base(stateMachine)
        {
            stateMachine.navigation.SeekQueen();
            this.target = target;
        }

        public override void Update()
        {
            if (target == null)
            {
                stateMachine.popState(this);
                return;
            }
            stateMachine.navigation.Seek(target.transform.position);
        }

        public override void OnExit() { }

        public override void OnSuspend() { }

        public override void OnResume()
        {
            Update();
        }
    }

    public class IdleState : AIState
    {
        public IdleState(AbstractEnemyAI stateMachine) : base(stateMachine) { stateMachine.navigation.ClearSeeking(); }

        public override void Update() { }

        public override void OnExit() { }

        public override void OnSuspend() { }

        public override void OnResume() { }
    }

    [SerializeField]
    protected AIParameters aiParameters;

    protected Health health;
    protected EnemyNavigation navigation;
    protected AIState rootState;
    protected Stack<AIState> stateStack = new Stack<AIState>();

    // Use this for initialization
    protected virtual void Start()
    {
        health = GetComponent<Health>();
        navigation = GetComponent<EnemyNavigation>();
    }

    protected AIState currentState { get { return stateStack.Count > 0 ? stateStack.Peek() : rootState; } }

    // Update is called once per frame
    protected void Update()
    {
        currentState.Update();
    }

    public void pushNewState(AIState newState)
    {
        currentState.OnSuspend();
        stateStack.Push(newState);
    }

    protected void popState(AIState toRemove)
    {
        AIState poppedState = stateStack.Pop();
        Assert.AreEqual(poppedState, toRemove);
        poppedState.OnExit();
        currentState.OnResume();
    }
}

public class BasicEnemyAI : AbstractEnemyAI, IAggroable
{
    protected override void Start()
    {
        health = GetComponent<Health>();
        navigation = GetComponent<EnemyNavigation>();
        rootState = new SeekingQueenAIState(this);
        health.OnHitPublisher += OnHit;
    }

    protected void OnHit(HitData data)
    {
        pushNewState(new SeekingPlayerAIState(this));
    }

    protected virtual void OnDestroy()
    {
        health.OnHitPublisher -= OnHit;
    }
}