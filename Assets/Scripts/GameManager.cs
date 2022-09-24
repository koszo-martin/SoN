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
    [field: SyncVar]
    public bool canStart { get; private set; }
    [field: SyncVar]
    public bool didStart { get; private set; }

    [field: SyncObject]
    public SyncList<Color> colors { get; } = new SyncList<Color>();

    public Dropdown cardNameDropDown;

    [SyncVar]
    public int turn;

    private void Awake()
    {
        Instance = this;
        setTurn(0);
    }

    [Server]
    public void initColors()
    {
        addColor(Color.red);
        addColor(Color.black);
        addColor(Color.green);
        addColor(Color.yellow);
        addColor(Color.blue);
        addColor(Color.magenta);
    }

    [ServerRpc(RequireOwnership = false)]
    private void addColor(Color color)
    {
        colors.Add(color);
    }

    private void Update()
    {
        if (!IsServer) return;
        canStart = allReady();
    }

    [Server]
    public void startGame()
    {
        didStart = true;
        setPlayerColors();
        startRound();
    }

    [ServerRpc (RequireOwnership = false)]
    public void startRound()
    {
        if(checkForGameEnd()){
            endGame();
            return;
        }
        int lowestSheriffCount = 5;
        foreach (Player player in players)
        {
            if (player.beenSheriff < lowestSheriffCount)
            {
                lowestSheriffCount = player.beenSheriff;
            }
        }
        foreach (Player player in players)
        {
            bool isSheriff = (player.beenSheriff == lowestSheriffCount);
            if (isSheriff) lowestSheriffCount = 10;
            setSheriff(player, isSheriff);
            player.sack.Clear();
            player.thrownCards = 0;
            player.targetSetSheriff(player.Owner, isSheriff);
            player.startGame();
        }
        startTurn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void startTurn()
    {
        if (turn < players.Count)
        {
            players[turn].startTurn();
        }
        else
        {
            UIManager.Instance.targetActivateNextButton(findSheriff().Owner);
        }

    }

    [ServerRpc(RequireOwnership = false)]
    public void endTurn()
    {
        players[turn].endTurn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void endGame()
    {
        foreach (Player player in players){
            player.endGame();
        }
    }

    public void sackCheck()
    {
        setTurn(0);
        foreach (Player player in players)
        {
            player.goSackOpen();
        }
        initSacks();
    }

    public void initSacks()
    {
        for (int i = 0; i < players.Count; i++)
        {
            SackContainer.Instance.localSacks[i].initalize(i);
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
    public void setPlayerColors()
    {
        foreach (Player player in players)
        {
            player.color = colors[0];
            colors.RemoveAt(0);
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

    private bool checkForGameEnd(){
        bool result = true;
        foreach (Player player in players){
            if (player.beenSheriff < 2) result = false;
        }
        return result;
    }
}
