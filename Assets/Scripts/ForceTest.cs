using UnityEngine;
using System.Collections;

public class ForceTest : MonoBehaviour {
    public float x, y, z;
	void Start () {
        GetComponent<Rigidbody>().AddForce(new Vector3(x,y,z));
	}
}
