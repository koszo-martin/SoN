using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Offer : NetworkBehaviour
{
    public Text text;
    public NetworkedSack parent;

    public int value;

    public void setValue(int value){
        this.value = value;
        text.text = value.ToString();
    }

    public void accept(){
        parent.owner.takeCoin(value);
        GameManager.Instance.findSheriff().addCoin(value);
        parent.hideOffer();
        parent.returnSack();
    }

    public void decline(){
        parent.hideOffer();
    }
}
