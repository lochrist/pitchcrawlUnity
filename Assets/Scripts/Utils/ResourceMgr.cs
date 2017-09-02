using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourceMgr {
    Dictionary<string, GameObject> nameToUniqueResource = new Dictionary<string, GameObject>();

    public GameObject GetUniqueResource(string path) {
        GameObject resource;
        if (!nameToUniqueResource.TryGetValue (path, out resource)) {
            var template = Resources.Load (path) as GameObject;
            if (!template) {
                throw new UnityException("No resource with path: " + path);
            }
            resource = GameObject.Instantiate(template) as GameObject;
        }
        return resource;
    }

    static public T Load<T>(string path, bool clone = false) where T : class, System.ICloneable {
        T result = null;
        Object resultObj = null;
        Object[] objs = Resources.LoadAll(path);
        for (int i = 0; i < objs.Length; ++i) {
            resultObj = objs[i];
            result = resultObj as T;
            if (result != null) {
                break;
            }
        }

        if (resultObj && clone) {
            return result.Clone() as T;
        } else {
            return result;
        }
    }
}
