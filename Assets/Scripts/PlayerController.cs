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
    private bool audioFinished = false;

    void Start()
    {
        count = 0;
        winText.text = "";
        numberOfGameObjects = GameObject.FindGameObjectsWithTag("PickUp").Length;
        state = PlayerState.WaitToStart;
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

        CheckXboxInput();
        CheckPlayerState();
    }

    private void CheckPlayerState()
    {
        switch (state)
        {
            case PlayerState.WaitToStart:
                break;
            case PlayerState.Win:
                winText.text = "YOU MAKE PAYMENT HAPPEN!";
                timerText.text = "";
                break;
            case PlayerState.Lose:
                winText.text = "TOO SLOW TO MAKE PAYMENT HAPPEN!";
                timerText.text = "";
                break;
            case PlayerState.Playing:
                CheckTimer();
                break;
        }

        if (!audioFinished && (state == PlayerState.Win || state == PlayerState.Lose))
        {
            audioFinished = true;
            backgroundAudio.Stop();
            finishedAudio.Play();
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

        foreach (var pickupObject in GameObject.FindGameObjectsWithTag("PickUp"))
        {
            pickupObject.SetActive(false);
        }

        state = PlayerState.Lose;
    }

    private void CheckXboxInput()
    {
        bool pushed = Input.GetButton("Fire1");
        Debug.LogWarning("PlayerController xboxcontroller button Fire 1 pushed: " + pushed);
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
