using UnityEngine;

namespace BarthaSzabolcs.IsometricAiming
{
    public class IsometricAimingWithGizmos : MonoBehaviour
    {
        #region Datamembers

        #region Editor Settings

        [Header("Aim")]
        [SerializeField] private bool aim;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private bool ignoreHeight;
        [SerializeField] private Transform aimedTransform;

        [Header("Laser")]
        [SerializeField] private LineRenderer laserRenderer;
        [SerializeField] private LayerMask laserMask;
        [SerializeField] private float laserLength;

        [Header("Projectile")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform prefabSpawn;

        [Header("Gizmos")]
        [SerializeField] private bool gizmo_cameraRay = false;
        [SerializeField] private bool gizmo_ground = false;
        [SerializeField] private bool gizmo_target = false;
        [SerializeField] private bool gizmo_ignoredHeightTarget = false;

        #endregion
        #region Private Fields

        private Camera mainCamera;

        #endregion

        #endregion


        #region Methods

        #region Unity Callbacks

        private void Start()
        {
            mainCamera = Camera.main;

            if (laserRenderer != null)
            {
                laserRenderer.SetPositions(new Vector3[]{
                    Vector3.zero,
                    Vector3.zero
                });
            }
        }

        private void Update()
        {
            Aim();
            RefreshLaser();
            Shoot();
            ChangeTargetMode();
            GizmoSettings();
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying == false)
            {
                return;
            }

            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, float.MaxValue, groundMask))
            {
                if (gizmo_cameraRay)
                {
                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(ray.origin, hitInfo.point);
                    Gizmos.DrawWireSphere(ray.origin, 0.5f);
                }

                var hitPosition = hitInfo.point;
                var hitGroundHeight = Vector3.Scale(hitInfo.point, new Vector3(1, 0, 1)); ;
                var hitPositionIngoredHeight = new Vector3(hitInfo.point.x, aimedTransform.position.y, hitInfo.point.z);

                if (gizmo_ground)
                {
                    Gizmos.color = Color.gray;
                    Gizmos.DrawWireSphere(hitGroundHeight, 0.5f);
                    Gizmos.DrawLine(hitGroundHeight, hitPosition);
                }

                if (gizmo_target)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(hitInfo.point, 0.5f);
                    Gizmos.DrawLine(aimedTransform.position, hitPosition);
                }

                if (gizmo_ignoredHeightTarget)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawWireSphere(hitPositionIngoredHeight, 0.5f);
                    Gizmos.DrawLine(aimedTransform.position, hitPositionIngoredHeight);
                }
            }
        }

        #endregion

        private void Aim()
        {
            if (aim == false)
            {
                return;
            }

            var (success, position) = GetMousePosition();
            if (success)
            {
                // Direction is usually normalized, 
                // but it does not matter in this case.
                var direction = position - aimedTransform.position;

                if (ignoreHeight)
                {
                    // Ignore the height difference.
                    direction.y = 0;
                }

                // Make the transform look at the mouse position.
                aimedTransform.forward = direction;
            }
        }
        
        private (bool success, Vector3 position) GetMousePosition()
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, groundMask))
            {
                return (success: true, position: hitInfo.point);
            }
            else
            {
                return (success: false, position: Vector3.zero);
            }
        }

        private void Shoot()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var projectile = Instantiate(projectilePrefab, prefabSpawn.position, Quaternion.identity);
                projectile.transform.forward = aimedTransform.forward;
            }
        }

        private void RefreshLaser()
        {
            if (laserRenderer == null)
            {
                return;
            }

            Vector3 lineEnd;

            if (Physics.Raycast(prefabSpawn.position, prefabSpawn.forward, out var hitinfo,  laserLength, laserMask))
            {
                lineEnd = hitinfo.point;
            }
            else
            {
                lineEnd = prefabSpawn.position + aimedTransform.forward * laserLength;
            }

            laserRenderer.SetPosition(1, aimedTransform.InverseTransformPoint(lineEnd));
        }

        private void ChangeTargetMode()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ignoreHeight = !ignoreHeight;
            }
        }

        private void GizmoSettings()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gizmo_cameraRay = !gizmo_cameraRay;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gizmo_ground = !gizmo_ground;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gizmo_target = !gizmo_target;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                gizmo_ignoredHeightTarget = !gizmo_ignoredHeightTarget;
            }
        }

        #endregion
    }
}