using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Object;

public class Sack : NetworkBehaviour
{
    public static Sack Instance {get; private set;}
    public List<Card> cards = new List<Card>();
    public int maxCards = 5;

    public void Update(){
        gameObject.GetComponent<Image>().color = Player.Instance.color;
    }

    public override void OnStartClient(){
        base.OnStartClient();
        Instance = this;
    }
    
}
