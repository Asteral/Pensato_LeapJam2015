using UnityEngine;
using System.Collections;

public class PlasmaFireworksEnvironment : PensatoEnvironment
{
    public GameObject fireworkAsset;
    public int numFireworks;
    public float nearRadius;
    public float farRadius;
    public float fireworkMaxHeight;

    public float fworkSpeed = 0.01f;
    public float fworkSize = 0.0001f;
    public float fworksMaxDelaySpawn = 10.0f;

    public float explodeThreshold = 0.35f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Reset finished fireworks.
        PlasmaFirework pfw;
        foreach(GameObject fw in assets)
        {
            pfw = fw.GetComponent<PlasmaFirework>();
            if (pfw.awaitngReset)
            {
                fw.transform.position = transform.position + getRandInBounds(nearRadius, farRadius);
                pfw.init(fworkSpeed, fworkSize, fireworkMaxHeight, fworksMaxDelaySpawn, explodeThreshold);
                pfw.ignite();
            }
        }
    }

    public void setup(GameObject asset, int numFWorks, float nearRadBounds, float farRadBounds, float maxHeight)
    {
        fireworkAsset = asset;
        numFireworks = numFWorks;
        nearRadius = nearRadBounds;
        farRadius = farRadBounds;
        fireworkMaxHeight = maxHeight;
    }

    public override void initEnv()
    {
        GameObject fw;
        PlasmaFirework pfw;
        for (int i=0; i<numFireworks; i++)
        {
            fw = (GameObject)Instantiate(fireworkAsset, transform.position + getRandInBounds(nearRadius, farRadius), Quaternion.identity);

            fw.transform.SetParent(transform, false);

            pfw = fw.GetComponent<PlasmaFirework>();
            pfw.idx = (int)Mathf.Abs((Vector3.Distance(fw.transform.position, transform.position) / farRadius) * samples);

            pfw.init(fworkSpeed, fworkSize, fireworkMaxHeight, fworksMaxDelaySpawn, explodeThreshold);
            pfw.ignite();

            pfw.setAudioData(audioData);
            assets.Add(fw);
        }
    }

    // https://rockonflash.wordpress.com/2013/03/14/getrandomindonut-random-location-in-a-donut-area/
    //- Need to move to a more appropriate place later on.
    private Vector3 getRandInBounds(float min, float max)
    {
        float rot = Random.Range(1f, 360f);
        Vector3 direction = Quaternion.AngleAxis(rot, Vector3.up) * Vector3.forward;
        Ray ray = new Ray(Vector3.zero, direction);
        Vector3 p = ray.GetPoint(Random.Range(min, max));
        p.y = 0.0f;
        return p;
    }
}
