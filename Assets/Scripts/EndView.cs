using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class EndView : View
{
    [SerializeField]
    private Button exitButton;

    public override void Initialize(){
        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        base.Initialize();
    }
}
