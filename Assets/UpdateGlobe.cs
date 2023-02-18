using UnityEngine;

[ExecuteInEditMode]
public class UpdateGlobe : MonoBehaviour
{
    // Start is called before the first frame update
    void Update()
    {
        Shader.SetGlobalVector("_Center", new Vector4(transform.position.x, transform.position.y, transform.position.z, 1.0f));
    }
}
