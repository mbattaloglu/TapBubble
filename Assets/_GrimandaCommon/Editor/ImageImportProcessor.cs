using UnityEngine;
using UnityEditor;

class MakeUIImage : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        // Automatically convert any texture file with "GUIImages" in its file name into an uncompressed unchanged GUI Image.
        if (assetPath.Contains("UIimages") || assetPath.Contains("SpriteFonts") || assetPath.Contains("SpriteAtlases"))
        {
            Debug.Log("Importing new GUI Image!");
            TextureImporter myTextureImporter = (TextureImporter)assetImporter;
            myTextureImporter.textureType = TextureImporterType.Sprite;
            myTextureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            myTextureImporter.convertToNormalmap = false;
            myTextureImporter.maxTextureSize = 2048;
            myTextureImporter.alphaSource =TextureImporterAlphaSource.FromInput;
//            myTextureImporter.generateCubemap = TextureImporter.texture;
            myTextureImporter.npotScale = TextureImporterNPOTScale.None;
            myTextureImporter.isReadable = true;
            myTextureImporter.mipmapEnabled = false;
            //            myTextureImporter.borderMipmap = false;
            //            myTextureImporter.correctGamma = false;
            myTextureImporter.mipmapFilter = TextureImporterMipFilter.BoxFilter;
            myTextureImporter.fadeout = false;
            //            myTextureImporter.mipmapFadeDistanceStart;
            //            myTextureImporter.mipmapFadeDistanceEnd;
            myTextureImporter.convertToNormalmap = false;
            //            myTextureImporter.normalmap;
            //            myTextureImporter.normalmapFilter;
            //            myTextureImporter.heightmapScale;
            //myTextureImporter.lightmap = false;
            myTextureImporter.ClearPlatformTextureSettings("Web");
            myTextureImporter.ClearPlatformTextureSettings("Standalone");
            myTextureImporter.ClearPlatformTextureSettings("iPhone");
        }
    }
}