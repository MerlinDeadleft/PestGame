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
	[SerializeField, MyBox.ConditionalField("hidingObjectType", HidingObjectType.Manhole)] public GameObject manholeCover = null;

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

	public void MoveManholeCoverHideBegin()
	{

	}

	public void MoveManholeCoverHideEnd()
	{

	}
}
