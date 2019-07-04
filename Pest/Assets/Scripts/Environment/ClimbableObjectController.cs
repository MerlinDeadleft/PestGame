using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ClimbableObjectController : MonoBehaviour
 {
	[SerializeField] bool hasClimbDownPosition = false;
	/// <summary>Position, that the character needs to move to, to climb down a climbable</summary>
	public Transform ClimbDownPosition { get; private set; } = null;

	// Use this for initialization
	void Start ()
	{
		if(hasClimbDownPosition)
		{
			ValidateClimbDownPosition();

			if(!Application.isPlaying)
			{
				if(ClimbDownPosition == null)
				{
					Debug.LogError("ClimbDownPosition could not be found! Climbing interaction can not function properly!" +
						"\nRun \"Revalidate ClimbDownPosition\" from the component's context menu.");
				}
			}
			else
			{
				if(ClimbDownPosition == null)
				{
					Debug.LogError("ClimbDownPosition could not be found! Climbing interaction can not function properly!");
				}
			}
		}
		else
		{
			Transform climbDownPos = transform.Find("ClimbDownPosition");

			if(climbDownPos != null)
			{
				DestroyImmediate(climbDownPos.gameObject);
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
			ClimbDownPosition = newChild.transform;
			ClimbDownPosition.localScale = Vector3.one;
			ClimbDownPosition.tag = "ClimbDownPosition";

			BoxCollider box = ClimbDownPosition.GetComponent<BoxCollider>();
			if(box == null)
			{
				box = ClimbDownPosition.gameObject.AddComponent<BoxCollider>();
				box.isTrigger = true;
				box.size = new Vector3(1.0f, 1.0f / transform.localScale.y, 1.0f);
			}

			ClimbDownPosition.gameObject.AddComponent<ClimbDownController>();
		}
		else
		{
			ClimbDownPosition = child.transform;
			ClimbDownController climbDownController = ClimbDownPosition.GetComponent<ClimbDownController>();
			if(climbDownController == null)
			{
				ClimbDownPosition.gameObject.AddComponent<ClimbDownController>();
			}
		}
	}

	IEnumerator LateValidateClimbDownPosition()
	{
		yield return new WaitForEndOfFrame();
		ValidateClimbDownPosition();
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if(player.IsClimbing)
			{
				player.climbableObject = this;
			}

			if(!player.IsClimbing)
			{
				player.StartClimbing(this);
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			PlayerController player = other.GetComponent<PlayerController>();

			if(player.climbableObject == this)
			{
				player.EndClimb();
			}
		}
	}

	private void OnValidate()
	{
		if(hasClimbDownPosition)
		{
			if(gameObject.activeInHierarchy)
			{
				StartCoroutine(LateValidateClimbDownPosition());
			}
		}
		else
		{
			Transform climbDownPos = transform.Find("ClimbDownPosition");

			if(climbDownPos != null && climbDownPos.gameObject.activeInHierarchy)
			{
				StartCoroutine(DestroyGameObject(climbDownPos.gameObject));
			}
		}
	}

	IEnumerator DestroyGameObject(GameObject go)
	{
		yield return new WaitForEndOfFrame();
		DestroyImmediate(go);
	}
}
