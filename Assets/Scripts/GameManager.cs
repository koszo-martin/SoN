using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;


public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SyncObject, SerializeField]
    public SyncList<Player> players { get; } = new SyncList<Player>();

    [field: SyncObject]
    public SyncList<Color> colors { get; } = new SyncList<Color>();

    public Dropdown cardNameDropDown;

    [SyncVar]
    public int turn;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Instance = this;
        setTurn(0);
        InitPlayers();
        initColors();
    }

    [ServerRpc(RequireOwnership = false)]
    public void InitPlayers()
    {
        observerAddPlayers();
        Debug.Log(players.Count);
    }

    public void initColors()
    {
        addColor(Color.red, GameManager.Instance);
        addColor(Color.black, GameManager.Instance);
        addColor(Color.green, GameManager.Instance);
        addColor(Color.yellow, GameManager.Instance);
        addColor(Color.blue, GameManager.Instance);
        addColor(Color.magenta, GameManager.Instance);
    }

    [ServerRpc(RequireOwnership = false)]
    private void addColor(Color color, GameManager gameManager)
    {
        if (!gameManager.colors.Contains(color))
        {
            gameManager.colors.Add(color);
        }
    }

    [ObserversRpc]
    private void observerAddPlayers()
    {
        Player.Instance.addToManager();
    }

    public void startGame()
    {
        setPlayerColors(GameManager.Instance);
        startRoundServer(GameManager.Instance, UIManager.Instance);
    }

    public void startRound()
    {
        if (checkForGameEnd())
        {
            calculateScores(GameManager.Instance);
            endGame(GameManager.Instance);
        }
        else
        {
            startRoundServer(GameManager.Instance, UIManager.Instance);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void startRoundServer(GameManager gameManager, UIManager uiManager)
    {
        Debug.Log(gameManager.players.Count);
        int lowestSheriffCount = 5;
        foreach (Player player in gameManager.players)
        {
            if (player.beenSheriff < lowestSheriffCount)
            {
                lowestSheriffCount = player.beenSheriff;
            }
        }
        foreach (Player player in gameManager.players)
        {
            bool isSheriff = (player.beenSheriff == lowestSheriffCount);
            if (isSheriff) lowestSheriffCount = 10;
            setSheriff(player, isSheriff);
            player.sack.Clear();
            player.thrownCards = 0;
            player.targetSetSheriff(player.Owner, isSheriff);
            player.startGame();
        }
        startTurn(gameManager, uiManager);
    }

    [ServerRpc(RequireOwnership = false)]
    public void startTurn(GameManager gameManager, UIManager uiManager)
    {
        if (gameManager.turn < gameManager.players.Count)
        {
            gameManager.players[gameManager.turn].startTurn();
        }
        else
        {
            uiManager.targetActivateNextButton(findSheriff().Owner);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void endTurn(GameManager gameManager)
    {
        gameManager.players[gameManager.turn].endTurn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void calculateScores(GameManager gameManager)
    {
        foreach (Player player in gameManager.players)
        {
            player.score = player.coins;
            foreach (Card card in player.stash)
            {
                player.score += card.value;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void endGame(GameManager gameManager)
    {
        foreach (Player player in gameManager.players)
        {
            player.endGame(gameManager);
        }
    }

    public void sackCheck()
    {
        setTurn(0);
        foreach (Player player in players)
        {
            player.goSackOpen();
        }
        initSacks(SackContainer.Instance);
    }

    public void initSacks(SackContainer sackContainer)
    {
        for (int i = 0; i < players.Count; i++)
        {
            sackContainer.localSacks[i].initalize(i);
        }
    }

    public bool allReady()
    {
        foreach (Player player in players)
        {
            if (player.isReady == false)
            {
                return false;
            }
        }
        return true;
    }

    [ServerRpc(RequireOwnership = false)]
    public void setPlayerColors(GameManager gameManager)
    {
        foreach (Player player in gameManager.players)
        {
            player.color = gameManager.colors[0];
            gameManager.colors.RemoveAt(0);
        }
    }

    public void setSheriff(Player player, bool value)
    {
        player.isSheriff = value;
        if (value == true)
        {
            player.beenSheriff += 1;
        }
    }

    public Player findSheriff()
    {
        foreach (Player player in players)
        {
            if (player.isSheriff) return player;
        }
        return null;
    }


    public void giveOffer()
    {
        int value = (int)HaggleMenu.Instance.slider.value;
        offer(findSheriff(), Player.Instance, value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void offer(Player sheriff, Player player, int value)
    {
        sheriff.offer(player, value);
    }

    [ServerRpc(RequireOwnership = false)]
    public void setTurn(int value)
    {
        turn = value;
    }

    [ServerRpc(RequireOwnership = false)]
    public void increaseTurn()
    {
        turn++;
    }

    private bool checkForGameEnd()
    {
        bool result = true;
        foreach (Player player in players)
        {
            if (player.beenSheriff < 2) result = false;
        }
        return result;
    }
}
