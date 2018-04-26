using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class PathEditor : MonoBehaviour {

    private Color trailColor;
    public List<Transform> pathObjects = new List<Transform>();
    Transform[] transformArray;

    void OnDrawGizmos()
    {
        string trailParent = gameObject.transform.parent.name;
        switch (trailParent)
        {
            case "Red":
                Gizmos.color = Color.red;
                break;
            case "Blue":
                Gizmos.color = Color.blue;
                break;
            case "Green":
                Gizmos.color = Color.green;
                break;
            case "Yellow":
                Gizmos.color = Color.yellow;
                break;
        }
        transformArray = GetComponentsInChildren<Transform>();
        pathObjects.Clear();

        foreach (Transform pathObject in transformArray)
        {
            if (pathObject != this.transform)
            {
                pathObjects.Add(pathObject);
            }

            for (int i = 0; i < pathObjects.Count; i++)
            {
                Vector3 currentPosition = pathObjects[i].position;
                if (i > 0)
                {
                    Vector3 previous = pathObjects[i - 1].position;
                    Gizmos.DrawLine(previous, currentPosition);
                    Gizmos.DrawWireSphere(currentPosition,0.1f);
                }
            }
        }
    }
}
