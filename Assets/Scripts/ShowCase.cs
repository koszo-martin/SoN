using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class ShowCase : NetworkBehaviour
{
    [field: SyncObject, SerializeField]
    public readonly SyncList<Card> cards = new SyncList<Card>();
}
