using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class ObjectPool : MonoBehaviour
{
    static ObjectPool _instance;

    //Dictionary of lists. objectLookup contains a list for each poolable object type in our game
    Dictionary<Component, List<Component>> objectLookup = new Dictionary<Component, List<Component>>();
    //list of "poolable" objects in the current scene
    Dictionary<Component, Component> prefabLookup = new Dictionary<Component, Component>();

    public static void Clear()
    {
        instance.objectLookup.Clear();
        instance.prefabLookup.Clear();
    }

    public static void CreatePool<T>(T prefab) where T : Component
    {
        if (!instance.objectLookup.ContainsKey(prefab))
            instance.objectLookup.Add(prefab, new List<Component>());
    }

    public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        if (instance.objectLookup.ContainsKey(prefab))
        {
            T obj = null;
            //get reference to the list of objects in this pool
            var list = instance.objectLookup[prefab];
            if (list.Count > 0)
            {
                //pull out an object from the pool, assign it to obj
                while (obj == null && list.Count > 0)
                {
                    obj = list[0] as T;
                    list.RemoveAt(0);   //remove object from the pool
                }
                if (obj != null)    //set object pos,rot, etc.
                {
                    obj.transform.parent = null;
                    obj.transform.localPosition = position;
                    obj.transform.localRotation = rotation;
                    obj.gameObject.SetActive(true); //enables the object
                    instance.prefabLookup.Add(obj, prefab);
                    return (T)obj; //returns the object to the requestor
                }
            }
            //no reusable objects of this type found in the pool, so instantiate one as normal
            obj = (T)Object.Instantiate(prefab, position, rotation);
            instance.prefabLookup.Add(obj, prefab);
            return (T)obj;
        }
        else
            return (T)Object.Instantiate(prefab, position, rotation);
    }

    public static T Spawn<T>(T prefab, Vector3 position) where T : Component
    {
        return Spawn(prefab, position, Quaternion.identity);
    }

    public static T Spawn<T>(T prefab) where T : Component
    {
        return Spawn(prefab, Vector3.zero, Quaternion.identity);
    }

    public static void Recycle<T>(T obj) where T : Component
    {
        if (instance.prefabLookup.ContainsKey(obj)) //if object is in the list of poolable scene objects
        {
            //add the object back to its "poolable" list
            instance.objectLookup[instance.prefabLookup[obj]].Add(obj);
            //remove it from the list of active poolable objects in the scene 
            instance.prefabLookup.Remove(obj);
            obj.transform.parent = instance.transform;
            obj.gameObject.SetActive(false); //deactivate the object
            Debug.Log("Recycle");
        }
        else
        {
            Debug.Log("Throw Away");
            Object.Destroy(obj.gameObject);
        }
    }

    public static int Count<T>(T prefab) where T : Component
    {
        if (instance.objectLookup.ContainsKey(prefab))
            return instance.objectLookup[prefab].Count;
        else
            return 0;

    }

    public static ObjectPool instance
    {
        get
        {
            if (_instance != null)
                return _instance;
            var obj = new GameObject("_ObjectPool");
            obj.transform.localPosition = Vector3.zero;
            _instance = obj.AddComponent<ObjectPool>();
            return _instance;
        }
    }

}

public static class ObjectPoolExtensions
{
    public static void CreatePool<T>(this T prefab) where T : Component
    {
        ObjectPool.CreatePool(prefab);
    }

    public static T Spawn<T>(this T prefab, Vector3 position, Quaternion rotation) where T : Component
    {
        return ObjectPool.Spawn(prefab, position, rotation);
    }
    public static T Spawn<T>(this T prefab, Vector3 position) where T : Component
    {
        return ObjectPool.Spawn(prefab, position, Quaternion.identity);
    }
    public static T Spawn<T>(this T prefab) where T : Component
    {
        return ObjectPool.Spawn(prefab, Vector3.zero, Quaternion.identity);
    }

    public static void Recycle<T>(this T obj) where T : Component
    {
        ObjectPool.Recycle(obj);
    }

    public static int Count<T>(T prefab) where T : Component
    {
        return ObjectPool.Count(prefab);
    }
}