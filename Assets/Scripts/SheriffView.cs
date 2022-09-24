using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SheriffView : View
{

    public Slider slider;

    public void Update(){
        slider.maxValue = Player.Instance.coins;
    }
}
