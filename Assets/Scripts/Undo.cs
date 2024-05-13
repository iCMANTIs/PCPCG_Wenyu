using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Undo : MonoBehaviour
{
    public void Start() 
    {
        //GameManager.instance.Register(gameObject);
    }

    private void OnDestroy()
    {
        //GameManager.instance.Remove(gameObject);
    }
}
