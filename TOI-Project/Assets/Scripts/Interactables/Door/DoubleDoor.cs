using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleDoor : MonoBehaviour
{
    public Door leftDoor;
    public Door rightDoor;


    public void Interaction()
    {
        leftDoor.Interaction();
        rightDoor.Interaction();
    }
}
