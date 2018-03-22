using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class StorySnapshot
{
    public string avatar;
    public string text;
    public StorySnapshot(string avatar, string text)
    {
        this.avatar = avatar;
        this.text = text;
    }
    public StorySnapshot(string text)
    {
        this.avatar = null;
        this.text = text;
    }
}
