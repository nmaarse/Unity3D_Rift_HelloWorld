using System;
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GUIText winText;
    public GUIText timerText;
    private int count;
    private int numberOfGameObjects;

    public AudioSource pickupAudio;
    public AudioSource finishedAudio;
    public AudioSource backgroundAudio;

    public int gameTimeToFinishInSeconds;
    private DateTime timeStarted;
    private PlayerState state;
    private bool gameFinished = false;
    private float initialDistanceToGround;
    private Vector3 initialPosition;

    private GameObject[] pickupObjects;

    public float jumpforce;
    void Start()
    {
        jumpforce = 250.0f;
        gameTimeToFinishInSeconds = 30;
        count = 0;
        winText.text = "";
        numberOfGameObjects = GameObject.FindGameObjectsWithTag("PickUp").Length;
        state = PlayerState.WaitToStart;
        initialPosition = transform.position;
        initialDistanceToGround = GetComponent<Collider>().bounds.extents.y;
        pickupObjects = GameObject.FindGameObjectsWithTag("PickUp");
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (state == PlayerState.WaitToStart && movement != new Vector3())
        {
            timeStarted = DateTime.Now;
            state = PlayerState.Playing;
        }

        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(movement * speed * Time.deltaTime);

        CheckInput();
        if (BallOffTheTable())
        {
            state = PlayerState.Lose;
        }
        CheckPlayerState();

    }

    private void CheckPlayerState()
    {
        switch (state)
        {
            case PlayerState.WaitToStart:
                break;
            case PlayerState.Win:
                winText.text = "YES, YOU MAKE PAYMENT HAPPEN!";
                timerText.text = "";
                break;
            case PlayerState.Lose:
                winText.text = "YOU FAILED TO MAKE PAYMENT HAPPEN!";
                timerText.text = "";
                break;
            case PlayerState.Playing:
                winText.text = "";
                CheckTimer();
                break;
        }

        if (!gameFinished && (state == PlayerState.Win || state == PlayerState.Lose))
        {
            ShowPickupObjects(false);

            gameFinished = true;
            backgroundAudio.Stop();
            finishedAudio.Play();
        }
    }

    private void ShowPickupObjects(bool show)
    {
        Debug.LogWarning("ShowPickupObjects: " + show.ToString());
        foreach (var pickupObject in pickupObjects)
        {
            pickupObject.SetActive(show);
        }
    }


    private void CheckTimer()
    {
        var elapsedSeconds = DateTime.Now.Subtract(timeStarted).TotalSeconds;
        if (elapsedSeconds < gameTimeToFinishInSeconds)
        {
            timerText.text = Convert.ToInt32(gameTimeToFinishInSeconds - elapsedSeconds).ToString();
            return;
        }

        state = PlayerState.Lose;
    }

    private void CheckInput()
    {
        bool pushed = Input.GetButton("Fire1") || Input.GetKeyDown("space");

        if (pushed && (state == PlayerState.Lose || state == PlayerState.Win))
        {
            StartNewGame();
            return;
        }

        var rigidBody = GetComponent<Rigidbody>();
        if (pushed && BallOnTheGround())
        {
            Vector3 movement = new Vector3(0.0f, jumpforce, 10.0f);
            rigidBody.AddForce(movement);
        }
    }

    private void StartNewGame()
    {
        gameFinished = false;
        state = PlayerState.WaitToStart;
        ShowPickupObjects(true);
        count = 0;
        transform.position = initialPosition;
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.velocity = new Vector3();
        backgroundAudio.Play();
        winText.text = "";
    }

    private bool BallOnTheGround()
    {
        // compare current position with ground position
        bool grounded = Physics.Raycast(transform.position, -Vector3.up, initialDistanceToGround + 0.1f);
        Debug.LogWarning("grounded:" + grounded);
        return grounded;
    }

    private bool BallOffTheTable()
    {
        if (transform.position.y < initialDistanceToGround - 0.1f)
        {
            return true;
        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count = count + 1;

            if (count >= numberOfGameObjects)
                state = PlayerState.Win;
            else
                pickupAudio.Play();
        }
    }

    private enum PlayerState
    {
        WaitToStart,
        Playing,
        Win,
        Lose
    }
}
