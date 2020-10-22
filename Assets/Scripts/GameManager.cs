using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public bool GameStarted { get; private set; }
    public bool GameEnded { get; private set; }

    [SerializeField] private float slowMotionFactor = 0.1f;
    [SerializeField] private float deltaTime = 0.02f;
    [SerializeField] private int transitionTime = 3;

    [SerializeField] private Transform startTransform;
    [SerializeField] private Transform finishTransform;
    [SerializeField] private PlayerBallController playerBallController;

    public float EntireDistance { get; private set; }
    public float RemainingDistance { get; private set; }

    private void Start()
    {
        EntireDistance = finishTransform.position.z - startTransform.position.z;
    }

    private void Update()
    {
        RemainingDistance = Vector3.Distance(playerBallController.transform.position, finishTransform.position);

        // If Ball is behind start line then the distance is the distance between the start and finish lines 
        if (RemainingDistance > EntireDistance)
            RemainingDistance = EntireDistance;

        // To avoid negative distance being shown to the player if the ball has passed the finish line
        if (playerBallController.transform.position.z > finishTransform.transform.position.z)
            RemainingDistance = 0;
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != this)
            Destroy(gameObject);

        // Specifies Default time flow
        Time.timeScale = 1f;
        Time.fixedDeltaTime = deltaTime;
    }

    public void StartGame()
    {
        GameStarted = true;
        Debug.Log("Game Started !!!");
    }

    public void EndGame(bool gameWon)
    {
        GameEnded = true;

        if (!gameWon)
        {
            AddSlowMotionEffect("RestartGame", transitionTime);

            Debug.Log("Death Obstracle hit !!!");
        }
        else
        {
            Invoke("RestartGame", transitionTime);

            Debug.Log("Finish Line Reached !!!");
        }

        // TODO - Add Text UI Indicating Game has Ended !!!
    }

    public void RestartGame()
    {
        // Scene Numbers are according to those shown in Build
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    private void AddSlowMotionEffect(string method, int slowMotionTime)
    {
        // Calls Method after Specified time
        Invoke(method, slowMotionTime * slowMotionFactor);

        // Creates Slow motion time flow effect 
        Time.timeScale = slowMotionFactor;
        Time.fixedDeltaTime = deltaTime * Time.timeScale;
    }
}
