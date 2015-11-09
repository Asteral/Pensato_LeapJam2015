using UnityEngine;
using System.Collections;

public class PlasmaFirework : MonoBehaviour
{

	public float speed;
	public Vector3 startPos;
	public Vector3 endPos;
	public float size;
	private int id;
	private PlasmaFireworkManager manager;
	private float spawnDelay;
	private bool ready = false;
	private bool grown = false;
	private bool exploded = false;
	private ParticleSystem particles;

    public float dataPoint = 1.0f;

	// Use this for initialization
	void Start ()
	{
		particles = GetComponent<ParticleSystem> ();
		init ();
	}

	void init ()
	{
		StopAllCoroutines ();
		transform.position = startPos;
		transform.localScale = Vector3.zero;
		ready = false;
		grown = false;
		exploded = false;
		speed = 0.1f;
		size = .003f;
		endPos = new Vector3 (startPos.x, startPos.y + Random.Range (0.0f, manager.fireworkMaxHeight), startPos.z);
		StartCoroutine (SpawnDelay());
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ready && !grown) {
			StartCoroutine (Grow ());
			grown = true;
		}
		if (grown && !exploded) {
			Vector3 toEnd = endPos - transform.position;
			transform.Translate (toEnd.normalized * speed);
			transform.localScale = new Vector3 (size, size, size);
            //speed = (10.0f * (Spectrum.bands [manager.registerOnBand]) + 0.15f);
            //size = (10.0f * (Spectrum.bands [manager.registerOnBand]) + 0.5f);
            speed = (0.03f * dataPoint + 0.00045f);
            size = (0.03f * dataPoint + 0.0015f);
        }
        if ((grown && !exploded)) {
            //if (Vector3.Distance (transform.position, endPos) < speed || Spectrum.getVolume() > manager.explosionVolume) {
            if (Vector3.Distance (transform.position, endPos) < speed || dataPoint > manager.explosionVolume) {
                StartCoroutine(ExplodeMe ());
				exploded = true;
			}
		}
	}

	IEnumerator SpawnDelay ()
	{
		spawnDelay = Random.Range(0.0f, manager.spawnMaxDelay);
		yield return new WaitForSeconds (spawnDelay);
		ready = true;
	}

	IEnumerator Grow ()
	{
		transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f); //non-zero
		while (transform.localScale.x < size) {
			transform.localScale *= 1.1f;
			yield return new WaitForEndOfFrame ();
		}
	}

	IEnumerator ExplodeMe ()
	{
		particles.startSpeed = speed * 10.0f;
		particles.Play ();
		while (transform.localScale.x > 0.01f) {
			transform.localScale *= 0.9f;
			yield return new WaitForEndOfFrame ();
		}
		particles.Stop ();
		init ();
	}

	public void setId (int ID)
	{
		id = ID;
	}

	public void setManager (PlasmaFireworkManager man)
	{
		manager = man;
	}
}
