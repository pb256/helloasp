using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public float rotateSpeed = 30f;

    private bool _emitting;
    public bool emitting
    {
        set
        {
            if (_emitting == value) return;
            _emitting = value;
            foreach (var t in trails) t.emitting = value;
        }
    }

    private List<TrailRenderer> trails = new();

    private void Awake()
    {
        trails.Clear();
        var childs = GetComponentsInChildren<TrailRenderer>();
        foreach (var c in childs) 
            trails.Add(c);
        emitting = false;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(1, 0, 0), rotateSpeed * Time.deltaTime);
    }

    public void Clear()
    {
        foreach (var t in trails) 
            t.Clear();
    }
}
