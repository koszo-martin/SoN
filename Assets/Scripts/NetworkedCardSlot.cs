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
        SyncList<Card> cards = new SyncList<Card>();
        if(parent.GetComponent<ShowCase>() != null){
            cards = parent.GetComponent<ShowCase>().cards;
        }else{
            cards = parent.GetComponent<ThrowDeck>().cards;
        }
        Image image = gameObject.GetComponent<Image>();
        if(index <= cards.Count-1 ){
            Sprite cardSprite = null;
            foreach( Sprite sprite in UIManager.Instance.sprites){
                if(sprite.name == cards[index].cardName){
                    cardSprite = sprite;
                }
            }
            image.sprite = cardSprite;
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
