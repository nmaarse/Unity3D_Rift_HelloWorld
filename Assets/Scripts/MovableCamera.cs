using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class MovableCamera : MonoBehaviour {

    public GameObject player;
    private Vector3 offset;
    // Use this for initialization
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //InputTracking.GetLocalPosition(VRNode.Head);
        transform.position = InputTracking.GetLocalPosition(VRNode.Head) + offset;
    }

    
}
