using UnityEngine;
using System.Collections;

public class DestroyFirstCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        { 
            Destroy(collision.gameObject);
        }
        else
        {
            Destroy(gameObject, 20);
        }
    }
}
