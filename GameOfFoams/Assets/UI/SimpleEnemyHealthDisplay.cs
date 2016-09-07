using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimpleEnemyHealthDisplay : MonoBehaviour, IHealthDisplay {

    [SerializeField]
    protected Image fillMask;

    [SerializeField]
    protected Image backgroundFillMask;

    public float healthPercentage { set { fillMask.fillAmount = backgroundFillMask.fillAmount = value; } }
}
