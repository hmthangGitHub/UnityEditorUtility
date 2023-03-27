using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TaskTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DoSomething();
    }

    // Update is called once per frame
    void Update()
    {

    }

    async Task DoSomething()
    {
        DoSomethingAsync();
        Debug.Log("Run on DoSomething...");
    }
    async Task DoSomethingAsync()
    {
        await Task.Delay(1000);
        Debug.Log("Run on DoSomethingAsync after delay 1s...");
    }
}
