using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class NetworkedCardSlot : NetworkBehaviour
{
    [SerializeField]
    public int index;

    public GameObject parent;

    void Update()
    {
        SyncList<Card> cards = parent.GetComponent<NetworkedSack>().cards;
        Image image = gameObject.GetComponent<Image>();
        if(index <= cards.Count-1 ){
            image.sprite = Resources.Load<Sprite>("Sprites/" + cards[index].cardName);
            Color tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
        }else{
            image.sprite = null;
            Color tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;
        }
        
    }
}
