using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.UI;

public class FPSOff : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
#if UNITY_EDITOR
		GetComponent<FPSCounter>().enabled = true;
#else
		GetComponent<FPSCounter>().enabled = false;
		GetComponent<Text>().text = "";
#endif

	}
}
