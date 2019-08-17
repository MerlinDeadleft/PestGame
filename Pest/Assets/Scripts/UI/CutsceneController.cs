using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;
using TMPro;

public class CutsceneController : MonoBehaviour
{
#region structs
	[System.Serializable]
	struct CutsceneData
	{
		[HideInInspector] public string name;

		public Sprite image;
		public List<string> texts;
		[HideInInspector] public int CurrentText;

		public CutsceneDataContent GetCurrentCutsceneDataContent()
		{
			CutsceneDataContent content = new CutsceneDataContent();
			content.Image = image;
			if(texts.Count == 0)
			{
				content.Text = "";
			}
			else
			{
				content.Text = texts[CurrentText];
			}
			content.IsLastImage = false;

			if(CurrentText >= texts.Count - 1)
			{
				content.IsLastText = true;
			}
			else
			{
				content.IsLastText = false;
			}

			return content;
		}

		public CutsceneDataContent GetNextCutsceneDataContent()
		{
			CurrentText++;
			CutsceneDataContent content = GetCurrentCutsceneDataContent();

			//if(CurrentText >= texts.Count - 1)
			//{
			//	content.IsLastText = true;
			//}
			//else
			//{
			//	content.IsLastText = false;
			//}

			return content;
		}
	}

	[System.Serializable]
	struct Cutscene
	{
		[HideInInspector] public string name;

		public CutsceneData[] cutsceneDatas;
		[HideInInspector] public int CurrentImage;
		bool shouldGetNextImage;

		public CutsceneDataContent GetCurrentCutsceneDataContent()
		{
			CutsceneDataContent content = cutsceneDatas[CurrentImage].GetCurrentCutsceneDataContent();
			if(content.IsLastText)
			{
				shouldGetNextImage = true;
			}
			else
			{
				shouldGetNextImage = false;
			}

			return content;
		}

		public CutsceneDataContent GetNextCutsceneDataContent()
		{
			CutsceneDataContent content;

			if(shouldGetNextImage)
			{
				CurrentImage++;
				content = cutsceneDatas[CurrentImage].GetCurrentCutsceneDataContent();
				shouldGetNextImage = content.IsLastText;
			}
			else
			{
				content = cutsceneDatas[CurrentImage].GetNextCutsceneDataContent();
			}

			if(CurrentImage >= cutsceneDatas.Length - 1)
			{
				content.IsLastImage = true;
			}
			else
			{
				content.IsLastImage = false;
			}

			if(content.IsLastText)
			{
				shouldGetNextImage = true;
			}

			return content;
		}
	}

	struct CutsceneDataContent
	{
		public Sprite Image;
		public string Text;
		public bool IsLastImage;
		public bool IsLastText;
	}
	#endregion

	[SerializeField] GameObject loadingIcon = null;
	[SerializeField] Image cutsceneImage = null;
	//[SerializeField] Text cutsceneText = null;
	[SerializeField] TextMeshProUGUI cutsceneText = null;
	[Space]
	[SerializeField] public static string sceneToLoad = "";
	[SerializeField] public static int cutsceneToShow = 0;
	[Space]
	[SerializeField] Cutscene[] cutscenes;

	Player player = null;
	bool cutsceneCoroutineRunning = false;
	public float loadingIconTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
		loadingIconTimer = Random.Range(3.0f, 6.0f);
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		player = ReInput.players.GetPlayer(0);
		player.controllers.maps.SetAllMapsEnabled(false);
		player.controllers.maps.SetMapsEnabled(true, RewiredConsts.Category.LoadingScreenControl);
		player.controllers.maps.SetMapsEnabled(true, RewiredConsts.Category.UIControl);

		for(int i = 0; i < cutscenes.Length; i++)
		{
			cutscenes[i].CurrentImage = 0;

			for(int j = 0; j < cutscenes[i].cutsceneDatas.Length; j++)
			{
				cutscenes[i].cutsceneDatas[j].CurrentText = 0;
			}
		}

		StartCoroutine(ShowCutscene(cutsceneToShow));
		StartCoroutine(LoadScene(sceneToLoad));
    }

	void Update()
	{
		if(loadingIconTimer >= 0.0f)
		{
			loadingIconTimer -= Time.deltaTime;

			if(loadingIconTimer <= 0.0f || !cutsceneCoroutineRunning)
			{
				loadingIcon.SetActive(false);
			}
		}
	}

	IEnumerator LoadScene(string scene)
	{
		yield return new WaitForEndOfFrame(); // wait for a frame, so it is guaranteed that cutscene is showing

		AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
		operation.allowSceneActivation = false;

		while(!operation.isDone)
		{
			if(operation.progress >= 0.9f)
			{
				Debug.Log("Scene Loaded");
				if(!cutsceneCoroutineRunning)
				{
					Debug.Log("Scene can be activated");
					if(player.GetButtonDown(RewiredConsts.Action.LoadingScreenControl.NextIllustration))
					{
						operation.allowSceneActivation = true;
						Cursor.lockState = CursorLockMode.None;
						Cursor.visible = false;
					}
				}
			}

			yield return new WaitForEndOfFrame();
		}

		player.controllers.maps.SetAllMapsEnabled(false);
	}

	IEnumerator ShowCutscene(int cutscene)
	{
		cutsceneCoroutineRunning = true;
		bool showingCutscene = true;

		showingCutscene = StartCutscene(cutscene);

		while(showingCutscene)
		{
			if(player.GetButtonDown(RewiredConsts.Action.LoadingScreenControl.NextIllustration))
			{
				showingCutscene = ContinueCutscene(cutscene);
			}

			yield return new WaitForEndOfFrame();
		}

		cutsceneCoroutineRunning = false;
	}

	bool StartCutscene(int cutscene)
	{
		CutsceneDataContent content = cutscenes[cutscene].GetCurrentCutsceneDataContent();

		cutsceneImage.sprite = content.Image;
		cutsceneText.text = content.Text;

		if(content.IsLastImage && content.IsLastText)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	bool ContinueCutscene(int cutscene)
	{
		CutsceneDataContent content = cutscenes[cutscene].GetNextCutsceneDataContent();

		cutsceneImage.sprite = content.Image;
		cutsceneText.text = content.Text;

		if(content.IsLastImage && content.IsLastText)
		{
			return false;
		}
		else
		{
			return true;
		}
	}

	private void OnValidate()
	{
		for(int i = 0; i < cutscenes.Length; i++)
		{
			cutscenes[i].name = "Cutscene " + (i + 1).ToString();

			for(int j = 0; j < cutscenes[i].cutsceneDatas.Length; j++)
			{
				cutscenes[i].cutsceneDatas[j].name = cutscenes[i].name + " Image " + (j + 1).ToString();
			}
		}
	}
}
