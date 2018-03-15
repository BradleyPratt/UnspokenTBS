using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
	private List<GameObject> ignoreColliders = new List<GameObject>();
	Vector3 target, initialPos;
	Vector2 normalizedHorizontalChange;
	float sv, uv, vv, av, tv, sh, uh, vh, ah, th;
	float attackRadius;
	float attackStrength;
	bool collisionOn = false;

	public void SetInfo(Vector3 target, float radius, float strength, GameObject creator)
	{
		SetTarget(target);
		attackRadius = radius;
		attackStrength = strength;
		ignoreColliders.Add(creator);
		uh = vh = 10;
		sh = ah = th = 0;
		sv = 0;
		uv = 10;
		vv = 0;
		collisionOn = true;
	}

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(target.x, target.z)) < 0.1)
		{
			Detonate();
		}
		else
		{
		}
		th += Time.deltaTime;
		tv += Time.deltaTime;

		sh = uh * th;
		Vector3 tempVector3 = new Vector3();//initialPos;
		tempVector3.x = (normalizedHorizontalChange * sh).x;
		tempVector3.z = (normalizedHorizontalChange * sh).y;
		transform.Translate(tempVector3, Space.World);
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

	public void IgnoreObject(GameObject objectToIgnore)
	{
		ignoreColliders.Add(objectToIgnore);
	}

	private void Detonate()
	{

		Collider[]
		colliderArray = Physics.OverlapSphere(transform.position, attackRadius);

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
	}

	void OnCollisionEnter(Collision col)
	{
		if (!ignoreColliders.Contains(col.gameObject))
		{
			Detonate();
		}
	}
}
