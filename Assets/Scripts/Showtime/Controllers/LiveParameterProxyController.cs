﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LiveParameterProxyController : LiveProxyController<LiveParameterProxy>
{
    public GameObject dragPrefab;

    public LiveParameterProxy createParameter(LiveLink live, string id, string name, string parent, float min, float max, float startValue=0.0f)
    {
        Debug.Log("Building parameter: " + id.ToString());
        LiveParameterProxy parameter = createProxyUI(id.ToString());
        parameter.init(live, id, name, parent, min, max, startValue);
        return parameter;
    }

    public override LiveProxy copyProxy(LiveProxy proxy)
    {
        LiveParameterProxy parameter = (LiveParameterProxy)proxy;
        return createParameter(parameter.live, parameter.id, parameter.proxyName, parameter.parentId, parameter.min, parameter.max);
    }

    public override void registerShowtimeListeners()
    {
        m_live.node.subscribeToMethod(m_live.peer.methods["param_updated"], param_updated);
    }

    private object param_updated(ZST.ZstMethod methodData)
    {
        LiveMessage msg = LiveLink.parseLiveMessage(methodData.output.ToString(), LiveLink.LiveMessageType.VALUE);
        LiveParameterProxy proxy = proxies[msg.id];
        float val = (float)msg.payload;
        if (proxy) queueIncomingAction(() => proxy.receive_value(val) );
        return null;
    }
}
