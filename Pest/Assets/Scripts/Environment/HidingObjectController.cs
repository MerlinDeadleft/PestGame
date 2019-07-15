using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HidingObjectController : MonoBehaviour
 {
	public enum HidingObjectType { None, Pipe, Crate_or_Barrel, Door, Manhole };

	public HidingObjectType hidingObjectType = HidingObjectType.None;
	public bool hasFixedHideAnimationStartPoint = false;
	/// <summary>Position to which the character needs to be moved to, before playing hiding animation</summary>
	public Transform HideAnimationStartPosition { get; private set; } = null;
	[MyBox.ConditionalField("hasFixedHideAnimationStartPoint", false)] public float hideAnimationStartDistance = 0.0f;
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Manhole)] public GameObject manholeLid = null;
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Manhole)] float lidOpenDistance = 1.0f;
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Manhole)] float lidhalfOpenDistance = 0.25f;

	// Use this for initialization
	void Start ()
	{
		if(!Application.isPlaying)
		{
			if(hasFixedHideAnimationStartPoint)
			{
				ValidateHidingAnimationStartPosition();

				if(HideAnimationStartPosition == null)
				{
					Debug.LogError("hideAnimationStartPosition could not be found. Hiding interaction can not function properly." +
						"\nRun \"Revalidate HidingAnimationStartPosition\" from the component's context menu.");
				}
			}
		}
		else
		{
			if(hasFixedHideAnimationStartPoint)
			{
				HideAnimationStartPosition = transform.Find("HideAnimationStartPosition");
			}
		}
	}

	[ContextMenu("Revalidate HidingAnimationStartPosition")]
	void ValidateHidingAnimationStartPosition()
	{
		HideAnimationStartPosition = transform.Find("HideAnimationStartPosition");
		if(HideAnimationStartPosition == null)
		{
			GameObject newChild = new GameObject("HideAnimationStartPosition");
			newChild.transform.SetPositionAndRotation(transform.position, transform.rotation);
			newChild.transform.parent = transform;
			newChild.transform.Translate(0.0f, 0.0f, 1.0f);
			HideAnimationStartPosition = newChild.transform;
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

	public void MoveManholeCoverHideBegin(Vector3 playerPos)
	{
		StartCoroutine(MoveManholeLidToOpen(playerPos));
	}

	IEnumerator MoveManholeLidToOpen(Vector3 playerPos)
	{
		bool finished = false;

		while(!finished)
		{
			Vector3 directionToMoveIn = Vector3.Normalize(manholeLid.transform.position - playerPos);
			Vector3 pointToMoveTo = transform.position + directionToMoveIn * lidOpenDistance;
			directionToMoveIn = pointToMoveTo - manholeLid.transform.position;
			manholeLid.transform.Translate(directionToMoveIn * Time.deltaTime, Space.Self);

			if(Vector3.Magnitude(manholeLid.transform.position - pointToMoveTo) < 0.1f)
			{
				finished = true;
			}

			yield return new WaitForEndOfFrame();
		}

		StartCoroutine(MoveManholeLidToHalfOpen());
	}

	IEnumerator MoveManholeLidToHalfOpen()
	{
		bool finished = false;

		while(!finished)
		{
			Vector3 directionToMoveIn = Vector3.Normalize(transform.position - manholeLid.transform.position);
			Vector3 pointToMoveTo = transform.position + directionToMoveIn * lidOpenDistance;
			directionToMoveIn = pointToMoveTo - manholeLid.transform.position;
			manholeLid.transform.Translate(-directionToMoveIn * Time.deltaTime, Space.Self);

			if(Vector3.Magnitude(manholeLid.transform.position - pointToMoveTo) < 0.1f)
			{
				finished = true;
			}

			yield return new WaitForEndOfFrame();
		}
	}

	public void MoveManholeCoverHideEnd()
	{

	}
}
