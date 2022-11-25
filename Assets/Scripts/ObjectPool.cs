using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public T prefab { get; }
    public bool autoExpand { get; set; }
    public Transform container { get; }

    private List<T> pool;

    public ObjectPool(T prefab, int count)
    {
        this.prefab = prefab;

        this.CreatePool(count);
    }

    public ObjectPool(T prefab, int count, Transform container)
    {
        this.prefab = prefab;
        this.container = container;

        this.CreatePool(count);
    }

    private void CreatePool(int count)
    {
        this.pool = new List<T>();

        for (int i = 0; i < count; i++)
        {
            this.CreateObject();
        }
    }

    private T CreateObject()
    {
        var createdObj = UnityEngine.Object.Instantiate(this.prefab, this.container.position, Quaternion.identity);
        createdObj.gameObject.SetActive(false);
        this.pool.Add(createdObj);
        return createdObj;
    }

    public bool HasFreeElement(out T element)
    {
        foreach (var item in pool)
        {
            if (!item.gameObject.activeInHierarchy)
            {
                element = item;
                item.transform.position = container.position;
                item.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if(this.HasFreeElement(out var element))
        {
            return element;
        }
        if (this.autoExpand)
            return this.CreateObject();

        throw new Exception($"Havn't element: {typeof(T)}");
    }
}
