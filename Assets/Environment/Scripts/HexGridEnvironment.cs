using UnityEngine;
using System.Collections;

public class HexGridEnvironment : PensatoEnvironment {

    public GameObject tileAsset;
    public int size;
    public float padding;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void setup(GameObject asset, int rad, float pad)
    {
        tileAsset = asset;
        size = rad;
        padding= pad;
    }

    public override void initEnv()
    {
        int oddFix = (size % 2 == 0) ? 1 : 0;
        float ang30 = Mathf.Deg2Rad * 30;
        float xOff = Mathf.Cos(ang30) * padding;
        float yOff = Mathf.Sin(ang30) * padding;
        int half = size / 2;

        float maxDist = size * padding;

        for (int row = 0; row < size + oddFix; row++)
        {
            int cols = size - Mathf.Abs(row - half) + oddFix;

            for (int col = 0; col < cols; col++)
            {
                Vector3 p = Vector3.zero;
                p.x = xOff * (col * 2 + 1 - cols);
                p.z = yOff * (row - half) * 3; 

                GameObject t = (GameObject)Instantiate(tileAsset, p, Quaternion.Euler(90, 0, 0));
                t.transform.SetParent(gameObject.transform, false);

                Hextile script = t.GetComponent<Hextile>();
                script.idx = (int)Mathf.Abs((Vector3.Distance(t.transform.position, transform.position) / (maxDist * 1.1f)) * samples);
                script.setAudioData(audioData);
                assets.Add(t);
            }
        }
    }
}
