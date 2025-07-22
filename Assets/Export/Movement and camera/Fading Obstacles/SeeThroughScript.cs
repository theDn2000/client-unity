using UnityEngine;

using System.Collections.Generic;
using UnityEngine.Rendering;

public class SeeThroughScript : MonoBehaviour
{
    private HashSet<ObjectFader> _faders = new HashSet<ObjectFader>();
    public static int PosID = Shader.PropertyToID("_Position");
    public static int SizeID = Shader.PropertyToID("_Size");
    public Material WallMaterial;
    public Camera camera;
    public LayerMask layerMask;

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
        var currentFaders = new HashSet<ObjectFader>();
        bool anyHit = false;
        foreach (var h in hits)
        {
            var fader = h.collider.GetComponent<ObjectFader>();
            if (fader != null)
            {
                var rend = fader.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }
                fader.DoFade = true;
                currentFaders.Add(fader);
                anyHit = true;
            }
        }
        // Desactivar fade en los que ya no están siendo tocados por el raycast
        foreach (var oldFader in _faders)
        {
            if (!currentFaders.Contains(oldFader))
            {
                oldFader.DoFade = false;
                var rend = oldFader.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.shadowCastingMode = ShadowCastingMode.On;
                }
            }
        }
        _faders = currentFaders;
        WallMaterial.SetFloat(SizeID, anyHit ? 0.5f : 0);

        var view = camera.WorldToViewportPoint(transform.position);
        WallMaterial.SetVector(PosID, new Vector4(view.x, view.y, 0, 0));
    }
}
