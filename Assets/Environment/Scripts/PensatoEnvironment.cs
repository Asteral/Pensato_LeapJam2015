using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PensatoEnvironment : MonoBehaviour {

    public List<float> audioData;
    private float dataUpdate = 0.0f;
    private bool updatedable = false;

    public int envSize;
    public float envPadding;

    protected int samples = 128;

    public List<GameObject> assets = new List<GameObject>();

    void Awake()
    {
        audioData = new List<float>(new float[samples]);
    }

    void Start () {
        
    }
	
	void Update () {
        
    }

    void FixedUpdate()
    {
        updateAudioData();
    }

    public void addAudioData(float d)
    {
        dataUpdate = d;
        updatedable = true;
    }

    private void updateAudioData()
    {
        if (updatedable)
        {
            audioData.Insert(0, dataUpdate);
            audioData.RemoveAt(audioData.Count - 1);
            updatedable = false;
        }
        else
        {
            dataUpdate = 0;
            audioData.Insert(0, dataUpdate);
            audioData.RemoveAt(audioData.Count - 1);
        }
    }

    public virtual void initEnv() { }
}
