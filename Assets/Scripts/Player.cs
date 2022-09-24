using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class Player : NetworkBehaviour
{
    public static Player Instance {get; private set;}
    [SyncVar]
    public string username;

    [field: SyncVar]
    public bool isReady {get; [ServerRpc] set;}

    [SyncVar]
    public bool isSheriff;

    [SyncVar]
    public int beenSheriff = 0;

    public List<Card> cards = new List<Card>();

    [SyncObject]
    public readonly SyncList<Card> sack = new SyncList<Card>();

    [SyncVar]
    public string cardName;

    [field: SyncVar]
    public Color color = Color.white;

    [SyncObject]
    public readonly SyncList<Card> stash = new SyncList<Card>();

    [SyncVar]
    public int coins;

    [SyncVar]
    public int score;

    public int thrownCards;

    public override void OnStartServer(){
        base.OnStartServer();
        GameManager.Instance.players.Add(this);
    }

    public override void OnStopServer(){
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
    }

    public override void OnStartClient(){
        base.OnStartClient();

        if(!IsOwner) return;
        Instance = this;
        score = 0;
        thrownCards = 0;
        color=Color.white;
        setCardName("cheese");
        addCoin(50);
        ViewManager.Instance.Initialize();
    }

    [ServerRpc(RequireOwnership = false)]
    public void serverSetIsReady(bool value){
        isReady = value;
    }

    [TargetRpc]
    public void targetSetSheriff(NetworkConnection networkConnection, bool isSheriff){
        this.isSheriff = isSheriff;
    }

    [ServerRpc (RequireOwnership = false)]
    public void startGame(){
        targetStartGame(Owner);
    }

    [TargetRpc]
    private void targetStartGame(NetworkConnection networkConnection){
        ViewManager.Instance.Show<GameView>();
        UIManager.Instance.startGame();
    }

    [ServerRpc (RequireOwnership = false)]
    public void endGame(){
        setScore();
        targetEndGame(Owner);
    }

    [TargetRpc]
    private void targetEndGame(NetworkConnection networkConnection){
        ViewManager.Instance.Show<EndView>();
        UIManager.Instance.endGame();
    }

    [ServerRpc (RequireOwnership = false)]
    public void startTurn(){
        targetStartTurn(Owner);
    }

    [TargetRpc]
    private void targetStartTurn(NetworkConnection networkConnection){
        UIManager.Instance.startTurn();
    }

    [ServerRpc (RequireOwnership = false)]
    public void endTurn(){
        targetEndTurn(Owner);
    }

    [TargetRpc]
    private void targetEndTurn(NetworkConnection networkConnection){
        UIManager.Instance.endTurn();
    }

    [ServerRpc (RequireOwnership = false)]
    public void goSackOpen(){
        targetGoSackOpen(Owner);
    }

    [TargetRpc]
    private void targetGoSackOpen(NetworkConnection networkConnection){
        ViewManager.Instance.Show<SheriffView>();
        UIManager.Instance.goSackOpen();
    }

    [ServerRpc (RequireOwnership = false)]
    public void setUsername( string username){
        this.username = username;
    }

    [ServerRpc (RequireOwnership = false)]
    public void addToSack(Card card){
        sack.Add(card);
    }

    [ServerRpc (RequireOwnership = false)]
    public void setCardName(string name){
        this.cardName = name;
    }

    [ServerRpc (RequireOwnership = false)]
    public void addCoin(int amount){
        coins += amount;
    }

    [ServerRpc (RequireOwnership = false)]
    public void takeCoin(int amount){
        coins -= amount;
    }

    [ServerRpc (RequireOwnership = false)]
    public void addStash(Card card){
        stash.Add(card);
    }

    private NetworkedSack findSack(Player player){
        foreach (NetworkedSack sack in SackContainer.Instance.localSacks){
            if (sack.owner == player){
                return sack;
            }
        }
        return null;
    }

    public void offer(Player player, int value){
        targetOffer(Owner, player, value);
    }

    [TargetRpc]
    private void targetOffer(NetworkConnection networkConnection, Player player, int value){
        findSack(player).showOffer(value);
    }
    public void deactivateHaggleMenu(){
        targetDeactivateHaggleMenu(Owner);
    }

    [TargetRpc]
    public void targetDeactivateHaggleMenu(NetworkConnection networkConnection){
        if(HaggleMenu.Instance){
            HaggleMenu.Instance.gameObject.SetActive(false);
        }
    }

    [ServerRpc (RequireOwnership = false)]
    private void setScore(){
        score = coins;
        foreach (Card card in stash){
            score += card.value;
        }
    }
}
