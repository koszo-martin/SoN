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

    public List<Button> ownCardButtons;

    public List<Button> sackButtons;

    public Text scoreBoard;

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
            foreach (Button button in ownCardButtons)
            {
                button.interactable = false;
            }
            foreach (Button button in sackButtons)
            {
                button.interactable = false;
            }
            sackObject.GetComponent<Image>().color = Player.Instance.color;
            while (Player.Instance.cards.Count < 6)
            {
                Deck.Instance.draw();
            }
        }
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
            foreach (Button button in ownCardButtons)
            {
                button.interactable = true;
            }
            foreach (Button button in sackButtons)
            {
                button.interactable = true;
            }
            endTurnButtonObject.SetActive(true);
        }
    }

    public void endTurn()
    {
        deckButton.interactable = false;
        throwdeck1Button.interactable = false;
        throwdeck2Button.interactable = false;
        if (!Player.Instance.isSheriff)
        {
            foreach (Button button in ownCardButtons)
            {
                button.interactable = false;
            }
            foreach (Button button in sackButtons)
            {
                button.interactable = false;
            }
            endTurnButtonObject.SetActive(false);
        }
        GameManager.Instance.increaseTurn();
        GameManager.Instance.startTurn(GameManager.Instance, this);
    }

    public void endGame(GameManager gameManager)
    {
        foreach (Player player in gameManager.players)
        {
            scoreBoard.text += (player.username + ": " + player.score + "\r\n");
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
}


