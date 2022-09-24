using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Deck : NetworkBehaviour
{
    [field: SyncObject, SerializeField]
    public readonly SyncList<Card> cards = new SyncList<Card>();

    public static Deck Instance {get; private set;}

    public int maxCards;
    public Card prefab;

    public override void OnStartClient(){
        base.OnStartClient();
        if(InstanceFinder.IsServer){
            createDeck();  
        }
        Instance = this;
    }
    
    public void draw(){
        List<Card> ownCards = Player.Instance.cards;
        if(ownCards.Count < 6){
            Card card = cards[0];
            removeCard();
            ownCards.Add(card);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void createDeck(){
        Debug.Log("create Deck");
        for(int i = 0; i<20 ; i++){
            addCard(prefab);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void removeCard(){
        cards.Remove(cards[0]);
    }

    [ServerRpc(RequireOwnership = false)]
    private void addCard(Card card){
        cards.Add(card);
    }
}
