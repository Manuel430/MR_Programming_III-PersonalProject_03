using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    [System.Serializable]
    public struct MR_Sentence
    {
        public string characterName;
        public string text;
        public Sprite characterSprite;

        public MR_Sentence(string setCharacterName, string setText)
        {
            characterSprite = null;
            characterName = setCharacterName;
            text = setText;
        }
    }
}
