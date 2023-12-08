using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MR
{
    public class EndGame : MonoBehaviour
    {
        [SerializeField] GameObject endGameUI;

        [SerializeField] GameObject lowRepEnd;
        [SerializeField] GameObject midRepEnd;
        [SerializeField] GameObject highRepEnd;

        [SerializeField] MR_ReputationScript finalReputation;
        [SerializeField] MR_PlayerMovementScript player;
        private bool setCutscene;
        private int finalRep;

        public void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                setCutscene = true;
                player.SetCutscene(setCutscene);

                Time.timeScale = 0f;
                endGameUI.SetActive(true);
            }
        }

        public void FinishGame()
        {
            finalRep = finalReputation.GetReputationPoints();
            endGameUI.SetActive(false);

            if(finalRep >= 90)
            {
                highRepEnd.SetActive(true);
            }
            else if(finalRep < 90 && finalRep >= 40)
            {
                midRepEnd.SetActive(true);
            }
            else if (finalRep < 40)
            {
                lowRepEnd.SetActive(true);
            }
        }

        public void ReturnToGame()
        {
            setCutscene = false;
            player.SetCutscene(setCutscene);
            
            Time.timeScale = 1f;
            endGameUI.SetActive(false);
        }
    }
}
