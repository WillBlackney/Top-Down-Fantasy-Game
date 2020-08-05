using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    // Variables + Component References
    #region
    [Header("Player Move Boundary Properties")]
    [SerializeField] private float padding;
    private float xMin;
    private float xMax;
    private float yMin;
    private float yMax;
    #endregion

    // Properties
    #region
    public float XMin
    {
        get { return xMin; }
        private set { xMin = value; }
    }
    public float XMax
    {
        get { return xMax; }
        private set { xMax = value; }
    }
    public float YMin
    {
        get { return yMin; }
        private set { yMin = value; }
    }
    public float YMax
    {
        get { return yMax; }
        private set { yMax = value; }
    }
    #endregion

    // Logic
    #region
    private void Start()
    {
        SetUpMoveBoundaries();
    }
    private void SetUpMoveBoundaries()
    {
        Camera mainCamera = CameraManager.Instance.mainCamera;

        XMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        XMax = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;

        YMin = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        YMax = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }
    public bool IsLocationInBounds(Vector3 location)
    {
        bool boolReturned = false;

        if (location.x >= XMin &&
            location.x <= XMax &&
            location.y >= YMin &&
            location.y <= YMax)
        {
            return true;
        }

        return boolReturned;
    }
    public Vector3 GetWorldCentre()
    {
        return new Vector3(0, 0, 0);
    }
    #endregion
}
