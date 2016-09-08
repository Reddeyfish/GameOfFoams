using UnityEngine;
using System.Collections;

/// <summary>
/// Component that lets something carry loot
/// </summary>
public class MoneyBag : MonoBehaviour {

    public delegate void MoneyEvent();

    /// <summary>
    /// UI hook
    /// </summary>
    public event MoneyEvent MoneyEventPublisher = delegate { };

    private int money = 0;
    public int Money
    {
        get { return money; }
        set
        {
            money = value;
            MoneyEventPublisher();
        }
    }

}
