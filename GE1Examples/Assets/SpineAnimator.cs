﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineAnimator : MonoBehaviour {
    public List<Vector3> offsets = new List<Vector3>();
    public List<Transform> children = new List<Transform>();
    public float damping = 10.0f;

    public bool useSpineAnimatorSystem = false;

    // Use this for initialization
    void Start()
    {
        offsets.Add(Vector3.zero);
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            Transform current = transform.parent.GetChild(i);
            if (i > 0)
            {
                Transform prev = transform.parent.GetChild(i - 1);
                offsets.Add((current.transform.position - prev.transform.position));
            }
            children.Add(current);
        }
        if (useSpineAnimatorSystem)
        {
            SpineAnimatorManager.Instance.AddSpine(this);
        }
    }

    // Update is called once per frame
    void Update () {
        if (useSpineAnimatorSystem)
        {
            return;
        }
        for (int i = 1; i < children.Count; i++)
        {
            Transform prev = children[i - 1];
            Transform current = children[i];
            Vector3 wantedPosition = prev.TransformPointUnscaled(offsets[i]);
            Quaternion wantedRotation = prev.rotation;
            current.position = Vector3.Lerp(current.position, wantedPosition, Time.deltaTime * damping);
            current.rotation = Quaternion.Slerp(current.rotation, wantedRotation, Time.deltaTime * damping);
        }
    }
}
