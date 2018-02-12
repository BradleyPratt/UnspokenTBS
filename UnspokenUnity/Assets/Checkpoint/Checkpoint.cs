﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public float radius = 200;

    bool neutralCheckpointBuilt = false;
    bool redCheckpointBuilt = false;
    bool blueCheckpointBuilt = false;

    public GameObject checkpointRed;
    public GameObject checkpointBlue;
    public GameObject checkpointNeutral;

    void Start()
    {
        /*checkpointNeutral = (GameObject)Resources.Load("Assets/Resources/Checkpoint/ControlPointNeutral");
        checkpointRed = (GameObject)Resources.Load("Assets/Resources/Checkpoint/ControlPointUSSR");
        checkpointBlue = (GameObject)Resources.Load("Assets/Resources/Checkpoint/ControlPointUS");*/
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
            Debug.Log("Zone is neutral");
            if (!neutralCheckpointBuilt) {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(checkpointNeutral, transform);
            }
            neutralCheckpointBuilt = true;
            redCheckpointBuilt = false;
            blueCheckpointBuilt = false;
        }
        else if (j > k)
        {
            Debug.Log("The zone belongs to the US");
            if (!blueCheckpointBuilt) {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(checkpointBlue, transform);
            }
            blueCheckpointBuilt = true;
            redCheckpointBuilt = false;
            neutralCheckpointBuilt = false;
        }
        else {
            Debug.Log("The zone belongs to the USSR");
            if (!redCheckpointBuilt) {
                foreach (Transform child in transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(checkpointRed, transform);
            }
            redCheckpointBuilt = true;
            blueCheckpointBuilt = false;
            neutralCheckpointBuilt = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
