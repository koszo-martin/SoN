using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class View : MonoBehaviour
{
    public bool isInitialized {get; private set;}

    public virtual void Initialize()
    {
        isInitialized = true;
    }

    public virtual void Show(object args = null){
        gameObject.SetActive(true);
    }

    public virtual void Hide(){
        gameObject.SetActive(false);
    }
}
