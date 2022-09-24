using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class SackContainer : NetworkBehaviour
{
    public static SackContainer Instance {get; private set;}
    public List<NetworkedSack> localSacks = new List<NetworkedSack>();

    private void Awake(){
        Instance = this;
    }

    void Update(){
        foreach (NetworkedSack sack in localSacks){
            if (sack.owner == null){
                sack.gameObject.SetActive(false);
            }else{
                sack.gameObject.SetActive(true);
            }
        }
    }

}
