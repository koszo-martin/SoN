using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class ThrowDeck : NetworkBehaviour
{
    [field: SyncObject]
    public readonly SyncList<Card> cards = new SyncList<Card>();

    public void draw()
    {
        UIManager.Instance.activateOwnCardButton(Player.Instance.cards.Count);
        if (Player.Instance.cards.Count < 6)
        {
            Player.Instance.cards.Add(cards[0]);
            removeCard();
        }
    }

    public void throwCard(int index)
    {
        UIManager.Instance.deactivateOwnCardButton(Player.Instance.cards.Count-1);
        if (Player.Instance.thrownCards < 5)
        {
            moveToOtherContainer(Player.Instance.cards, index);
            gameObject.GetComponent<Button>().interactable = false;
            Player.Instance.thrownCards++;
        }
    }

    private void moveToOtherContainer(List<Card> container, int index)
    {
        addCard(container[index]);
        container.RemoveAt(index);
        this.gameObject.SetActive(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void removeCard()
    {
        cards.Remove(cards[0]);
    }

    [ServerRpc(RequireOwnership = false)]
    private void addCard(Card card)
    {
        Debug.Log(card.cardName);
        cards.Add(card);
    }
}
