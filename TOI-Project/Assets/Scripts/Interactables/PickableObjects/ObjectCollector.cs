using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollector : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Item" || other.tag == "Alavanca" || other.tag == "Lamparina") 
        {
            other.SendMessage("ResetObject");
        }
    }
}
