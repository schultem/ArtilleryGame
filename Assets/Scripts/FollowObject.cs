using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour {
    public GameObject follow;
    private Vector3 initial_offset;

    void Start ()
    {
        initial_offset = follow.transform.position - gameObject.transform.position;
    }

	void Update () {
        gameObject.transform.position = follow.transform.position - initial_offset;

    }
}
