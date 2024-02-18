using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingChallengeManager : MonoBehaviour
{
    [System.Serializable]
    public class RoomChallenge
    {
        public List<GameObject> targets;
        public GameObject door;
    }

    public List<RoomChallenge> challenges; // Assign in the Inspector

    public void TargetHit(GameObject target)
    {
        foreach (RoomChallenge challenge in challenges)
        {
            if (challenge.targets.Contains(target))
            {
                target.SetActive(false); // Deactivate the target
                CheckRoomTargets(challenge);
                break;
            }
        }
    }

    void CheckRoomTargets(RoomChallenge challenge)
    {
        foreach (GameObject target in challenge.targets)
        {
            if (target.activeSelf) return;
        }
        challenge.door.SetActive(false); // Open the door when all targets are hit
    }
}
