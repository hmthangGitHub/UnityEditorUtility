using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBreakPoint : MonoBehaviour
{
    public bool isPauseWhenActive = false;
    public bool isPauseWhenInActive = false;
    public bool isPauseWhenDestroy = false;

    private void OnEnable()
    {
        if(isPauseWhenActive) Debug.Break();
    }

    private void OnDisable()
    {
        if(isPauseWhenInActive) Debug.Break();
    }

    private void OnDestroy()
    {
        if(isPauseWhenDestroy) Debug.Break();
    }
}
