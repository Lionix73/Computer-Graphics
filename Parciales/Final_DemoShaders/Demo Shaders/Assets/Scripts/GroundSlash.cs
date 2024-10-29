using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSlash : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    [SerializeField] private float slowDownRate = 0.1f;
    [SerializeField] private float detectingDistance = 0.1f;
    [SerializeField] private float destroyDelay = 5f;

    private Rigidbody rb;
    private bool stopped;

    void Start()
    {
        transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
        transform.rotation = Quaternion.Euler(0, 180, 0);

        if(GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
            StartCoroutine(SlowDown());
        }
        else
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        if(!stopped)
        {
            RaycastHit hit;
            Vector3 distance = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
            Debug.DrawRay(distance, transform.TransformDirection(-Vector3.up * detectingDistance), Color.red);
            if(Physics.Raycast(distance, transform.TransformDirection(-Vector3.up * detectingDistance), out hit, detectingDistance))
            {
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            else{
                transform.position = new Vector3(transform.position.x, 0.0f, transform.position.z);
            }

            Destroy(gameObject, destroyDelay);
        }
    }

    private IEnumerator SlowDown()
    {
        float t = 1;
        while(t > 0)
        {
            rb.velocity = Vector3.Lerp(Vector3.zero, rb.velocity, t);
            t -= slowDownRate * Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }

        stopped = true;
    }

    public float Speed{
        get { return speed; }
        set { speed = value; }
    }
}
