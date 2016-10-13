using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GUIText winText;
    private int count;
    private int numberOfGameObjects;
    public AudioSource pickupAudio;
    public AudioSource finishedAudio;
    public AudioSource backgroundAudio;

    void Start()
    {
        count = 0;
        SetCountText();
        winText.text = "";
        numberOfGameObjects = GameObject.FindGameObjectsWithTag("PickUp").Length;
    }
     
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.AddForce(movement * speed * Time.deltaTime);

        CheckXboxInput();
	}

    private void CheckXboxInput()
    {
        bool pushed = Input.GetButton("Fire1");
        Debug.LogWarning("PlayerController xboxcontroller button Fire 1 pushed: " + pushed );
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
            PlayAudio();
        }
    }

    void PlayAudio()
    {
        if (count >= numberOfGameObjects)
        {
            backgroundAudio.Stop();
            finishedAudio.Play();
        }
        else
        {
            pickupAudio.Play();
        }
    }

    void SetCountText()
     {
        if (count >= numberOfGameObjects)
        {
            winText.text = "YOU MAKE PAYMENT HAPPEN!"; 
        } 
    }
}
