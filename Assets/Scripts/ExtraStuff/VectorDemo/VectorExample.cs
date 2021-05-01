using TMPro;

using UnityEngine;

namespace BarthaSzabolcs.TutorialOnly
{
    public class VectorExample : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField] private Transform pointA;
        [SerializeField] private Transform pointB;

        [SerializeField] private TextMeshProUGUI distanceGUI;
        [SerializeField] private TextMeshProUGUI directionGUI;
        [SerializeField] private string formatSring;

        [SerializeField] private bool drawPointA;
        [SerializeField] private bool drawPointB;
        [SerializeField] private bool drawDistance;
        [SerializeField] private bool drawDirection;

        [SerializeField] private Color distanceColor;
        [SerializeField] private Color pointAColor;
        [SerializeField] private Color pointBColor;
        [SerializeField] private Color directionColor;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void OnDrawGizmos()
        {
            if (drawPointA)
            {
                Gizmos.color = pointAColor;
                Gizmos.DrawLine(Vector3.zero, pointA.position);
            }

            if (drawPointB)
            {
                Gizmos.color = pointBColor;
                Gizmos.DrawLine(Vector3.zero, pointB.position);
            }

            if (drawDistance)
            {
                Gizmos.color = distanceColor;
                Gizmos.DrawLine(pointA.position, pointB.position);
            }
            if (drawDirection)
            {
                Gizmos.color = directionColor;

                var distance = pointB.position - pointA.position;
                distance.Normalize();
                Gizmos.DrawLine(Vector3.zero, distance);
            }
        }

        private void Update()
        {
            var distance = pointB.position - pointA.position;
            distanceGUI.text = $"A => B distance: {distance.ToString(formatSring)}";

            distance.Normalize();
            directionGUI.text = $"A => B direction: {distance.ToString(formatSring)}";
        }

        #endregion

        #endregion
    }
}