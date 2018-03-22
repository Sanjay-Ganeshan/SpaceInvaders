using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class StoryTeller : MonoBehaviour {

    const string INPUT_ADVANCE_TEXT = "Read";

    public CharacterEntry[] Characters;

    private Dictionary<string, Sprite> AvatarLookup;

    public Image UIAvatar;
    public Text UIText;
    public Image UIPanel;
    public Text HelpText;

    public DisplayTimeRemaining timer;

    List<StorySnapshot> DialogueQueue;
    
	// Use this for initialization
	void Start () {
        AvatarLookup = new Dictionary<string, Sprite>();
        foreach(CharacterEntry c in Characters)
        {
            AvatarLookup.Add(c.Name, c.Avatar);
        }
        //UpdateStoryUI("Good morning, captain");
        DialogueQueue = new List<StorySnapshot>();
        LoadStoryScene(StoryScene.GOOD_MORNING);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown(INPUT_ADVANCE_TEXT))
        {
            AdvanceText();
        }
	}

    void AdvanceText()
    {
        StorySnapshot toPlay = null;
        if(DialogueQueue.Count > 0)
        {
            toPlay = DialogueQueue[0];
            DialogueQueue.RemoveAt(0);
        }
        if(toPlay != null)
        {
            UpdateStoryUI(toPlay);
        }
        else
        {
            HideAll();
            GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawn>().SpawnsActive = true;
            timer.startTime = Time.time;
        }
    }

    public void LoadStoryScene(StorySceneInfo scene)
    {
        bool shouldAdvance = DialogueQueue.Count == 0;
        foreach(StorySnapshot ss in scene.snapshots)
        {
            DialogueQueue.Add(ss);
        }
        DialogueQueue.Add(null);
        if(shouldAdvance)
        {
            AdvanceText();
        }
    }

    public void LoadStorySnapshot(StorySnapshot snap)
    {
        DialogueQueue.Add(snap);
        DialogueQueue.Add(null);
    }


    public void HideAll()
    {
        UIAvatar.enabled = false;
        UIPanel.enabled = false;
        UIText.enabled = false;
        HelpText.enabled = false;
    }

    public void UpdateStoryUI(StorySnapshot snap)
    {
        if (snap == null)
        {
            UpdateStoryUI(null, null);
        }
        else
        {
            UpdateStoryUI(snap.text, snap.avatar);
        }
    }

    public void UpdateStoryUI(string text, string avatar)
    {
        if (string.IsNullOrEmpty(avatar))
        {
            UIAvatar.sprite = null;
            UIAvatar.enabled = false;
        }
        else
        {
            UIAvatar.sprite = AvatarLookup[avatar];
            UIAvatar.enabled = true;
        }
        if (string.IsNullOrEmpty(text))
        {
            UIText.enabled = false;
            UIText.text = "";
            HideAll();
        }
        else
        {
            UIText.text = text;
            UIText.enabled = true;
            UIPanel.enabled = true;
            HelpText.enabled = true;
        }
    }

    public void UpdateStoryUI(string text)
    {
        UpdateStoryUI(text, null);
    }
}
