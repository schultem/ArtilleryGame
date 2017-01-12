using UnityEngine;
using System.Collections;

public class TrebuchetController : MonoBehaviour {
    public GameObject arm, counterweight, projectile, projectile_position;
    public int max_tension, initial_tension, input_increment, current_tension, release_angle;
    private Quaternion initial_rotation_arm, initial_rotation_counterweight;
    private Vector3 initial_position_arm, initial_position_counterweight;
    private GameObject loaded_projectile;
    private bool ready_to_fire;

	void Start () {
        initial_rotation_arm = arm.GetComponent<Transform>().localRotation;
        initial_position_arm = arm.GetComponent<Transform>().localPosition;
        initial_rotation_counterweight = counterweight.GetComponent<Transform>().localRotation;
        initial_position_counterweight = counterweight.GetComponent<Transform>().localPosition;

        Reset();
	}

    void Update()
    {
        if (Input.GetMouseButton(0))
            StartCoroutine("Fire");
        else if (Input.GetMouseButton(1) && !ready_to_fire)
            Reset();
        else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            RotateRight();
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            RotateLeft();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && ready_to_fire)
            PullTension();
    }

    void Reset()
    {
        arm.GetComponent<Rigidbody>().useGravity = false;
        arm.GetComponent<Transform>().localRotation = initial_rotation_arm;
        arm.GetComponent<Transform>().localPosition = initial_position_arm;
        arm.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        arm.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

        counterweight.GetComponent<Transform>().localRotation = initial_rotation_counterweight;
        counterweight.GetComponent<Transform>().localPosition = initial_position_counterweight;

        loaded_projectile = (GameObject)Instantiate(projectile, projectile_position.transform.position, projectile_position.transform.rotation);

        projectile_position.AddComponent<HingeJoint>().connectedBody = loaded_projectile.GetComponent<Rigidbody>();
        projectile_position.GetComponent<HingeJoint>().connectedAnchor = loaded_projectile.transform.position;

        current_tension = initial_tension;
        ready_to_fire = true;
    }

    void PullTension()
    {
        if ( current_tension + input_increment < max_tension )
        {
            current_tension += input_increment;
            arm.GetComponent<Transform>().Rotate(new Vector3(-input_increment, 0, 0));
        }
    }

    IEnumerator Fire()
    {
        arm.GetComponent<Rigidbody>().useGravity = true;
        for (;;)
        {
            if (arm.GetComponent<Transform>().localRotation.eulerAngles.x < 180 && arm.GetComponent<Transform>().localRotation.eulerAngles.x > release_angle)
            {
                Destroy(projectile_position.GetComponent<HingeJoint>());
                loaded_projectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0,projectile.GetComponent<Rigidbody>().mass*current_tension/2, projectile.GetComponent<Rigidbody>().mass * current_tension));
                ready_to_fire = false;
                yield break;
            }
            else
                yield return new WaitForSeconds(.1f);
        }
    }

    void RotateRight()
    {
        transform.Rotate(new Vector3(0, input_increment, 0));
    }

    void RotateLeft()
    {
        transform.Rotate(new Vector3(0, -input_increment, 0));
    }
}
