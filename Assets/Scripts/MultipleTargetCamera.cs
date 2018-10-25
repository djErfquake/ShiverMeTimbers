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
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;


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



    private Bounds CreateBounds()
    {
        Bounds bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds;
    }


    private void Move(Bounds bounds)
    {
        Vector3 centerPoint = (targets.Count != 1) ? bounds.center : targets[0].position;
        Vector3 newPostion = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPostion, ref velocity, smoothTime);
    }


    private void Zoom(Bounds bounds)
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, bounds.size.x / zoomLimiter);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime);
    }



    public void MoveTo(Vector2 newPosition, System.Action onComplete = null)
    {
        Vector3 camPosition = newPosition;
        camPosition.z = -10f;

        transform.DOMove(camPosition, 0.8f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            if (onComplete != null) { onComplete(); }
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
}
