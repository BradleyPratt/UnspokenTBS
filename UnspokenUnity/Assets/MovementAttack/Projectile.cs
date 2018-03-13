using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

	Vector3 target, initialPos;
	Vector2 normalizedHorizontalChange;
	float sv, uv, vv, av, tv, sh, uh, vh, ah, th;
	float attackRadius;
	float attackStrength;
	// Use this for initialization
	void Start () {
		uh = vh = 10;
		sh = ah = th = 0;
		sv = 0;
		uv = 10;
		vv = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z)) < 0.1)
		{
			Collider[] colliderArray = Physics.OverlapSphere(target, attackRadius);

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

			Destroy(this.gameObject);
		} else
		{
		}
		th += Time.deltaTime;
		tv += Time.deltaTime;

		sh = uh * th;
		Vector3 tempVector3 = initialPos;
		tempVector3.x += (normalizedHorizontalChange * sh).x;
		tempVector3.z += (normalizedHorizontalChange * sh).y;
		transform.position = tempVector3;
	}

	public void SetTarget(Vector3 newTarget)
	{
		initialPos = transform.position;
		target = newTarget;
		normalizedHorizontalChange = new Vector2(target.x - initialPos.x, target.z - initialPos.z);
		normalizedHorizontalChange.Normalize();
	}

	public void SetAttackRadius(float newRadius)
	{
		attackRadius = newRadius;
	}

	public void SetAttackStrength(float newStrength)
	{
		attackStrength = newStrength;
	}

}
