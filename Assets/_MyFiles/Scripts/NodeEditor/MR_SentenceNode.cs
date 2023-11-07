using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MR
{
    public class MR_SentenceNode : MR_Node
    {
        [SerializeField] private MR_Sentence sentence;

        [Space(10)]
        public MR_Node parentNode;
        public MR_Node childNode;
    }
}
