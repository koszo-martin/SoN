using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class MultiplayerMenuView : View
{
    [SerializeField]
    private Button hostButton;
    [SerializeField]
    private Button connectButton;
    [SerializeField]
    private Button exitButton;

    public override void Initialize(){
        hostButton.onClick.AddListener(() => {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        connectButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());
        exitButton.onClick.AddListener(Application.Quit);
        base.Initialize();
    }
}
