﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using OWLib.Types;

namespace OWLib.ModelWriter {
  public class ASCIIWriter : IModelWriter {
    public string Name => "XNALara XPS ASCII";
    public string Format => ".mesh.ascii";
    public char[] Identifier => new char[2] { 'l', 'a' };
    public ModelWriterSupport SupportLevel => (ModelWriterSupport.VERTEX | ModelWriterSupport.UV | ModelWriterSupport.BONE | ModelWriterSupport.MATERIAL | ModelWriter.ModelWriterSupport.ATTACHMENT);
    
    public void Write(Model model, Stream output, List<byte> LODs, Dictionary<ulong, List<ImageLayer>> layers, object[] opts) {
			NumberFormatInfo numberFormatInfo = new NumberFormatInfo();
			numberFormatInfo.NumberDecimalSeparator = ".";
      Console.Out.WriteLine("Writing ASCII");
      using(StreamWriter writer = new StreamWriter(output)) {
        writer.WriteLine(model.BoneData.Length);
        for(int i = 0; i < model.BoneData.Length; ++i) {
          writer.WriteLine("bone{0:X}", model.BoneIDs[i]);
          writer.WriteLine(model.BoneHierarchy[i]);
          OpenTK.Vector3 bonePos = model.BoneData[i].ExtractTranslation();
          writer.WriteLine("{0} {1} {2}", bonePos.X.ToString("0.000000", numberFormatInfo), bonePos.Y.ToString("0.000000", numberFormatInfo), bonePos.Z.ToString("0.000000", numberFormatInfo));
        }
        
        Dictionary<byte, List<int>> LODMap = new Dictionary<byte, List<int>>();
        uint sz = 0;
        for(int i = 0; i < model.Submeshes.Length; ++i) {
          ModelSubmesh submesh = model.Submeshes[i];
          if(LODs != null && !LODs.Contains(submesh.lod)) {
            continue;
          }
          if(!LODMap.ContainsKey(submesh.lod)) {
            LODMap.Add(submesh.lod, new List<int>());
          }
          sz++;
          LODMap[submesh.lod].Add(i);
        }

        writer.WriteLine(sz);
        foreach(KeyValuePair<byte, List<int>> kv in LODMap) {
          Console.Out.WriteLine("Writing LOD {0}", kv.Key);
          foreach(int i in kv.Value) {
            ModelSubmesh submesh = model.Submeshes[i];
            ModelVertex[] vertex = model.Vertices[i];
            ModelVertex[] normal = model.Normals[i];
            ModelUV[][] uv = model.UVs[i];
            ModelIndice[] index = model.Faces[i];
            ModelBoneData[] bones = model.Bones[i];

            writer.WriteLine("Submesh_{0}.{1}.{2:X16}", i, kv.Key, model.MaterialKeys[submesh.material]);
            writer.WriteLine(uv.Length);
            ulong materialKey = model.MaterialKeys[submesh.material];
            if(layers.ContainsKey(materialKey)) {
              List<ImageLayer> materialLayers = layers[materialKey];
              uint count = 0;
              HashSet<ulong> done = new HashSet<ulong>();
              for(int j = 0; j < materialLayers.Count; ++j) {
                if(done.Add(materialLayers[j].key)) {
                  count += 1;
                }
              }
              writer.WriteLine(count);
              done.Clear();
              for(int j = 0; j < materialLayers.Count; ++j) {
                if(done.Add(materialLayers[j].key)) {
                  writer.WriteLine(string.Format("{0:X12}.dds", APM.keyToIndexID(materialLayers[j].key)));
                  writer.WriteLine(0);
                }
              }
            } else {
              writer.WriteLine(uv.Length);
              for(int j = 0; j < uv.Length; ++j) {
                writer.WriteLine("{0:X16}_UV{1}.dds", materialKey, j);
                writer.WriteLine(j);
              }
            }

            writer.WriteLine(vertex.Length);
            for(int j = 0; j < vertex.Length; ++j) {
              writer.WriteLine("{0} {1} {2}", vertex[j].x, vertex[j].y, vertex[j].z);
              writer.WriteLine("{0} {1} {2}", -normal[j].x, -normal[j].y, -normal[j].z);
              writer.WriteLine("255 255 255 255");
              for(int k = 0; k < uv.Length; ++k) {
                writer.WriteLine("{0} {1}", uv[k][j].u.ToString("0.######", numberFormatInfo), uv[k][j].v.ToString("0.######", numberFormatInfo));
              }
              if(model.BoneData.Length > 0) {
                writer.WriteLine("{0} {1} {2} {3}", model.BoneLookup[bones[j].boneIndex[0]], model.BoneLookup[bones[j].boneIndex[1]], model.BoneLookup[bones[j].boneIndex[2]], model.BoneLookup[bones[j].boneIndex[3]]);
                writer.WriteLine("{0} {1} {2} {3}", bones[j].boneWeight[0].ToString("0.######", numberFormatInfo), bones[j].boneWeight[1].ToString("0.######", numberFormatInfo), bones[j].boneWeight[2].ToString("0.######", numberFormatInfo), bones[j].boneWeight[3].ToString("0.######", numberFormatInfo));
              }
            }
            writer.WriteLine(index.Length);
            for(int j = 0; j < index.Length; ++j) {
              writer.WriteLine("{0} {1} {2}", index[j].v1, index[j].v2, index[j].v3);
            }
          }
        }
        if(opts.Length > 0 && opts[0] != null && opts[0].GetType() == typeof(bool) && (bool)opts[0] == true) {
          writer.WriteLine(model.AttachmentPoints.Length); // extension, empty nodes.
          for(uint i = 0; i < model.AttachmentPoints.Length; ++i) {
            ModelAttachmentPoint attachment = model.AttachmentPoints[i];
            writer.WriteLine("Attachment{0:X}", attachment.id);
            OpenTK.Matrix4 mat = attachment.matrix.ToOpenTK();
            OpenTK.Vector3 pos = mat.ExtractTranslation();
            OpenTK.Quaternion quat = mat.ExtractRotation();
            writer.WriteLine("{0} {1} {2}", pos.X.ToString("0.######", numberFormatInfo), pos.Y.ToString("0.######", numberFormatInfo), pos.Z.ToString("0.######", numberFormatInfo));
            writer.WriteLine("{0} {1} {2} {3}", quat.X.ToString("0.######", numberFormatInfo), quat.Y.ToString("0.######", numberFormatInfo), quat.Z.ToString("0.######", numberFormatInfo), quat.W.ToString("0.######", numberFormatInfo));
          }
        }
        writer.WriteLine("");
      }
    }
  }
}
