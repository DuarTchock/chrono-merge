using UnityEngine;

public class TestCursor : MonoBehaviour
{
    void Start()
    {
        Debug.Log("✅ Unity + Cursor funcionando perfectamente!");
        Debug.Log("🎮 .NET version: " + System.Environment.Version);
        Debug.Log("🚀 Listo para programar el juego!");
    }
    
    void Update()
    {
        // Rotar el objeto continuamente
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
    }
}