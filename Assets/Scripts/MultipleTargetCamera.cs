using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MultipleTargetCamera : MonoBehaviour
{

    #region singleton
    // singleton
    public static MultipleTargetCamera instance;
    private void Awake()
    {
        if (instance && instance != this) { Destroy(gameObject); return; }
        instance = this;
    }
    #endregion


    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = 0.5f;

    public float minZoom = 40f;
    public float maxZoom = 10f;

    public float padding = 20f;

    private Vector3 velocity;
    private Camera cam;

    private float setSize = 5f;
    private bool settingSize = false;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (targets.Count == 0) { return; }

        Bounds bounds = CreateBounds();
        Move(bounds);
        Zoom(bounds);
    }

    private void Update()
    {
        if (settingSize)
        {
            cam.orthographicSize = setSize;
        }
    }



    private Bounds CreateBounds()
    {
        if (targets.Count > 0)
        {
            Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds;
        }
        else
        {
            return new Bounds();
        }
    }


    private void Move(Bounds bounds)
    {
        Vector3 centerPoint = (targets.Count != 1) ? bounds.center : targets[0].position;
        Vector3 newPostion = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPostion, ref velocity, smoothTime);
    }



    private void Zoom(Bounds bounds)
    {
        float desiredSize = bounds.size.y / 2;

        float xy = ((16f / 9f) * bounds.size.x) / 4f;
        desiredSize = Mathf.Max(desiredSize, xy);

        float percentage = (desiredSize - minZoom) / (maxZoom - minZoom);
        float newZoom = minZoom + Mathf.Lerp(padding, maxZoom - minZoom + padding, percentage);

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }



    public void MoveTo(Vector2 newPosition, float size, System.Action onComplete = null)
    {
        Vector3 camPosition = newPosition;
        camPosition.z = -10f;

        transform.DOMove(camPosition, 0.8f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            if (onComplete != null) { onComplete(); }
        });


        settingSize = true;
        DOTween.To(() => setSize, value => setSize = value, size, 0.8f).OnComplete(() =>
        {
            settingSize = false;
        });
    }


    public void AddTarget(Transform newTarget)
    {
        targets.Add(newTarget);
    }

    public void RemoveTarget(Transform removeTarget)
    {
        targets.Remove(removeTarget);
    }

    public void ClearTargets()
    {
        targets.Clear();
    }

    private void OnDrawGizmosSelected()
    {
        Bounds bounds = CreateBounds();

        Gizmos.color = Color.magenta;
        float left = bounds.center.x - (bounds.size.x / 2);
        float right = bounds.center.x + (bounds.size.x / 2);
        float top = bounds.center.y - (bounds.size.y / 2);
        float bottom = bounds.center.y + (bounds.size.y / 2);
        Gizmos.DrawLine(new Vector2(left, top), new Vector2(right, top));
        Gizmos.DrawLine(new Vector2(right, top), new Vector2(right, bottom));
        Gizmos.DrawLine(new Vector2(left, bottom), new Vector2(right, bottom));
        Gizmos.DrawLine(new Vector2(left, top), new Vector2(left, bottom));
    }
}
