using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MR
{
    public class SceneManagement : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
        }

        public void Menu()
        {
            SceneManager.LoadScene(0);
        }

        public void PlayGame()
        {
            SceneManager.LoadScene(1);
        }
    }
}
