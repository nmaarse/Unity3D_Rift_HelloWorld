using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class MovableCamera : MonoBehaviour
{

    public int sensitivity = 35;
    public GameObject player;
    private Vector3 offset;
    public GUIText winText;
    // Use this for initialization
    void Start()
    {
        offset = transform.position;
        Debug.LogWarning("MovableCamera script started ");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = InputTracking.GetLocalPosition(VRNode.Head) * sensitivity + offset;
        
//Debug.LogWarning("LateUpdate of Movable Camera: " + transform.position);
    }
     void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {
            Debug.LogWarning("MovableCamera collision with " + other.gameObject.tag.ToString());
            other.gameObject.SetActive(false);
        }
    }
}
