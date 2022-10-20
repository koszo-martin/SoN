using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = "Coins: " + Player.Instance.coins;
    }
}
