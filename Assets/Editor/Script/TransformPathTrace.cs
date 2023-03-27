using System;
using UnityEngine;

public class TransformPathTrace : MonoBehaviour
{
    private GameObject go;
    public float size = 0.1f;
    private void Start()
    {
        go = new GameObject
                    {
                        name = $"{this.gameObject.name}_TransformPathTrace"
                    };
    }

    public void Update()
    {
        var sp = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sp.transform.parent = go.transform;
        sp.transform.position = this.transform.position;
        sp.transform.localScale = Vector3.one * size;
    }
}
