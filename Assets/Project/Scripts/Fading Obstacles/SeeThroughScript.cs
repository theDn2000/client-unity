using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Rendering;

public class SeeThroughScript : MonoBehaviour
{
    private HashSet<Renderer> _wallRenderers = new HashSet<Renderer>();
    public Camera camera;
    public LayerMask layerMask;

    void Start()
    {
        // Limit fps to 30 to avoid performance issues
        Application.targetFrameRate = 250;
    }

    // Update is called once per frame
    void Update()
    {
        /////////////////////////////////////////////////////////
        /// HAY QUE ASEGURARSE DE QUE EL OBJETO QUE VE EL RAYCAST ES AL QUE SE LE APLICA EL SHADER
        /// Y NO A OTRO OBJETO QUE NO SEA EL QUE SE QUIERE VER
        /////////////////////////////////////////////////////////
        var dir = camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
        var currentWallRenderers = new HashSet<Renderer>();
        bool anyHit = false;
        
        foreach (var h in hits)
        {
            // Verificar si el objeto está en la capa "Wall"
            if (h.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
            {
                var rend = h.collider.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                    currentWallRenderers.Add(rend);
                    anyHit = true;
                }
            }
        }
        
        // Restaurar el shadowCastingMode en los renderers que ya no están siendo tocados por el raycast
        foreach (var oldRenderer in _wallRenderers)
        {
            if (!currentWallRenderers.Contains(oldRenderer))
            {
                if (oldRenderer != null)
                {
                    oldRenderer.shadowCastingMode = ShadowCastingMode.On;
                }
            }
        }
        
        _wallRenderers = currentWallRenderers;
    }
}
