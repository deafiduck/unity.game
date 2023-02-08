using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
	public bool dropped = false;

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "drop")
		{
			dropped = true;
		}
	}
}
