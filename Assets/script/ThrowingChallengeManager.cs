using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingChallengeManager : MonoBehaviour
{
    public List<GameObject> targets; // Assign all your target objects here in the Inspector
    public GameObject door; // Assign the door or cube object that should disappear/move
    public void TargetHit()
    {
        foreach (GameObject target in targets)
        {
            if (target.activeSelf)
            {
                return;
            }
        }

        OpenDoor();
    }

    void OpenDoor()
    {
        door.SetActive(false);
    }
}
