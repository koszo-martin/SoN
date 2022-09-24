using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FishNet;

public class PreGameView : View
{
    [SerializeField]
    private TMP_Text playerList;

    [SerializeField]
    private Button startButton;

    [SerializeField]
    private Button readyButton;

    [SerializeField]
    private TMP_Text readyButtonText;

    [SerializeField]
    private InputField nameInput;

    public override void Initialize(){
        readyButton.onClick.AddListener(() => {
            Player.Instance.isReady = !Player.Instance.isReady;
        });
        if(!InstanceFinder.IsServer){
            startButton.gameObject.SetActive(false);
        }
        nameInput.onEndEdit.AddListener((value) => {
            Player.Instance.setUsername(value);
        });
        if(GameManager.Instance.colors.Count<6){
            GameManager.Instance.initColors();
        }
        
        base.Initialize();
    }

    public void Update(){
        if (!isInitialized) return;
        string playersText = "";
        foreach (Player player in GameManager.Instance.players){
            playersText += "\r\n" + player.username + " Is Ready:" + player.isReady;
        }
        playerList.text=playersText;
        startButton.interactable = GameManager.Instance.canStart;
        readyButton.interactable = Player.Instance.username != "";
        readyButtonText.color = Player.Instance.isReady ? Color.green : Color.red;
    }
}
