using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteAlways]
public class TakeDownPositionController : MonoBehaviour
{
	Transform takeDownAnimStartPos = null;
	/// <summary>Position to which the character needs to be moved to, before playing takedown animation</summary>
	public Transform TakeDownAnimationStartPosition { get { return takeDownAnimStartPos; } }

	[SerializeField] EnemyAnimationController animationController = null;

	// Start is called before the first frame update
	void Start()
    {
		if(!Application.isPlaying)
		{
			ValidateTakeDownAnimationStartPosition();

			if(TakeDownAnimationStartPosition == null)
			{
				Debug.LogError("hideAnimationStartPosition could not be found. Hiding interaction can not function properly." +
					"\nRun \"Revalidate HidingAnimationStartPosition\" from the component's context menu.");
			}
		}
		else
		{
			takeDownAnimStartPos = transform.Find("TakeDownAnimationStartPosition");
		}
	}

	[ContextMenu("Revalidate HidingAnimationStartPosition")]
	void ValidateTakeDownAnimationStartPosition()
	{
		takeDownAnimStartPos = transform.Find("TakeDownAnimationStartPosition");
		if(takeDownAnimStartPos == null)
		{
			GameObject newChild = new GameObject("TakeDownAnimationStartPosition");
			newChild.transform.SetPositionAndRotation(transform.position, transform.rotation);
			newChild.transform.parent = transform;
			newChild.transform.Translate(0.0f, 0.0f, 1.0f);
			takeDownAnimStartPos = newChild.transform;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().TakeDownObject = this;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().TakeDownObject = null;
		}
	}

	public void Die()
	{
		animationController.Die();
		Destroy(transform.parent.gameObject);
		Destroy(animationController.gameObject, 5.0f);
	}

	public void StartTakeDown()
	{
		GetComponentInParent<NavMeshAgent>().isStopped = true;
		transform.parent.parent.GetComponentInChildren<AIController>().Die();
	}
}
