using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MR
{
    public class MR_ReputationScript : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI reputationPointsText;
        [SerializeField] int reputationPoints = 50;

        private void Awake()
        {
            reputationPointsText.text = reputationPoints.ToString();
        }

        private void Update()
        {
            reputationPointsText.text = reputationPoints.ToString();
        }

        public int GetReputationPoints()
        {
            return reputationPoints;
        }

        public void AddReputation(int addPoints)
        {
            reputationPoints += addPoints;
            reputationPointsText.text = reputationPoints.ToString();
        }

        public void LoseReputation(int losePoints)
        {
            reputationPoints -= losePoints;
            reputationPointsText.text = losePoints.ToString();
        }
    }
}
