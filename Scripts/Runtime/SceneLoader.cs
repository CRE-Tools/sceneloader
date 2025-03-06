using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace hrspecian.sceneloader.runtime
{
    public class SceneLoader : MonoBehaviour
    {
        private float loadProgress;

        [SerializeField] private LoaderScreen loader = null;

        private bool haveLoaderScreen = false;
        private bool isLoaderInitialized = false;
        private AsyncOperation asyncOp = null;

        #region Singleton
        private static SceneLoader instance;
        public static SceneLoader Instance() => instance;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                DestroyImmediate(this);
            }

            haveLoaderScreen = loader != null;
        }
        #endregion


        public void LoadScene(string sceneName)
        {
            if (asyncOp == null)
                StartCoroutine(LoadSceneCoroutine(sceneName));
        }

        public void LoaderHasBeenInitialized() => isLoaderInitialized = true;

        private IEnumerator LoadSceneCoroutine(string sceneName)
        {
            loadProgress = 0;
            isLoaderInitialized = false;

            asyncOp = SceneManager.LoadSceneAsync(sceneName);
            asyncOp.allowSceneActivation = false;

            if (haveLoaderScreen)
            {
                loader.InitializeLoader(LoaderHasBeenInitialized);
                yield return new WaitUntil(() => isLoaderInitialized);
            }

            do
            {
                yield return new WaitForEndOfFrame();
                loadProgress = asyncOp.progress;

                if (haveLoaderScreen)
                    loader.SetLoadingProgress(loadProgress);

            } while (asyncOp.progress < .9f);

            asyncOp.allowSceneActivation = true;
            yield return new WaitUntil(() => asyncOp.isDone);

            if (haveLoaderScreen)
                loader.SetLoadingProgress(1);

            asyncOp = null;
            yield return null;
        }
    }
}
