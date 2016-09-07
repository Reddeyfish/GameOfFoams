using UnityEngine;
using System.Collections;

public class SpawnWaves : MonoBehaviour {

    [SerializeField]
    protected Transform waveTemplate;

    [SerializeField]
    protected float numWaves;

	// Use this for initialization
	void Start () {
        for (float i = 0; i < numWaves; i++)
        {
            Transform newWave = Instantiate(waveTemplate, this.transform) as Transform;
            Material mat = newWave.GetComponent<MeshRenderer>().material;
            mat.SetFloat("_Offset", Random.value);
            mat.SetFloat("_Speed", 1 + 4 * Random.value);
            newWave.localPosition = waveTemplate.localPosition;
            newWave.localRotation = waveTemplate.localRotation;
            newWave.localScale = waveTemplate.localScale + ((2 * Random.value + 0.5f) * Vector3.up);
        }
	}
}
