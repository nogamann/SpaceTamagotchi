using UnityEngine;
using System.Collections;

public class buttonTest : MonoBehaviour {

    public GameObject prefab;

    public void test()
    {
        Instantiate(prefab, new Vector3(2,2,0), new Quaternion(0,0,0,0));
    }
}
