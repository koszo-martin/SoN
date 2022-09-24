using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class HaggleMenu : NetworkBehaviour
{
    public Slider slider;

    public static HaggleMenu Instance { get; private set;}

    private void Awake(){
        Instance = this;
    }
}
