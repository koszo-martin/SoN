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
    public List<Card> prefabs;

    public ThrowDeck throwDeck1;
    public ThrowDeck throwDeck2;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Instance = this;
    }

    public void draw()
    {
        UIManager.Instance.activateOwnCardButton(Player.Instance.cards.Count);
        List<Card> ownCards = Player.Instance.cards;
        if (ownCards.Count < 6)
        {
            Card card = cards[0];
            removeCard();
            ownCards.Add(card);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void createDeck(Deck deck)
    {

        foreach (Card card in prefabs)
        {
            for (int i = 0; i < card.numberInDeck; i++)
            {
                deck.cards.Add(card);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void shuffle(Deck deck)
    {
        int count = deck.cards.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            Card tmp = deck.cards[i];
            deck.cards[i] = deck.cards[r];
            deck.cards[r] = tmp;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void initThrowDecks(Deck deck, ThrowDeck throwDeck1, ThrowDeck throwDeck2)
    {
        for (int i = 0; i < 5; i++)
        {
            throwDeck1.cards.Add(deck.cards[0]);
            deck.cards.RemoveAt(0);
            throwDeck2.cards.Add(deck.cards[0]);
            deck.cards.RemoveAt(0);
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
