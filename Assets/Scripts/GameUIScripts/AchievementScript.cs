using System.Collections;
using UnityEngine;
using TMPro;

namespace BattleTank.Achievement
{
    public class AchievementScript : MonoBehaviour
    {
        private Vector3 initialPosition = new Vector3(0, 270, 0);

        [SerializeField] private Animator animator;
        [SerializeField] private TMP_Text achievementMessage;
        [SerializeField] private float timeToDestroy = 6f;

        public void SetLocalTransform()
        {
            transform.localScale = Vector3.one;
            transform.localPosition = initialPosition;
        }

        public void SetMessage(string _message)
        {
            achievementMessage.text = _message;
        }

        public void ShowcaseAchievement()
        {
            animator.SetTrigger("Showcase");
            StartCoroutine(DestroyTimer(timeToDestroy));
        }

        IEnumerator DestroyTimer(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(this.gameObject);
        }
    }
}
