using UnityEngine;
using System.Collections;

public class StopCameraOnCollision : MonoBehaviour {
    private Vector3 stop_position;
    private Quaternion stop_rotation;
    public GameObject trigger_object;
    private bool EnableStop = false;

    void Update()
    {
        if (EnableStop)
        {
            GetComponent<Transform>().position = stop_position;
            GetComponent<Transform>().rotation = stop_rotation;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!EnableStop && other.name == trigger_object.name)
        {
            stop_position = GetComponent<Transform>().position;
            stop_rotation = GetComponent<Transform>().rotation;
            EnableStop = true;
        }
    }
}
