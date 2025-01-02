using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    [SerializeField]
    private GameObject initialVisibleObject;

    private GameObject currentVisibleObject;

    private void Start()
    {
        if (initialVisibleObject != null)
        {
            SetObjectVisible(initialVisibleObject);
        }
        else
        {
            SetAllObjectsInvisible();
        }
    }

    public void AddObject(GameObject obj)
    {
        if (obj != null && !objects.Contains(obj))
        {
            objects.Add(obj);
            if (objects.Count == 1 && currentVisibleObject == null)
            {
                SetObjectVisible(obj);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    public void RemoveObject(GameObject obj)
    {
        if (objects.Contains(obj))
        {
            objects.Remove(obj);

            if (obj == currentVisibleObject)
            {
                currentVisibleObject = null;
            }
        }
    }

    public void SetObjectVisible(GameObject targetObject)
    {
        if (!objects.Contains(targetObject))
        {
            Debug.LogWarning("Target object is not in the managed list!");
            return;
        }

        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(obj == targetObject);
            }
        }

        currentVisibleObject = targetObject;
    }

    private void SetAllObjectsInvisible()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
        currentVisibleObject = null;
    }

    public GameObject GetCurrentVisibleObject()
    {
        return currentVisibleObject;
    }

    public int GetObjectCount()
    {
        return objects.Count;
    }

    public void ClearAllObjects()
    {
        objects.Clear();
        currentVisibleObject = null;
    }

    public bool ContainsObject(GameObject obj)
    {
        return objects.Contains(obj);
    }
}
