using UnityEngine;
using UnityEngine.UI;

public class ShaderController : MonoBehaviour
{
    public Image SourceImage;
    public Material material;
    public string shaderPropertyName = "_ShineLocation";

    [Range(0f, 1f)]
    public float shineLocation;

    private float previousShineLocation;

    private void Start()
    {
        // Set the initial value of the shine location
        shineLocation = Mathf.Clamp01(shineLocation);
        previousShineLocation = shineLocation;

        // Update the shine location value in the material
        material.SetFloat(shaderPropertyName, shineLocation);

        // Update the image color based on the shine location
        UpdateShineLocation();
    }

    private void Update()
    {
        // Check if the shine location value has changed
        if (shineLocation != previousShineLocation)
        {
            // Clamp the shine location value
            shineLocation = Mathf.Clamp01(shineLocation);

            // Update the shine location value in the material
            material.SetFloat(shaderPropertyName, shineLocation);

            // Update the image color based on the shine location
            UpdateShineLocation();

            // Update the previous shine location value
            previousShineLocation = shineLocation;
        }
    }

    private void UpdateShineLocation()
    {
        // Update the image color based on the shine location
        Color newColor = new Color(shineLocation, shineLocation, shineLocation);
    }
}
