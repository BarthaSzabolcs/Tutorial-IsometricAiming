using TMPro;

using UnityEngine;

namespace BarthaSzabolcs.TutorialOnly
{
    public class PointInSpace : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [SerializeField] private TextMeshProUGUI textGUI;
        [SerializeField] private string displayName;
        [SerializeField] private string formatString;
        [SerializeField] private bool displayZ;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Update()
        {
            var positionString = transform.position.x.ToString(formatString);
            positionString += "; " + transform.position.y.ToString(formatString);

            if (displayZ)
            {
                positionString += "; " + transform.position.z.ToString(formatString);
            }

            textGUI.text = $"{displayName} ({positionString})";
        }

        #endregion

        #endregion
    }
}