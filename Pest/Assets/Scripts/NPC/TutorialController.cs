using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using System;

public class TutorialController : MonoBehaviour
{
	Player player = null;

	Animator animator = null;
	[SerializeField] TutorialTextController textController = null;

	[SerializeField] List<string> tutorialText = new List<string>();
	int currentTutorialText = 0;

    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
		player = ReInput.players.GetPlayer(0);
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
		textController.TutorialText.text = SetTutorialText(); ;
		currentTutorialText++;
		return true;
	}

	string SetTutorialText()
	{
		string s = tutorialText[currentTutorialText];

		if(s.Contains("<magicbutton>"))
		{
			s = s.Replace("<magicbutton>", GetMagicButton());
		}

		if(s.Contains("<sneakbutton>"))
		{
			s = s.Replace("<sneakbutton>", GetSneakButton());
		}

		if(s.Contains("<runbutton>"))
		{
			s = s.Replace("<runbutton>", GetRunButton());
		}

		if(s.Contains("<attackbutton>"))
		{
			s = s.Replace("<attackbutton>", GetAttackButton());
		}

		return s;
	}

	private string GetAttackButton()
	{
		if(player.IsCurrentInputSource(RewiredConsts.Action.TutorialControl.Continue, ControllerType.Joystick))
		{
			return "<sprite index=16>";
		}
		else
		{
			return "<sprite index=13>";
		}
	}

	private string GetRunButton()
	{
		if(player.IsCurrentInputSource(RewiredConsts.Action.TutorialControl.Continue, ControllerType.Joystick))
		{
			return "<sprite index=6>";
		}
		else
		{
			return "<sprite index=9>";
		}
	}

	private string GetSneakButton()
	{
		if(player.IsCurrentInputSource(RewiredConsts.Action.TutorialControl.Continue, ControllerType.Joystick))
		{
			return "<sprite index=7>";
		}
		else
		{
			return "<sprite index=2>";
		}
	}

	private string GetMagicButton()
	{
		if(player.IsCurrentInputSource(RewiredConsts.Action.TutorialControl.Continue, ControllerType.Joystick))
		{
			return "<sprite index=12>";
		}
		else
		{
			return "<sprite index=3>";
		}
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
