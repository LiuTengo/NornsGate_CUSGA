using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using System.Linq;

namespace XPostProcessing
{
    public class CustomVolumePass : BaseVolumePass
    {
        protected override string RenderPostProcessingTag => "Custom Render PostProcessing Effects";


        protected override void OnInit()// CustomVolumePass():base()
        {
            // var customVolumes = VolumeManager.instance.baseComponentTypeArray
            //     .Where(t => t.IsSubclassOf(typeof(VolumeSetting)))
            //     .Select(t => VolumeManager.instance.stack.GetComponent(t) as VolumeSetting)
            //     .ToList();
            // Debug.LogError("customVolumesL:" + customVolumes.Count);

            //SSAO
            //AddEffect(new SSAORenderer());
            AddEffect(new BokehBlurRenderer());

        }
        
    }
}