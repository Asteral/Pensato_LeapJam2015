using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlasmaFirework : MonoBehaviour
{
    private bool ready = false;
    private bool grown = false;
    private bool exploded = false;

    public bool awaitngReset = false;

    public float speed;
    public float size;

	public Vector3 targetPos;

    private float spawnMaxDelay;
    private float spawnDelay;

    public int idx;

	private ParticleSystem particles;

    private List<float> audioData;

    public float explodeThreshold;

	// Use this for initialization
	void Start ()
	{
		particles = GetComponent<ParticleSystem> ();
	}

    public void init(float fwspeed, float fwsize, float fwmaxH, float maxSpawnDelay, float explosionLimit)
	{
        ready = false;
        grown = false;
        exploded = false;
        awaitngReset = false;

        StopAllCoroutines();
        transform.localScale = Vector3.zero;

        speed = fwspeed;
        size = fwsize;

        targetPos = transform.position;
        targetPos.y += Random.Range(fwmaxH*0.5f, fwmaxH);

        spawnMaxDelay = maxSpawnDelay;
        explodeThreshold = explosionLimit;
    }
	
	// Update is called once per frame
	void Update ()
	{
        if (ready && !grown)
        {
            StartCoroutine(Grow());
            grown = true;
        }

        if (grown && !exploded)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos, 0.00005f+ audioData[idx]*.01f);

            transform.Translate(targetPos.normalized * speed);
            transform.localScale = new Vector3(size, size, size);
            speed = (.03f * (audioData[idx]) + 0.00045f);
            size = (.03f * (audioData[idx]) + 0.0015f);
        }

        if ((grown && !exploded))
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.2f || audioData[idx] > explodeThreshold)
            {
                StopCoroutine(Grow());
                StartCoroutine(ExplodeMe());

                exploded = true;
            }
        }
    }

    IEnumerator SpawnDelay()
    {
        spawnDelay = Random.Range(0.0f, spawnMaxDelay);
        yield return new WaitForSeconds(spawnDelay);
        ready = true;
    }

    IEnumerator Grow()
    {
        transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f); //non-zero
        while (transform.localScale.x < size*.00003)
        {
            transform.localScale *= 1.01f;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ExplodeMe()
    {
        particles.startSpeed = 0.005f;
        particles.Play();
        particles.loop = false;
        while (transform.localScale.x > 0.02f)
        {
            transform.localScale *= 0.9f;    
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2.0f);

        particles.Stop();
        awaitngReset = true;
    }

    public void ignite()
    {
        StartCoroutine(SpawnDelay());
    }

    public void setAudioData(List<float> aud)
    {
        audioData = aud;
    }
}
