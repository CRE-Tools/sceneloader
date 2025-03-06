using System;
using UnityEngine;
using UnityEngine.UI;


namespace hrspecian.sceneloader.runtime
{
    public class LoaderScreen : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Slider loaderSlider;

        private Action InitializerCallback;

        public void InitializeLoader(Action HasBeenCompletedCallback)
        {
            InitializerCallback = HasBeenCompletedCallback;
            animator.SetBool("ShowLoader", true);
        }

        public void InitializeHasBeenCompleted() => InitializerCallback.Invoke();

        public void SetLoadingProgress(float progress)
        {
            loaderSlider.value = progress;
            if (progress == 1)
                EndLoader();
        }

        private void EndLoader() => animator.SetBool("ShowLoader", false);
    }
}
