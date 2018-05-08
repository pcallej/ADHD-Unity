using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour {

    public PathEditor pathToFollow;
    public int currentWayPointID = 0;
    [Range(0f,5f)]
    public float speed;
    private float reachDistance = 0.3f;
    public float rotationSpeed = 5.0f;
    public string pathName;

    private Vector3 initPos, lastPos, currentPos;
    private Quaternion initRot;
    [SerializeField] private GameObject explosion;
    public GameObject master;
    
	void Start ()
    {
        //pathToFollow = GameObject.Find(pathName).GetComponent<PathEditor>();
        lastPos = transform.position;
        initPos = transform.position;
        initRot = transform.rotation;
	}
	
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

    public void TrainCrashed() {
        Instantiate(explosion, gameObject.transform.position,gameObject.transform.rotation);
        StartCoroutine(WaitSeconds(2));
        gameObject.transform.position = initPos;
        currentPos = initPos;
        gameObject.transform.rotation = initRot;
        currentWayPointID = 0;
    }

    public void TrainSuccess() {
        gameObject.transform.position = initPos;
        currentPos = initPos;
        gameObject.transform.rotation = initRot;
        currentWayPointID = 0;
        int newScore = master.GetComponent<TrainCount>().score++;
        master.GetComponent<TrainCount>().UpdateScore(newScore);
    }

    private IEnumerator WaitSeconds(int numSeconds) {
        yield return new WaitForSeconds(numSeconds);
    }
}
