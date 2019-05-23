using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ClimbableObjectController : MonoBehaviour
 {
	Transform climbDownPosition = null;
	/// <summary>Position, that the character needs to move to, to climb down a climbable</summary>
	public Transform ClimbDownPositionTransform { get { return climbDownPosition; } }

	// Use this for initialization
	void Start ()
	{
		if(!Application.isPlaying)
		{
			ValidateClimbDownPosition();

			if(climbDownPosition == null)
			{
				Debug.LogError("ClimbDownPosition could not be found! Climbing interaction can not function properly!" +
					"\nRun \"Revalidate ClimbDownPosition\" from the component's context menu.");
			}
		}
	}

	[ContextMenu("Revalidate ClimbDownPosition")]
	void ValidateClimbDownPosition()
	{
		Transform child = transform.Find("ClimbDownPosition");
		if(child == null)
		{
			GameObject newChild = new GameObject("ClimbDownPosition");
			newChild.transform.SetPositionAndRotation(transform.position, transform.rotation);
			newChild.transform.parent = transform;
			climbDownPosition = newChild.transform;
			climbDownPosition.localScale = Vector3.one;
			climbDownPosition.tag = "ClimbDownPosition";

			BoxCollider box = climbDownPosition.GetComponent<BoxCollider>();
			if(box == null)
			{
				box = climbDownPosition.gameObject.AddComponent<BoxCollider>();
				box.isTrigger = true;
				box.size = new Vector3(1.0f, 1.0f / transform.localScale.y, 1.0f);
			}
		}
		else
		{
			climbDownPosition = child.transform;
		}
	}
}
