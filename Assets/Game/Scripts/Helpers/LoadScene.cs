using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hexagon
{
    public class LoadScene : MonoBehaviour
    {
        public string sceneName;
        public int delay;

        void Start()
        {
            StartCoroutine(Load());
        }

        IEnumerator Load()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(sceneName);
        }
    }
}
