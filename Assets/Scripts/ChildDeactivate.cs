using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChildDeactivate : MonoBehaviour
{
    void Update()
    {
        if (gameObject.GetComponent<Button>().interactable == false)
        {
            foreach (Transform child in transform)
            {
                GameObject childObject = child.gameObject;
                if (childObject.GetComponent<Button>() != null)
                {
                    childObject.SetActive(false);
                }
            }
        }

    }
}
