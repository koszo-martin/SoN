using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    
    public void popUp( GameObject popUpBox){
        popUpBox.SetActive(!popUpBox.activeSelf);
    }
}
