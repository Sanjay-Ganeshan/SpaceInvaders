using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class StorySceneInfo
{
    public StorySnapshot[] snapshots;

    public StorySceneInfo(params StorySnapshot[] snapshots)
    {
        this.snapshots = snapshots;
    }
}
