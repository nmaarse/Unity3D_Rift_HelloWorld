using UnityEngine;
using System.Collections;

public class PickUpController : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MovableCamera")
        {
            this.gameObject.SetActive(false);            
        }
    }
}
