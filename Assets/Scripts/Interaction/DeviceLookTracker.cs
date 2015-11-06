﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DeviceLookTracker : MonoBehaviour {
    public Transform vr_eye;
    private Transform m_activeFocussedDevice = null;

    public int overrideActiveDevice { set
        { lookAtDevice(transform.GetChild(value)); }
    }

    void Update()
    {
        lookAtDevice(focusedLookDevice());
    }

    private void lookAtDevice(Transform target)
    {
        if (target != m_activeFocussedDevice)
        {
            if (m_activeFocussedDevice != null)
                m_activeFocussedDevice.GetComponent<DeviceResizer>().minimize();
            m_activeFocussedDevice = target;
            m_activeFocussedDevice.GetComponent<DeviceResizer>().maximize();
        }
    }

    public RectTransform focusedLookDevice()
    {
        if (transform.childCount == 1)
            return (RectTransform)transform.GetChild(0);

        float smallestAngle = -1.0f;
        RectTransform closestLookTarget = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform device = (RectTransform)transform.GetChild(i);
            if (device.gameObject.activeSelf)
            {
                Vector3[] corners = new Vector3[4];
                device.GetWorldCorners(corners);
                Vector3 offset = device.position - Vector3.Lerp(corners[0], corners[2], 0.5f);
                float angle = Vector3.Angle(vr_eye.forward, (device.position - offset) - vr_eye.position);

                if (smallestAngle < 0.0f)
                    smallestAngle = angle;

                if (angle <= smallestAngle)
                {
                    smallestAngle = angle;
                    closestLookTarget = device;
                }
            }
        }
        return closestLookTarget;
    }
}