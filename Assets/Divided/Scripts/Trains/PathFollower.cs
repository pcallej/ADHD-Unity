using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    public PathEditor pathToFollow;
    public int currentWayPointID = 0;
    [Range(0f,2f)]
    public float speed;
    private float reachDistance = 0.3f;
    public float rotationSpeed = 5.0f;
    public string pathName;

    private Vector3 initPos, lastPos, currentPos;
    [SerializeField] private GameObject explosion;

	// Use this for initialization
	void Start ()
    {
        //pathToFollow = GameObject.Find(pathName).GetComponent<PathEditor>();
        lastPos = transform.position;
        initPos = transform.position;
        explosion = gameObject.transform.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float distance = Vector3.Distance(pathToFollow.pathObjects[currentWayPointID].position,transform.position);
        transform.position = Vector3.MoveTowards(transform.position,pathToFollow.pathObjects[currentWayPointID].position,Time.deltaTime * speed);

        Quaternion goRotation = Quaternion.LookRotation(pathToFollow.pathObjects[currentWayPointID].position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, goRotation, Time.deltaTime * rotationSpeed);

        if (distance <= reachDistance)
        {
            currentWayPointID++;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "crash") {
            Debug.Log("crashed");
            explosion.SetActive(true);
            WaitSeconds(3);
            explosion.SetActive(false);
            currentPos = initPos;
            currentWayPointID = 0;
        }           
    }

    IEnumerator WaitSeconds(int numSeconds) {
        yield return new WaitForSeconds(numSeconds);
    }
}
