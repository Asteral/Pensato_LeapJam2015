﻿using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using ThreadingCollections;

public abstract class LiveProxyController<T> : MonoBehaviour where T : Component
{
    private static LiveProxyController<T> c_instance;
    public GameObject controlPrefab;
    protected LiveLink m_live;
    protected Dictionary<string, T> m_proxies;
    private ConcurrentQueue<Action> m_queuedActions;

    public static LiveProxyController<T> instance { get { return LiveProxyController<T>.c_instance; } }

    void Awake()
    {
        c_instance = this;
        m_queuedActions = new ConcurrentQueue<Action>();
        m_proxies = new Dictionary<string, T>();
        m_live = GetComponent<LiveLink>();
    }

    public Dictionary<string, T> proxies
    {
        get { return m_proxies; }
    }

    protected virtual T createProxyUI(string id)
    {
        GameObject uiObj = GameObject.Instantiate(controlPrefab);
        T uiProxy = uiObj.AddComponent<T>();
        m_proxies.Add(id, uiProxy);
        return uiProxy;
    }

    public void queueIncomingAction(Action action)
    {
        m_queuedActions.Enqueue(action);
    }

    public void Update()
    {
        while (m_queuedActions.Count > 0)
        {
            m_queuedActions.Dequeue()();
        }
    }

    public virtual void registerShowtimeListeners() { }
    public virtual LiveProxy copyProxy(LiveProxy proxy){ throw new NotImplementedException(); }
}
