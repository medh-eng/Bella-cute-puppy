using UnityEditor;
using UnityEngine;

namespace Bella.EditorTools
{
    /// <summary>
    /// Import settings for the game's own files, applied automatically whenever Unity
    /// imports them — so nobody ever has to click through an Inspector:
    ///   Icons / Portraits : crisp UI pictures (no mip-maps, no resizing, clean transparent edges)
    ///   Bella's model     : generic skeleton with its bones kept as objects, no extra materials
    ///   Audio             : music is compressed, short sounds stay instant
    /// </summary>
    public class BellaImportSettings : AssetPostprocessor
    {
        public override uint GetVersion() { return 2; }

        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith("Assets/Resources/")) return;
            var ti = assetImporter as TextureImporter;
            if (ti == null) return;

            if (assetPath.Contains("/Icons/") || assetPath.Contains("/Portraits/"))
            {
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = false;
                ti.alphaIsTransparency = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = 1024;
            }
            else if (assetPath.Contains("/Bella/"))
            {
                ti.textureType = TextureImporterType.Default;
                ti.mipmapEnabled = true;
                ti.maxTextureSize = 1024;
            }
        }

        private void OnPreprocessModel()
        {
            if (!assetPath.StartsWith("Assets/Resources/Bella/")) return;
            var mi = assetImporter as ModelImporter;
            if (mi == null) return;
            mi.animationType = ModelImporterAnimationType.Generic;
            mi.importAnimation = true;
            mi.optimizeGameObjects = false;     // the game moves tail / ear / head bones itself
            mi.animationCompression = ModelImporterAnimationCompression.Off;
            mi.materialImportMode = ModelImporterMaterialImportMode.None;
            mi.importBlendShapes = false;
            mi.importCameras = false;
            mi.importLights = false;
        }

        private void OnPreprocessAudio()
        {
            if (!assetPath.StartsWith("Assets/Resources/Audio/")) return;
            var ai = assetImporter as AudioImporter;
            if (ai == null) return;
            bool music = assetPath.Contains("music_") || assetPath.Contains("amb_");
            AudioImporterSampleSettings s = ai.defaultSampleSettings;
            s.loadType = music ? AudioClipLoadType.CompressedInMemory : AudioClipLoadType.DecompressOnLoad;
            s.compressionFormat = music ? AudioCompressionFormat.Vorbis : AudioCompressionFormat.ADPCM;
            s.quality = 0.6f;
            ai.defaultSampleSettings = s;
            ai.forceToMono = true;
        }
    }
}
