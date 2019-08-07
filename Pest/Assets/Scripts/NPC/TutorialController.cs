using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
	Animator animator = null;
	[SerializeField] TutorialTextController textController = null;

	[SerializeField] List<string> tutorialText = new List<string>();
	int currentTutorialText = 0;

    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
    }

	public bool ShowTutorial()
	{
		if(currentTutorialText >= tutorialText.Count)
		{
			currentTutorialText = 0;
			textController.enabled = false;
			return false;
		}

		if(textController.enabled == false)
		{
			textController.enabled = true;
		}

		animator.SetTrigger(Animator.StringToHash("Talk"));
		textController.TutorialText.text = tutorialText[currentTutorialText];
		currentTutorialText++;
		return true;
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().TutorialController = this;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			other.GetComponent<PlayerController>().TutorialController = null;
		}
	}
}
