using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public float radius = 200;

    float counter;

    bool neutralCheckpointBuilt = false;
    bool redCheckpointBuilt = false;
    bool blueCheckpointBuilt = false;

    GameObject checkpointRed;
    GameObject checkpointBlue;
    GameObject checkpointNeutral;

	void Start()
	{
		GetComponentInChildren<Projector>().orthographicSize = radius;

        checkpointRed = (GameObject)Resources.Load("ControlPointUSSR");
        checkpointBlue = (GameObject)Resources.Load("ControlPointUS");
        checkpointNeutral = (GameObject)Resources.Load("ControlPointNeutral");
    }
	// Update is called once per frame
	void Update () {
        Colliding(transform.position, radius);
    }

    void Colliding(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int j = 0;
        int k = 0;

		foreach (Collider hitCollider in hitColliders)
		{
			if (hitCollider.GetComponent<Unit>() != null)
			{
				if (hitCollider.GetComponent<Unit>().GetTeam() == "USA")
				{
					j++;
				} else if (hitCollider.GetComponent<Unit>().GetTeam() == "USSR")
				{
					k++;
				}
			}
		}

        if ((j == 0 && k == 0) || j == k)
        {
            if (!neutralCheckpointBuilt) {
                foreach (Transform child in transform)
                {
					if(!child.name.Equals("RangeProjector"))
					{
						GameObject.Destroy(child.gameObject);
					}
                }
                Instantiate(checkpointNeutral, transform);
            }
            neutralCheckpointBuilt = true;
            redCheckpointBuilt = false;
            blueCheckpointBuilt = false;
        }
        else if (j > k)
        {
            if (!blueCheckpointBuilt) {
                foreach (Transform child in transform)
				{
					if (!child.name.Equals("RangeProjector"))
					{
						GameObject.Destroy(child.gameObject);
					}
				}
                Instantiate(checkpointBlue, transform);
            }
            blueCheckpointBuilt = true;
            redCheckpointBuilt = false;
            neutralCheckpointBuilt = false;
        }
        else {
            if (!redCheckpointBuilt) {
                foreach (Transform child in transform)
				{
					if (!child.name.Equals("RangeProjector"))
					{
						GameObject.Destroy(child.gameObject);
					}
				}
                Instantiate(checkpointRed, transform);
            }
            redCheckpointBuilt = true;
            blueCheckpointBuilt = false;
            neutralCheckpointBuilt = false;
        }
    }

    public string GetCheckpointOwner()
    {
        if (redCheckpointBuilt) {
            return "USSR";
        } else if (blueCheckpointBuilt) {
            return "USA";
        } else return "Neutral";
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
