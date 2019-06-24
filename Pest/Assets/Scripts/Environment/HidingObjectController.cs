using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HidingObjectController : MonoBehaviour
 {
	Transform hideAnimStartPos = null;
	/// <summary>Position to which the character needs to be moved to, before playing hiding animation</summary>
	public Transform HideAnimationStartPosition { get { return hideAnimStartPos; } }

	// Use this for initialization
	void Start ()
	{
		if(!Application.isPlaying)
		{
			ValidateHidingAnimationStartPosition();

			if(hideAnimStartPos == null)
			{
				Debug.LogError("hideAnimationStartPosition could not be found. Hiding interaction can not function properly." +
					"\nRun \"Revalidate HidingAnimationStartPosition\" from the component's context menu.");
			}
		}
		else
		{
			hideAnimStartPos = transform.Find("HideAnimationStartPosition");
		}
	}

	[ContextMenu("Revalidate HidingAnimationStartPosition")]
	void ValidateHidingAnimationStartPosition()
	{
		hideAnimStartPos = transform.Find("HideAnimationStartPosition");
		if(hideAnimStartPos == null)
		{
			GameObject newChild = new GameObject("HideAnimationStartPosition");
			newChild.transform.SetPositionAndRotation(transform.position, transform.rotation);
			newChild.transform.parent = transform;
			newChild.transform.Translate(0.0f, 0.0f, 1.0f);
			hideAnimStartPos = newChild.transform;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().HidingObject = this;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().HidingObject = null;
		}
	}
}
