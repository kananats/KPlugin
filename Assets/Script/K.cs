using KPlugin.Editor;
using UnityEngine;

public class K : MonoBehaviour
{
    [SerializeField]
    private int x;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [SerializeMethod]
    public int SetX()
    {
        transform.localPosition = new Vector3(x, 0, 0);
        return x;
    }
}
