using UnityEngine;
using System.Collections;

public class WorldSpaceUIPositioning : MonoBehaviour {

    RectTransform thisTransform;

    [SerializeField]
    protected Transform target;

    void Start()
    {
        thisTransform = (RectTransform)transform;
        if (transform.root == Camera.main.transform.root)
        {
            //this.gameObject.SetActive(false);
        }
    }

	// Update is called once per frame
	void LateUpdate () {
        Vector3 worldPos = target.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        if (screenPos.z > 0)
        {
            thisTransform.position = screenPos;
        }
        else
        {
            thisTransform.position = new Vector3(99999, 99999); //not visible
        }
	}
}
