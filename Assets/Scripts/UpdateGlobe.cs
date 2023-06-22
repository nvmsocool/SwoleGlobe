using UnityEngine;

[ExecuteInEditMode]
public class UpdateGlobe : MonoBehaviour
{
    // Start is called before the first frame update
    public bool on = true;
    public float radius = 1;
    void Update()
    {
        Shader.SetGlobalVector("_Center", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1 ));
        Shader.SetGlobalFloat("_Radius", on ? radius : float.MaxValue);
    }
}
