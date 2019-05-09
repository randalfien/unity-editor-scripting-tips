using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour
{
    public int Variable1;
    
 //   [EditorButton]
    public void SayHello()
    {
        print("Hello");
    }
    
    [ContextMenu("Here is my item")]
    public void Context()
    {
        print("context");
    }
}
