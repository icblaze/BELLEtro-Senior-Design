// This document contains the code for TheSilence Special Blind.
// This blind will mute the phonetic sounds for the round.
// Current Devs:
// Fredrick Bouloute (bouloutef04)

using UnityEngine;

public class TheSilence : SpecialBlind
{
    private AudioSource voiceHolderSource;
    public TheSilence() : base(SpecialBlindNames.TheSilence, 2, 2)
    {
        description = "All phonetic sounds are muted this round";
        nameText = "The Silence";
    }

    public override void applySpecialBlinds()
    {
        voiceHolderSource = GameObject.Find("VoiceSource").GetComponent<AudioSource>();
        voiceHolderSource.volume = 0;
    }
    public override void cleanUpEffect()
    {
        voiceHolderSource = GameObject.Find("VoiceSource").GetComponent<AudioSource>();
        voiceHolderSource.volume = 1;
    }
}
