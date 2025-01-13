using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dance
{
    public class DanceData : ScriptableObject
    {
        //Todo: To be compiled into BossPresenter

        public Sprite danceSprite;
        public DanceType correspondingNote;
        public enum DanceType
        {
            scratchNote,
            holdNote,
            badNote,
            changeNote,
        }
    }
}
