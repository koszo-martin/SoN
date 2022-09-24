using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance {get; private set;}
    public Button deckButton;
    public Button throwdeck1Button;
    public Button throwdeck2Button;
    public GameObject sackObject;
    public GameObject ownCardsObject;
    public GameObject endTurnButtonObject;
    public GameObject nextButtonObject;

    public GameObject sheriffViewNextButton;
    public Button ownCardSlot1Button;
    public Button ownCardSlot2Button;
    public Button ownCardSlot3Button;
    public Button ownCardSlot4Button;
    public Button ownCardSlot5Button;
    public Button ownCardSlot6Button;

    public Text scoreBoard;

    public void Awake(){
        Instance = this;
    }

    public void startGame(){
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
        if (Player.Instance.isSheriff){
            Debug.Log("Sheriff");
            sackObject.SetActive(false);
            ownCardsObject.SetActive(false);
            endTurnButtonObject.SetActive(false);
            nextButtonObject.SetActive(true);
            nextButtonObject.GetComponent<Button>().interactable = false;
        }else{
            Debug.Log("Not Sheriff");
            sackObject.SetActive(true);
            ownCardsObject.SetActive(true);
            endTurnButtonObject.SetActive(true);
            nextButtonObject.SetActive(false);
            ownCardSlot1Button.interactable = false;
            ownCardSlot2Button.interactable = false;
            ownCardSlot3Button.interactable = false;
            ownCardSlot4Button.interactable = false;
            ownCardSlot5Button.interactable = false;
            ownCardSlot6Button.interactable = false;
            sackObject.GetComponent<Image>().color = Player.Instance.color;
            while (Player.Instance.cards.Count < 6){
                Deck.Instance.draw();
            }
        }
    }

    public void startTurn(){
        if(Player.Instance.isSheriff){
            Player.Instance.endTurn();
        }else{
            deckButton.interactable = true;
            throwdeck1Button.interactable = true;
            throwdeck2Button.interactable = true;
            ownCardSlot1Button.interactable = true;
            ownCardSlot2Button.interactable = true;
            ownCardSlot3Button.interactable = true;
            ownCardSlot4Button.interactable = true;
            ownCardSlot5Button.interactable = true;
            ownCardSlot6Button.interactable = true;
            endTurnButtonObject.SetActive(true);
        }
    }

    public void endTurn(){
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
        if( !Player.Instance.isSheriff){
            ownCardSlot1Button.interactable = false;
            ownCardSlot2Button.interactable = false;
            ownCardSlot3Button.interactable = false;
            ownCardSlot4Button.interactable = false;
            ownCardSlot5Button.interactable = false;
            ownCardSlot6Button.interactable = false;
            endTurnButtonObject.SetActive(false);
        }
        GameManager.Instance.increaseTurn();
        GameManager.Instance.startTurn();
    }

    public void endGame(){
        foreach (Player player in GameManager.Instance.players){
            scoreBoard.text += (player.username + ": " + player.score + "\r\n");
        }
    }

    public void goSackOpen(){
        if(!Player.Instance.isSheriff){
            sheriffViewNextButton.SetActive(false);
        }else{
            sheriffViewNextButton.SetActive(true);
            sheriffViewNextButton.GetComponent<Button>().interactable = false;
        }
    }

    [TargetRpc]
    public void targetActivateNextButton(NetworkConnection networkConnection){
        nextButtonObject.GetComponent<Button>().interactable = true;
    }

    [TargetRpc]
    public void targetActivateNextButtonSheriffView(NetworkConnection networkConnection){
        sheriffViewNextButton.GetComponent<Button>().interactable = true;
    }
}


