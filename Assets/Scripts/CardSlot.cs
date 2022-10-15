
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class CardSlot : MonoBehaviour
{
    public int index;
    public string parentName;
    void Update()
    {
        
        List<Card> cards = new List<Card>();
        if(parentName == "OwnCards"){
            cards = Player.Instance.cards;
        }else if ( parentName == "Sack"){
            foreach (Card card in Player.Instance.sack){
                cards.Add(card);
            }
        }
        Image image = gameObject.GetComponent<Image>();
        if(index <= cards.Count-1 ){
            image.sprite = Resources.Load<Sprite>("Sprites/" + cards[index].cardName);
            Color tempColor = image.color;
            tempColor.a = 1f;
            image.color = tempColor;
            if(gameObject.GetComponent<Button>() != null){
                gameObject.GetComponent<Button>().interactable = true;
            }
        }else{
            image.sprite = null;
            Color tempColor = image.color;
            tempColor.a = 0f;
            image.color = tempColor;
            if(gameObject.GetComponent<Button>() != null){
                gameObject.GetComponent<Button>().interactable = false;
            }
        }
        
    }

    public void retrieve(){
        Player.Instance.cards.Add(Player.Instance.sack[index]);
        Player.Instance.sack.RemoveAt(index);
    }
    

    public void putInSack()
    {
        moveToOtherContainerLocal(Player.Instance, 5);
    }

    private void moveToOtherContainerLocal(Player player, int maxCards)
    {
        if( player.sack.Count < maxCards){
            Card card = player.cards[index];
            player.addToSack(card);
            player.cards.RemoveAt(index);
        }
    }
}
