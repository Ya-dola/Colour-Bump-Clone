using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishController : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider)
    {
        PlayerBallController playerBallController = collider.GetComponent<PlayerBallController>();

        if (!playerBallController || GameManager.singleton.GameEnded)
            return;

        GameManager.singleton.EndGame(true);
    }
}
