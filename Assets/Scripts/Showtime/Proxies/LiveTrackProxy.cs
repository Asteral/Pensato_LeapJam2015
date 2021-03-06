﻿using UnityEngine;
using Utils;
using UnityEngine.UI;
using System.Collections;
using System;

public class LiveTrackProxy : LiveProxy {

    public Transform deviceParent;
    public Transform clipParent;
    private Text label;
    private LiveClipProxy m_playingClip;

    void Awake() {
        isCloneable = false;
        label = gameObject.FindInChildren("track_label").GetComponent<Text>();
        deviceParent = gameObject.FindInChildren("devices").transform;
        clipParent = gameObject.FindInChildren("clips").transform;
    }

    public override void init(LiveLink live, string id, string name, string parent)
    {
        label.text = name;
        base.init(live, id, name, parent);
    }

    public override bool AddChild(LiveProxy proxy)
    {
        if(proxy.GetType() == typeof(LiveDeviceProxy))
            proxy.transform.SetParent(deviceParent);
        else if (proxy.GetType() == typeof(LiveClipProxy))
            proxy.transform.SetParent(clipParent);
        return base.AddChild(proxy);
    }

    public void playingClip(LiveClipProxy clip)
    {
        if(m_playingClip != null) m_playingClip.isPlaying = false;
        m_playingClip = clip;
    }

    public override void update_value(object value){}
    public override void receive_value(object value){}
}