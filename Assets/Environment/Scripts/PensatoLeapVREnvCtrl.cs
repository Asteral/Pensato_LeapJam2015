using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PensatoLeapVREnvCtrl : MonoBehaviour {

    public static PensatoLeapVREnvCtrl instance;

    public static Color[] colorChoices = {
        new Color(0f / 255f, 153f / 255f, 255f / 255f),
        new Color(84f / 255f, 163f / 255f, 255f / 255f),
        new Color(155f / 255f, 255f / 255f, 30f / 255f),
        new Color(255f / 255f, 47f  / 255f,   0f / 255f)
    };

    public static Color currentColor;

    LiveSongProxyController liveSongCtrl;
    public bool ready = false;

    //- Env 0 -----------------------------------
    public GameObject env_0_tileAsset;
    public int env_0_rad = 40;
    public float env_0_padding = .025f;
    //-----------------------------------

    //- Env 1 -----------------------------------
    public GameObject env_1_fireworksAsset;
    public int env_1_numFireworks = 100;
    public float env_1_nearRadius = 0.15f;
    public float env_1_farRadius = 0.3f;
    public float env_1_fireworkMaxHeight = 0.35f;
    //-----------------------------------

    public List<GameObject> environments = new List<GameObject>();
    public PatchCable trackAmplitudePlug = new PatchCable(PatchCable.PlugType.PLUG);
    public LiveLink liveLink;

    public float audioAmplAvg = 0;
    public float lastAmplAvg = -1;

    void Start () {
        PensatoLeapVREnvCtrl.instance = this;
        PensatoLeapVREnvCtrl.currentColor = PensatoLeapVREnvCtrl.colorChoices[0];

        liveSongCtrl = (LiveSongProxyController)LiveSongProxyController.instance;
<<<<<<< HEAD

        initEnvironments();
=======
        addEnvironment(40, 0.025f);

        liveLink.proxyCreationComplete += connectPlugs;
>>>>>>> refs/remotes/Mystfit/master
    }

    public void connectPlugs()
    {
        LiveTrackProxyController trackControl = (LiveTrackProxyController)LiveTrackProxyController.instance;
        foreach (LiveTrackProxy track in trackControl.proxies.Values)
            track.amplitudeJack.Connect(trackAmplitudePlug);
    }
	
	void Update ()
    {
        audioAmplAvg = 0;
        int total = 0;
        if (trackAmplitudePlug != null)
        {
            if (trackAmplitudePlug.IsDirty) {
                foreach (PatchCable incoming in trackAmplitudePlug.connections)
                {
                    float[] jackValue = trackAmplitudePlug.jackValue(incoming);
                    if (jackValue != null)
                    {
                        for (int i = 0; i < jackValue.Length; i++)
                        {
                            audioAmplAvg += jackValue[i];
                            total++;
                        }
                    }
                }
            }
            if (trackAmplitudePlug.connections.Count > 0)
            {
<<<<<<< HEAD
                audioAmplAvg = (audioAmplAvg + lastAmplAvg) / 2;
                updateAudioDataToEnv(environments[0].GetComponent<PensatoEnvironment>(), audioAmplAvg);
                updateAudioDataToEnv(environments[1].GetComponent<PensatoEnvironment>(), audioAmplAvg);
                lastAmplAvg = audioAmplAvg;
=======
                audioAmplAvg /= total;
                if (lastAmplAvg != audioAmplAvg)
                {
                    audioAmplAvg = (audioAmplAvg + lastAmplAvg) / 2;
                    updateAudioDataToEnv(environments[0].GetComponent<PensatoEnvironment>(), audioAmplAvg);
                    lastAmplAvg = audioAmplAvg;
                }
>>>>>>> refs/remotes/Mystfit/master
            }
        }
    }

    private void initEnvironments()
    {
        GameObject env;

        env = addNewEnvWrapper();
        HexGridEnvironment hexGridScript = env.AddComponent<HexGridEnvironment>();
        hexGridScript.setup(env_0_tileAsset, env_0_rad, env_0_padding);
        hexGridScript.initEnv();
        environments.Add(env);

        env = addNewEnvWrapper();
        PlasmaFireworksEnvironment plasmaFireScript = env.AddComponent<PlasmaFireworksEnvironment>();
        plasmaFireScript.setup(env_1_fireworksAsset, env_1_numFireworks, env_1_nearRadius, env_1_farRadius, env_1_fireworkMaxHeight);
        plasmaFireScript.initEnv();
        environments.Add(env);
    }

    private GameObject addNewEnvWrapper()
    {
        GameObject o = new GameObject("PensatoEnv_" + environments.Count.ToString());
        o.transform.SetParent(transform, false);
        return o;
    }

    private void updateAudioDataToEnv(PensatoEnvironment env, float audioData)
    {
        env.addAudioData(audioData);
    }
}
