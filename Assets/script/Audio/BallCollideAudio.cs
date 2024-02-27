using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollideAudio : MonoBehaviour
{
    [SerializeField] AudioData metalCollideSFX;
    [SerializeField] AudioData rubberCollideSFX;
    [SerializeField] PickUpController pick;
    [SerializeField] GameObject sparkVFX;
    [SerializeField] GameObject dustVFX;

    void OnCollisionEnter(Collision collision)
    {

        if(pick.IsMetalBall && collision.gameObject.tag != "Ground")
        {
            AudioManager.Instance.PlaySFX(metalCollideSFX);
            sparkVFX.SetActive(true);
        }
        else if(pick.IsMetalBall && collision.gameObject.tag == "Ground")
        {
            AudioManager.Instance.PlaySFX(metalCollideSFX);
            dustVFX.SetActive(true);
        }
        else
        {
            AudioManager.Instance.PlaySFX(rubberCollideSFX);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        sparkVFX.SetActive(false);
        dustVFX.SetActive(false);
    }
}
