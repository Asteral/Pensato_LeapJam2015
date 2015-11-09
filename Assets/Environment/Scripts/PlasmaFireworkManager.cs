using UnityEngine;
using System.Collections;

public class PlasmaFireworkManager : MonoBehaviour {

	public GameObject fireworkPrefab; //firework prefab instance
	public int numLayers; //number of firework layers
	public int numPerRing; //number of fireworks within a circular layer
	public Vector3 circleCenter;
	public float nearRadius; 
	public float farRadius;
	[Range(0, 10)]
	public int registerOnBand; //the frequency band that will trigger the change in visuals
	public float fireworkMaxHeight;
	public float explosionVolume;
	public float spawnMaxDelay; //how long we should wait (maximum) before spawning a replacement firework

	// Use this for initialization
	void Start () {
		float totalFireworks = numLayers * numPerRing;
		for (int i=0; i< totalFireworks; i++) {
			IgniteFirework(i);
		}
	}

	// Returns a firework's absolute position about the circle
	Vector3 getCircularPosition(int index){
		float layerNum = Mathf.Floor( (float)index / (float)numPerRing );
		float layerOffset = layerNum * 15.0f;
		float theta = (index % numPerRing) / (float)numPerRing * 360.0f + layerOffset;
		theta *= Mathf.Deg2Rad;
		float radius = (layerNum / numLayers) * (farRadius - nearRadius) + nearRadius;
		float x = circleCenter.x + radius * Mathf.Cos (theta);
		float y = circleCenter.y;
		float z = circleCenter.z + radius * Mathf.Sin (theta);
		return new Vector3(x, y, z);
	}

	// Spawns a firework in the scene
	void IgniteFirework(int index){
		Vector3 pos = getCircularPosition(index);
		GameObject firework = (GameObject) Instantiate(fireworkPrefab, pos, Quaternion.identity);
		PlasmaFirework script = firework.GetComponent<PlasmaFirework> ();
		script.setId (index);
		script.setManager (this);
		script.startPos = pos;
        firework.transform.parent = transform;
	}

}
