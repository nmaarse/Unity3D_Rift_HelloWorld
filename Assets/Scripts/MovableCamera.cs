using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class MovableCamera : MonoBehaviour
{

    public int sensitivity = 20;
    public GameObject player;
    private Vector3 offset;
    public GUIText winText;
    // Use this for initialization
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //InputTracking.GetLocalPosition(VRNode.Head);
        transform.position = InputTracking.GetLocalPosition(VRNode.Head) * sensitivity + offset;
        winText.text = "position:" + transform.position;
    }


}
