using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class AlphaFader : MonoBehaviour
{
	[SerializeField] Sprite mouseSprite = null;
	[SerializeField] Sprite keyboardSprite = null;
	Sprite joystickSprite = null;
	[SerializeField] Sprite xboxSprite = null;
	[SerializeField] Sprite ps4Sprite = null;


	[SerializeField] AnimationCurve alphaCurve = new AnimationCurve();
	[SerializeField] float fadeTime = 5.0f;
	float timer = 0.0f;
	Image imageToFade = null;
	Player player = null;

    // Start is called before the first frame update
    void Start()
    {
		player = ReInput.players.GetPlayer(0);
		imageToFade = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_PS4
		imageToFade.sprite = ps4Sprite;
#endif
		if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Mouse))
		{
			imageToFade.sprite = mouseSprite;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Keyboard))
		{
			imageToFade.sprite = keyboardSprite;
		}
		else if(player.IsCurrentInputSource(RewiredConsts.Action.UIControl.Any, ControllerType.Joystick))
		{
			joystickSprite = xboxSprite;
			imageToFade.sprite = joystickSprite;
		}

		Color col = imageToFade.color;
		col.a = alphaCurve.Evaluate(timer / fadeTime);
		imageToFade.color = col;

		timer += Time.deltaTime;
		if(timer >= fadeTime)
		{
			timer = 0.0f;
		}
    }
}
