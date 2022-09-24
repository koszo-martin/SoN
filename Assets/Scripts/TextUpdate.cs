using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    public void textUpdate(float value){
        gameObject.GetComponent<Text>().text = ((int)value).ToString();
    }
}
