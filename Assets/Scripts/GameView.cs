using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private Image progressBarFill;

    private float lastBarValue;

    // Update is called once per frame
    void Update()
    {
        // Checking if game running
        if (!GameManager.singleton.GameStarted)
            return;

        float travelledDistance = GameManager.singleton.EntireDistance - GameManager.singleton.RemainingDistance;
        float barValue = travelledDistance / GameManager.singleton.EntireDistance;

        if (GameManager.singleton.GameEnded && barValue < lastBarValue)
            return;

        progressBarFill.fillAmount = Mathf.Lerp(progressBarFill.fillAmount, barValue, 5 * Time.deltaTime);

        lastBarValue = barValue;
    }
}
