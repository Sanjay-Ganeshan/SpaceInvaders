using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StoryScene
{
    public static StorySceneInfo GOOD_MORNING = new StorySceneInfo(
        new StorySnapshot("captain", "Dammit! We've been fooled! Our radar readings were off - they're slipping past us!"),
        new StorySnapshot("captain", "Good evening, cadet. I am Admiral Eliza IV, commander of the Terran Starfleet."),
        new StorySnapshot("captain", "Early this morning, we recieved information on possible whereabouts of the Mothership. "),
        new StorySnapshot("captain",
            "Naturally, we sent Starfleet in response, to wipe out those Invaders once and for all"),
        new StorySnapshot("captain", "It looks like our information was falsely planted. We've used up our light jumps in order to get here, " +
            "and they slipped past us. You are the only human pilot who's anywhere close to position to defend Earth."),
        new StorySnapshot("captain", "We need you to hold the fort for 5 minutes while we send reinforcements. Until then, you're alone."),
        new StorySnapshot("captain", "We brought all of our resources with Starfleet, so you won't have anyone to resupply you."),
        new StorySnapshot("captain", "But, HQ had an idea. If you can use your docking tether as a weapon, you might be able to take control of enemy ships before you die"),
        new StorySnapshot("captain", "We're moving as fast as we can. Hold the fort till then...the fate of Earth depends on it"),
        new StorySnapshot("robot","Your ship is armed with advanced targetting systems (mouse), a blaster (left click), and a boarding tether (right click)"),
        new StorySnapshot("robot", "You can move around in any direction using ASDW, and use your lightspeed boosts with L-SHIFT"),
        new StorySnapshot("robot", "Your ship's energy, which powers your shields, weapons, and lightjump is in the top right"),
        new StorySnapshot("robot", "Your minimap is in the top right. Good luck, Cadet")

        );
}
