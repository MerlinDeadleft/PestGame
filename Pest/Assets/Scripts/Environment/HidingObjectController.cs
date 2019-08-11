using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HidingObjectController : MonoBehaviour
 {
	public enum HidingObjectType { None, Pipe, Crate, Barrel, Door, Manhole };

	public HidingObjectType hidingObjectType = HidingObjectType.None;
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Manhole)] Animator manholeLid = null;
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Door)] Animator door = null;

	public bool hasFixedHideAnimationStartPoint = false;
	/// <summary>Position to which the character needs to be moved to, before playing hiding animation</summary>
	public Transform HideAnimationStartPosition { get; private set; } = null;
	[MyBox.ConditionalField("hasFixedHideAnimationStartPoint", false)] public float hideAnimationStartDistance = 0.0f;

	public bool hasFixedKillAnimationStartPoint = false;
	/// <summary>Position to which a rnemy NavMeshAgent needs to be moved to, before playing hiding kill animation</summary>
	public Transform KillAnimationStartPosition { get; private set; }
	[MyBox.ConditionalField("hasFixedKillAnimationStartPoint", false)] public float killAnimationStartDistance = 0.0f;

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

			if(hasFixedKillAnimationStartPoint)
			{
				KillAnimationStartPosition = transform.Find("KillAnimationStartPosition");
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

	public void SetAnimatorRotation(Vector3 left)
	{
		if(hidingObjectType == HidingObjectType.Manhole)
		{
			manholeLid.transform.rotation = Quaternion.LookRotation(Vector3.up, left);
		}
	}

	public void AnimatorHideBegin()
	{
		if(hidingObjectType == HidingObjectType.Manhole)
		{
			manholeLid.SetTrigger("Hide");
		}

		if(hidingObjectType == HidingObjectType.Door)
		{
			door.SetTrigger("Hide");
		}
	}

	public void AnimatorHideEnd()
	{
		if(hidingObjectType == HidingObjectType.Manhole)
		{
			manholeLid.SetTrigger("Unhide");
		}

		if(hidingObjectType == HidingObjectType.Door)
		{
			door.SetTrigger("Unhide");
		}
	}

	public void AnimatorDie()
	{
		if(hidingObjectType == HidingObjectType.Manhole)
		{
			manholeLid.SetTrigger("Die");
		}

		if(hidingObjectType == HidingObjectType.Door)
		{
			door.SetTrigger("Die");
		}
	}
}
