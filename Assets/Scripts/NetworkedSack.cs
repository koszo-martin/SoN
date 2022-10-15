using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class NetworkedSack : NetworkBehaviour
{
    [field: SyncObject, SerializeField]
    public readonly SyncList<Card> cards = new SyncList<Card>();

    [field: SyncVar]
    public Player owner = null;

    [SyncVar, SerializeField]
    public string cardName;

    [SyncVar]
    public Color color;

    [SerializeField]
    
    private Text text;

    public Button openButton;

    public Button haggleButton;

    public Button returnButton;

    [ServerRpc(RequireOwnership = false)]
    public void addCard(Card card){
        cards.Add(card);
    }

    public GameObject offer;

    void Update(){
        text.text = cards.Count + " " + cardName;
        gameObject.GetComponent<Image>().color = color;
        if(owner == Player.Instance){
            haggleButton.gameObject.SetActive(true);
        }else{
            haggleButton.gameObject.SetActive(false);
        }
        if(Player.Instance.isSheriff == true){
            openButton.gameObject.SetActive(true);
            returnButton.gameObject.SetActive(true);
        }else{
            openButton.gameObject.SetActive(false);
            returnButton.gameObject.SetActive(false);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void setOwner(Player owner){
        this.owner = owner;
        this.color = owner.color;
    }

    [ServerRpc(RequireOwnership = false)]
    public void setCardName(string cardName){
        this.cardName = cardName;
    }

    public void initalize(int number){
        if(GameManager.Instance.players[number].isSheriff != true){
            Debug.Log("Client active:" + InstanceFinder.IsClient);
            cards.Clear();
            foreach (Card card in GameManager.Instance.players[number].sack){
                addCard(card);
            }
            setOwner(GameManager.Instance.players[number]);
            setCardName(GameManager.Instance.players[number].cardName);
        }
    }

    public void open(){
        bool result = checkInside();
        if (result == false){
            playerPenalty();
        }else{
            sheriffPenalty();
        }
        empty();
        activateNextButtonIfAllEmpty(SackContainer.Instance, UIManager.Instance, GameManager.Instance);
    }

    private bool checkInside(){
        foreach (Card card in cards){
            if (card.cardName != cardName){
                return false;
            }
        }
        return true;
    }

    private void playerPenalty(){
        foreach (Card card in cards){
            if (card.cardName != cardName){
                owner.takeCoin(card.penalty);
                GameManager.Instance.findSheriff().addCoin(card.penalty);
            }else{
                owner.addStash(card);
            }
        }
    }

    private void sheriffPenalty(){
        foreach(Card card in cards){
            GameManager.Instance.findSheriff().takeCoin(card.penalty);
            owner.addCoin(card.penalty);
            owner.addStash(card);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void empty(){
        cards.Clear();
        owner.deactivateHaggleMenu();
        this.owner = null;
        deactivate();
    }

    public void showOffer(int value){
        Debug.Log("Showing offer");
        offer.SetActive(true);
        offer.GetComponent<Offer>().setValue(value);
    }

    public void hideOffer(){
        offer.GetComponent<Offer>().setValue(0);
        offer.SetActive(false);
    }

    public void returnSack(){
        foreach (Card card in cards){
            owner.addStash(card);
        }
        empty();
        activateNextButtonIfAllEmpty(SackContainer.Instance, UIManager.Instance, GameManager.Instance);
    }

    [ObserversRpc]
    public void deactivate(){
        this.gameObject.SetActive(false);
    }

    [ServerRpc (RequireOwnership = false)]
    private void activateNextButtonIfAllEmpty(SackContainer sackContainer, UIManager uiManager, GameManager gameManager){
        bool allEmpty = true;
        foreach(NetworkedSack sack in sackContainer.localSacks){
            if(sack.cards.Count != 0 ){
                allEmpty = false;
            }
        }
        if(allEmpty){
            uiManager.targetActivateNextButtonSheriffView(gameManager.findSheriff().Owner);
        }
    }

}
