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

    public static Deck Instance { get; private set; }

    public int maxCards;
    public Card prefab;

    public override void OnStartClient()
    {
        base.OnStartClient();
        createDeck(this);
        Instance = this;
    }

    public void draw()
    {
        List<Card> ownCards = Player.Instance.cards;
        if (ownCards.Count < 6)
        {
            Card card = cards[0];
            removeCard();
            ownCards.Add(card);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void createDeck(Deck deck)
    {
        Debug.Log("create Deck");
        if (deck.cards.Count == 0)
        {
            for (int i = 0; i < 20; i++)
            {
                addCard(prefab, deck);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void removeCard()
    {
        cards.Remove(cards[0]);
    }

    [ServerRpc(RequireOwnership = false)]
    private void addCard(Card card, Deck deck)
    {
        deck.cards.Add(card);
    }
}
