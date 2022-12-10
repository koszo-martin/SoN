using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }
    public Button deckButton;
    public Button throwdeck1Button;
    public Button throwdeck2Button;
    public GameObject sackObject;
    public GameObject ownCardsObject;
    public GameObject endTurnButtonObject;
    public GameObject nextButtonObject;

    public GameObject sheriffViewNextButton;

    public List<GameObject> ownCardSlots;

    public List<Button> sackButtons;

    public Text scoreBoard;

    public List<Sprite> sprites;

    public void Awake()
    {
        Instance = this;
    }

    public void startGame()
    {
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
        if (Player.Instance.isSheriff)
        {
            Debug.Log("Sheriff");
            sackObject.SetActive(false);
            ownCardsObject.SetActive(false);
            endTurnButtonObject.SetActive(false);
            nextButtonObject.SetActive(true);
            nextButtonObject.GetComponent<Button>().interactable = false;
        }
        else
        {
            Debug.Log("Not Sheriff");
            sackObject.SetActive(true);
            ownCardsObject.SetActive(true);
            endTurnButtonObject.SetActive(true);
            nextButtonObject.SetActive(false);
            sackObject.GetComponent<Image>().color = Player.Instance.color;
        }
    }

    public void deactivateThrowButtons()
    {
        foreach (GameObject obj in ownCardSlots)
        {
            obj.GetComponent<CardSlot>().throwButton1.interactable = false;
            obj.GetComponent<CardSlot>().throwButton2.interactable = false;
        }
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
    }

    public void startTurn()
    {
        if (Player.Instance.isSheriff)
        {
            Player.Instance.endTurn();
        }
        else
        {
            deckButton.interactable = true;
            throwdeck1Button.interactable = true;
            throwdeck2Button.interactable = true;
            GameManager.Instance.cardNameDropDown.interactable = true;
            endTurnButtonObject.SetActive(true);
            for (int i = 0; i< Player.Instance.cards.Count; i++){
                ownCardSlots[i].GetComponent<Button>().interactable = true;
            }
            for (int i = 0; i< Player.Instance.sack.Count; i++){
                sackButtons[i].interactable = true;
            }
            foreach (GameObject obj in ownCardSlots)
            {
                obj.GetComponent<CardSlot>().throwButton1.interactable = true;
                obj.GetComponent<CardSlot>().throwButton2.interactable = true;
            }
        }
    }

    public void endTurn()
    {
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
        foreach (GameObject obj in ownCardSlots)
        {
            obj.GetComponent<Button>().interactable = false;
        }
        foreach (Button button in sackButtons)
        {
            button.interactable = false;
        }
        if (!Player.Instance.isSheriff)
        {
            GameManager.Instance.cardNameDropDown.interactable = false;
            endTurnButtonObject.SetActive(false);
        }
        GameManager.Instance.increaseTurn();
        GameManager.Instance.startTurn(GameManager.Instance, this);
    }

    public void endGame(GameManager gameManager)
    {
        List<Player> players = new List<Player>();
        foreach (Player player in gameManager.players)
        {
            players.Add(player);
        }
        players.Sort((p1, p2) =>
        {
            return p2.score.CompareTo(p1.score);
        });
        for (int i = 0; i < players.Count; i++)
        {
            scoreBoard.text += (i + 1 + ". " + players[i].username + ": " + players[i].score + "\r\n");
        }
    }

    public void goSackOpen()
    {
        if (!Player.Instance.isSheriff)
        {
            sheriffViewNextButton.SetActive(false);
        }
        else
        {
            sheriffViewNextButton.SetActive(true);
            sheriffViewNextButton.GetComponent<Button>().interactable = false;
        }
    }

    [TargetRpc]
    public void targetActivateNextButton(NetworkConnection networkConnection)
    {
        nextButtonObject.GetComponent<Button>().interactable = true;
    }

    [TargetRpc]
    public void targetActivateNextButtonSheriffView(NetworkConnection networkConnection)
    {
        sheriffViewNextButton.GetComponent<Button>().interactable = true;
    }

    public void activateSackButton(int index)
    {
        sackButtons[index].interactable = true;
    }

    public void deactivateSackButton(int index)
    {
        sackButtons[index].interactable = false;
    }

    public void activateOwnCardButton(int index)
    {
        ownCardSlots[index].GetComponent<Button>().interactable = true;
    }

    public void deactivateOwnCardButton(int index)
    {
        ownCardSlots[index].GetComponent<Button>().interactable = false;
    }
}


