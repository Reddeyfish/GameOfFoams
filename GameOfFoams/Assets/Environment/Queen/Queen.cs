using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;

public class Queen : MonoBehaviour {

    static Transform main = null;
    public static Transform Main { 
        get { return main; }
        private set { main = value; }
    }

	// Use this for initialization
	void Start () {
        Assert.IsNull(Main);
        Main = this.transform;
	}

    void OnDestroy()
    {
        Main = null;
    }
}
