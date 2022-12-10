
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

    public Button throwButton1;
    public Button throwButton2;
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

    public void retrieve(){
        UIManager.Instance.deactivateSackButton(Player.Instance.sack.Count-1);
        UIManager.Instance.activateOwnCardButton(Player.Instance.cards.Count);
        Player.Instance.cards.Add(Player.Instance.sack[index]);
        Player.Instance.removeFromSack(index);
    }
    

    public void putInSack()
    {
        UIManager.Instance.deactivateOwnCardButton(Player.Instance.cards.Count-1);
        UIManager.Instance.activateSackButton(Player.Instance.sack.Count);
        moveToOtherContainerLocal(Player.Instance, 5);
        UIManager.Instance.deactivateThrowButtons();
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
