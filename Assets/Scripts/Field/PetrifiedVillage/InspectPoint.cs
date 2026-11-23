using UnityEngine;

namespace PetrifiedVillage
{
    /// <summary>
    /// 調べられる場所の目印。表示するテキストだけを持ち、
    /// 実際の会話ウィンドウ表示はプロジェクト側のUIから title / body を読んで行う。
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class InspectPoint : MonoBehaviour
    {
        [SerializeField] private string title;
        [SerializeField, TextArea(2, 5)] private string body;

        public string Title { get { return title; } }
        public string Body { get { return body; } }

        public void Setup(string title, string body, float radius)
        {
            this.title = title;
            this.body = body;

            SphereCollider trigger = GetComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = radius;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, 0.85f, 0.2f, 0.9f);
            Gizmos.DrawWireSphere(transform.position, 0.35f);
        }
    }
}
