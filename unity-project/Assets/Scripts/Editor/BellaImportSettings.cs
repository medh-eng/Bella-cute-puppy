using UnityEditor;
using UnityEngine;

namespace Bella.EditorTools
{
    /// <summary>
    /// Import settings for the game's own files, applied automatically whenever Unity
    /// imports them — so nobody ever has to click through an Inspector:
    ///   Icons / Portraits : crisp UI pictures (no mip-maps, no resizing, clean transparent edges)
    ///   Bella's model     : generic skeleton with its bones kept as objects, no extra materials
    ///   Audio             : music is compressed stereo, short sounds stay instant and mono
    /// </summary>
    public class BellaImportSettings : AssetPostprocessor
    {
        public override uint GetVersion() { return 7; }

        private void OnPreprocessTexture()
        {
            if (!assetPath.StartsWith("Assets/Resources/")) return;
            var ti = assetImporter as TextureImporter;
            if (ti == null) return;

            if (assetPath.Contains("/Surfaces/"))
            {
                // wallpapers and floors tile (Repeat + mip-maps); rugs / window strips / wall pictures stay crisp
                bool tile = assetPath.Contains("/wall_") || assetPath.Contains("/floor_");
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = tile;
                ti.alphaIsTransparency = !tile;
                ti.wrapMode = tile ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = 512;
            }
            else if (assetPath.Contains("/Places/"))
            {
                // scenery backdrops: one big picture, no mip-maps, clamped
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = false;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = 2048;
            }
            else if (assetPath.Contains("/Seasons/"))
            {
                // festival decorations hang in the 3D room (some lie flat on the floor): keep mip-maps so they don't shimmer
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = true;
                ti.alphaIsTransparency = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = 512;
            }
            else if (assetPath.Contains("/Icons/") || assetPath.Contains("/Portraits/") || assetPath.Contains("/Fx/"))
            {
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = false;
                ti.alphaIsTransparency = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = 1024;
            }
            else if (assetPath.Contains("/UI/") || assetPath.Contains("/Story/") || assetPath.Contains("/Stickers/"))
            {
                // generated interface art: crisp, clamped, no mip-maps; big pictures may stay larger
                ti.textureType = TextureImporterType.Default;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.mipmapEnabled = false;
                ti.alphaIsTransparency = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.filterMode = FilterMode.Bilinear;
                ti.maxTextureSize = (assetPath.Contains("title_bg") || assetPath.Contains("/Story/")) ? 2048 : 1024;
            }
            else if (assetPath.Contains("/Props/"))
            {
                ti.textureType = TextureImporterType.Default;
                ti.mipmapEnabled = true;
                ti.wrapMode = TextureWrapMode.Clamp;
                ti.maxTextureSize = 512;
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
            bool prop = assetPath.StartsWith("Assets/Resources/Props/");
            if (!prop && !assetPath.StartsWith("Assets/Resources/Bella/")) return;
            var mi = assetImporter as ModelImporter;
            if (mi == null) return;
            if (prop)
            {
                // furniture and toys: plain static meshes, the game supplies the material and texture
                mi.animationType = ModelImporterAnimationType.None;
                mi.importAnimation = false;
                mi.materialImportMode = ModelImporterMaterialImportMode.None;
                mi.importBlendShapes = false;
                mi.importCameras = false;
                mi.importLights = false;
                mi.globalScale = 1f;
                return;
            }
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
            ai.forceToMono = !assetPath.Contains("/music_");   // music keeps its stereo; sounds and ambience are mono
        }
    }
}
