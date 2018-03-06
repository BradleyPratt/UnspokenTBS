using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

	private List<GameObject> alreadyHit = new List<GameObject>();

	[SerializeField]
	float attackRange = 50.0f;

	[SerializeField]
	float attackStrength = 50.0f;

	[SerializeField]
	float attackRadius = 50.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CheckForTargets()
	{
		Collider[] potentialTargets = Physics.OverlapSphere(transform.position, attackRange);

		foreach(Collider col in potentialTargets)
		{
			if (col.CompareTag("Unit") && !alreadyHit.Contains(col.gameObject))
			{
				alreadyHit.Add(col.gameObject);
				FireAt(col.gameObject);
			}
		}
	}

	public void FireAt(GameObject target)
	{
		RotateToFace(target.transform.position);
		// todo - switch to physical projectile.

			Collider[] colliderArray = Physics.OverlapSphere(target.transform.position, attackRadius);

			foreach (Collider collider in colliderArray)
			{
				if (collider.gameObject != this.gameObject)
				{
					if (collider.CompareTag("Unit"))
					{
						collider.gameObject.GetComponent<HealthBar>().TakeDamage(attackStrength);
						collider.gameObject.GetComponent<Unit>().UnitHit();
					}
					if (collider.CompareTag("WatchTower"))
					{
						collider.gameObject.GetComponent<WatchTowerHealth>().WatchTowerTakeDamage(attackStrength);
						collider.gameObject.GetComponent<Unit>().UnitHit();
					}
				}
			}
	}

	public void RotateToFace(Vector3 target)
	{

		Vector3 angleTarget = target;
		angleTarget.x = angleTarget.x - transform.position.x;
		angleTarget.z = angleTarget.z - transform.position.z;
		float finalAngle = Mathf.Atan2(angleTarget.z, angleTarget.x) * Mathf.Rad2Deg;
		
		transform.Find("Turret").rotation = Quaternion.Euler(new Vector3(0, -finalAngle, 0));
	}

	public void KillTurret()
	{
		Destroy(this);
	}
}
