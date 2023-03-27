using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    virtual protected void OnSet()
    {

    }

    virtual protected void MyLoveLySet()
    {

    }
}

public class SomeAnotherScript : TestScript
{
    protected override void OnSet()
    {
        base.OnSet();
    }
}
