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
            if(InstanceFinder.IsServer){
                InstanceFinder.ServerManager.StopConnection(true);
            }else{
                InstanceFinder.ClientManager.StopConnection();
            }
        });
        base.Initialize();
    }
}
