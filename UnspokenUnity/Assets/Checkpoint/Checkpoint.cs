using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {
    public float radius = 200;
	
	// Update is called once per frame
	void Update () {
        Colliding(transform.position, radius);
    }

    void Colliding(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        int i = 0;
        int j = 0;
        int k = 0;
        while (i < hitColliders.Length)
        {
            if(hitColliders[i].tag == "Unit" && hitColliders[i].name.Contains("USA"))
            {
                j++;
            }
            if (hitColliders[i].tag == "Unit" && hitColliders[i].name.Contains("USSR"))
            {
                k++;
            }
            i++;
        }

        if(j == 0 && k == 0 || j == k)
        {
            Debug.Log("Zone is neutral");
        }
        else if (j > k)
        {
            Debug.Log("The zone belongs to the US");
        }
        else {
            Debug.Log("The zone belongs to the USSR");
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
