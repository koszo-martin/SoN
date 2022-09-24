using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;

public class ClientServerManager : MonoBehaviour
{
    [SerializeField] private bool isServer;
    public void Awake()
    { 
        if (isServer) InstanceFinder.ServerManager.StartConnection();
        else InstanceFinder.ClientManager.StartConnection();
    }
}
