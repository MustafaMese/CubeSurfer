using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance { get => instance; }

    public bool canStart;

    private void Awake() 
    {
        instance = this;
        canStart = false;    
    }
}
