﻿using System;
using System.Collections.Generic;
using System.IO;
using DataTool.Flag;
using OWLib;
using OWLib.Types;
using OWLib.Types.Map;
using OWLib.Writer;
using STULib;
using STULib.Types.Generic;
using TankLib;
using TankLib.ExportFormats;
using TankLib.STU.Types;
using static DataTool.Helper.IO;
using static DataTool.Helper.STUHelper;
using static DataTool.Helper.Logger;
using STUMap = STULib.Types.STUMap;
using STUModelComponentInstanceData = STULib.Types.STUModelComponentInstanceData;

namespace DataTool.SaveLogic {
    public class Map {
        public class OWMap14Writer : IDataWriter {
            public string Format => ".owmap";
            public WriterSupport SupportLevel => WriterSupport.VERTEX | WriterSupport.MAP;
            public char[] Identifier => new [] { 'O' };
            public string Name => "OWM Map Format";
    
            public bool Write(Animation anim, Stream output, object[] data) {
                return false;
            }

            public Dictionary<ulong, List<string>>[] Write(Stream output, OWLib.Map map, OWLib.Map detail1, OWLib.Map detail2, OWLib.Map props, OWLib.Map lights, string name,
                IDataWriter modelFormat) {
                throw new NotImplementedException();
            }

            public bool Write(Map10 physics, Stream output, object[] data) {
                return false;
            }
    
            public bool Write(Chunked model, Stream output, List<byte> LODs, Dictionary<ulong, List<ImageLayer>> layers, object[] data) {
                return false;
            }
            
            public void Write(Stream output, STULib.Types.Map.Map map, STULib.Types.Map.Map detail1, STULib.Types.Map.Map model8s, STULib.Types.Map.Map entities, STULib.Types.Map.Map lights, string name, IDataWriter modelFormat, FindLogic.Combo.ComboInfo info) {
                if (modelFormat == null) {
                    modelFormat = new OWMDLWriter();
                }
                using (BinaryWriter writer = new BinaryWriter(output)) {
                    writer.Write((ushort)1); // version major
                    writer.Write((ushort)1); // version minor
    
                    if (name.Length == 0) {
                        writer.Write((byte)0);
                    } else {
                        writer.Write(name);
                    }
    
                    uint size = 0;
                    foreach (IMapFormat t in map.Records) {
                        if (t != null && t.GetType() != typeof(Map01)) {
                            continue;
                        }
                        size++;
                    }
                    writer.Write(size); // nr objects
    
                    size = 1;
                    foreach (IMapFormat t in detail1.Records) {
                        if (t != null && t.GetType() != typeof(Map02)) {
                            continue;
                        }
                        size++;
                    }
                    foreach (IMapFormat t in model8s.Records) {
                        if (t != null && t.GetType() != typeof(Map08)) {
                            continue;
                        }
                        size++;
                    }
                    foreach (IMapFormat t in entities.Records) {
                        if (t != null && t.GetType() != typeof(MapEntity)) {
                            continue;
                        }
                        if (((MapEntity)t).Model == 0) {
                            continue;
                        }
                        size++;
                    }
                    writer.Write(size); // nr details
    
                    // Extension 1.1 - Lights
                    size = 0;
                    foreach (IMapFormat t in lights.Records) {
                        if (t != null && t.GetType() != typeof(Map09)) {
                            continue;
                        }
                        size++;
                    }
                    writer.Write(size); // nr Lights
    
                    foreach (IMapFormat t in map.Records) {
                        if (t != null && t.GetType() != typeof(Map01)) {
                            continue;
                        }
                        Map01 obj = (Map01)t;
                        FindLogic.Combo.Find(info, obj.Header.Model);
                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[obj.Header.Model];
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        writer.Write(modelFn);
                        writer.Write(obj.Header.groupCount);
                        for (int j = 0; j < obj.Header.groupCount; ++j) {
                            Map01.Map01Group group = obj.Groups[j];
                            FindLogic.Combo.Find(info, group.ModelLook, null, new FindLogic.Combo.ComboContext {Model = obj.Header.Model});
                            FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[group.ModelLook];
                            string materialFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                            
                            writer.Write(materialFn);
                            writer.Write(group.recordCount);
                            for (int k = 0; k < group.recordCount; ++k) {
                                Map01.Map01GroupRecord record = obj.Records[j][k];
                                writer.Write(record.position.x);
                                writer.Write(record.position.y);
                                writer.Write(record.position.z);
                                writer.Write(record.scale.x);
                                writer.Write(record.scale.y);
                                writer.Write(record.scale.z);
                                writer.Write(record.rotation.x);
                                writer.Write(record.rotation.y);
                                writer.Write(record.rotation.z);
                                writer.Write(record.rotation.w);
                            }
                        }
                    }
                                      
                    // todo: broken?
                    writer.Write($"Models\\physics\\physics.{modelFormat.Format}");
                    writer.Write((byte)0);
                    writer.Write(0.0f);
                    writer.Write(0.0f);
                    writer.Write(0.0f);
                    writer.Write(1.0f);
                    writer.Write(1.0f);
                    writer.Write(1.0f);
                    writer.Write(0.0f);
                    writer.Write(0.0f);
                    writer.Write(0.0f);
                    writer.Write(1.0f);
    
                    foreach (IMapFormat t in detail1.Records) {
                        if (t != null && t.GetType() != typeof(Map02)) {
                            continue;
                        }
                        Map02 obj = (Map02)t;
                        
                        FindLogic.Combo.Find(info, obj.Header.Model);
                        FindLogic.Combo.Find(info, obj.Header.ModelLook, null, new FindLogic.Combo.ComboContext {Model = obj.Header.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[obj.Header.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[obj.Header.ModelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(obj.Header.position.x);
                        writer.Write(obj.Header.position.y);
                        writer.Write(obj.Header.position.z);
                        writer.Write(obj.Header.scale.x);
                        writer.Write(obj.Header.scale.y);
                        writer.Write(obj.Header.scale.z);
                        writer.Write(obj.Header.rotation.x);
                        writer.Write(obj.Header.rotation.y);
                        writer.Write(obj.Header.rotation.z);
                        writer.Write(obj.Header.rotation.w);
                    }
    
                    foreach (IMapFormat t in model8s.Records) {
                        if (t != null && t.GetType() != typeof(Map08)) {
                            continue;
                        }
                        Map08 obj = (Map08)t;
                        
                        FindLogic.Combo.Find(info, obj.Header.Model);
                        FindLogic.Combo.Find(info, obj.Header.ModelLook, null, new FindLogic.Combo.ComboContext {Model = obj.Header.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[obj.Header.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[obj.Header.ModelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(obj.Header.position.x);
                        writer.Write(obj.Header.position.y);
                        writer.Write(obj.Header.position.z);
                        writer.Write(obj.Header.scale.x);
                        writer.Write(obj.Header.scale.y);
                        writer.Write(obj.Header.scale.z);
                        writer.Write(obj.Header.rotation.x);
                        writer.Write(obj.Header.rotation.y);
                        writer.Write(obj.Header.rotation.z);
                        writer.Write(obj.Header.rotation.w);
                    }
    
                    foreach (IMapFormat t in entities.Records) {
                        if (t != null && t.GetType() != typeof(MapEntity)) {
                            continue;
                        }
                        MapEntity mapEntity = (MapEntity)t;
                        if (mapEntity.Model == 0) {
                            continue;
                        }

                        ulong modelLook = mapEntity.ModelLook;

                        foreach (object container in mapEntity.STUContainers) {
                            ISTU realContainer = (ISTU) container;

                            foreach (Common.STUInstance instance in realContainer.Instances) {
                                if (instance is STUModelComponentInstanceData modelComponentInstanceData) {
                                    if (modelComponentInstanceData.Look != 0) {
                                        modelLook = modelComponentInstanceData.Look;
                                    }
                                }
                            }
                        }
                        
                        FindLogic.Combo.Find(info, mapEntity.Model);
                        FindLogic.Combo.Find(info, modelLook, null, new FindLogic.Combo.ComboContext {Model = mapEntity.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[mapEntity.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[modelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(mapEntity.Header.Position.x);
                        writer.Write(mapEntity.Header.Position.y);
                        writer.Write(mapEntity.Header.Position.z);
                        writer.Write(mapEntity.Header.Scale.x);
                        writer.Write(mapEntity.Header.Scale.y);
                        writer.Write(mapEntity.Header.Scale.z);
                        writer.Write(mapEntity.Header.Rotation.x);
                        writer.Write(mapEntity.Header.Rotation.y);
                        writer.Write(mapEntity.Header.Rotation.z);
                        writer.Write(mapEntity.Header.Rotation.w);
                    }
    
                    // Extension 1.1 - Lights
                    foreach (IMapFormat t in lights.Records) {
                        if (t != null && t.GetType() != typeof(Map09)) {
                            continue;
                        }
                        Map09 obj = (Map09)t;
                        writer.Write(obj.Header.position.x);
                        writer.Write(obj.Header.position.y);
                        writer.Write(obj.Header.position.z);
                        writer.Write(obj.Header.rotation.x);
                        writer.Write(obj.Header.rotation.y);
                        writer.Write(obj.Header.rotation.z);
                        writer.Write(obj.Header.rotation.w);
                        writer.Write(obj.Header.LightType);
                        writer.Write(obj.Header.LightFOV);
                        writer.Write(obj.Header.Color.x);
                        writer.Write(obj.Header.Color.y);
                        writer.Write(obj.Header.Color.z);
                        writer.Write(obj.Header.unknown1A);
                        writer.Write(obj.Header.unknown1B);
                        writer.Write(obj.Header.unknown2A);
                        writer.Write(obj.Header.unknown2B);
                        writer.Write(obj.Header.unknown2C);
                        writer.Write(obj.Header.unknown2D);
                        writer.Write(obj.Header.unknown3A);
                        writer.Write(obj.Header.unknown3B);
    
                        writer.Write(obj.Header.unknownPos1.x);
                        writer.Write(obj.Header.unknownPos1.y);
                        writer.Write(obj.Header.unknownPos1.z);
                        writer.Write(obj.Header.unknownQuat1.x);
                        writer.Write(obj.Header.unknownQuat1.y);
                        writer.Write(obj.Header.unknownQuat1.z);
                        writer.Write(obj.Header.unknownQuat1.w);
                        writer.Write(obj.Header.unknownPos2.x);
                        writer.Write(obj.Header.unknownPos2.y);
                        writer.Write(obj.Header.unknownPos2.z);
                        writer.Write(obj.Header.unknownQuat2.x);
                        writer.Write(obj.Header.unknownQuat2.y);
                        writer.Write(obj.Header.unknownQuat2.z);
                        writer.Write(obj.Header.unknownQuat2.w);
                        writer.Write(obj.Header.unknownPos3.x);
                        writer.Write(obj.Header.unknownPos3.y);
                        writer.Write(obj.Header.unknownPos3.z);
                        writer.Write(obj.Header.unknownQuat3.x);
                        writer.Write(obj.Header.unknownQuat3.y);
                        writer.Write(obj.Header.unknownQuat3.z);
                        writer.Write(obj.Header.unknownQuat3.w);
    
                        writer.Write(obj.Header.unknown4A);
                        writer.Write(obj.Header.unknown4B);
                        writer.Write(obj.Header.unknown5);
                        writer.Write(obj.Header.unknown6A);
                        writer.Write(obj.Header.unknown6B);
                        writer.Write(obj.Header.unknown7A);
                        writer.Write(obj.Header.unknown7B);
                    }
                }
            }
            
            public void Write(Stream output, teMapPlaceableData modelGroups, teMapPlaceableData singleModels, teMapPlaceableData model8s, teMapPlaceableData entities, teMapPlaceableData lights, string name, FindLogic.Combo.ComboInfo info) {
                using (BinaryWriter writer = new BinaryWriter(output)) {
                    writer.Write((ushort)1); // version major
                    writer.Write((ushort)1); // version minor
    
                    if (name.Length == 0) {
                        writer.Write((byte)0);
                    } else {
                        writer.Write(name);
                    }
    
                    //uint size = 0;
                    //foreach (IMapPlaceable t in modelGroups.Placeables) {
                    //    if (t != null && t.GetType() != typeof(Map01)) {
                    //        continue;
                    //    }
                    //    size++;
                    //}
                    writer.Write(modelGroups.Header.PlaceableCount); // nr objects
    
                    //size = 1;
                    //foreach (IMapFormat t in singleModels.Records) {
                    //    if (t != null && t.GetType() != typeof(Map02)) {
                    //        continue;
                    //    }
                    //    size++;
                    //}
                    //foreach (IMapFormat t in model8s.Records) {
                    //    if (t != null && t.GetType() != typeof(Map08)) {
                    //        continue;
                    //    }
                    //    size++;
                    //}
                    //foreach (IMapFormat t in entities.Records) {
                    //    if (t != null && t.GetType() != typeof(MapEntity)) {
                    //        continue;
                    //    }
                    //    if (((MapEntity)t).Model == 0) {
                    //        continue;
                    //    }
                    //    size++;
                    //}
                    int entitiesWithModelCount = 0;
                    STUModelComponent[] modelComponents = new STUModelComponent[entities.Header.PlaceableCount];
                    
                    for (int i = 0; i < entities.Header.PlaceableCount; i++) {
                        teMapPlaceableEntity entity = (teMapPlaceableEntity) entities.Placeables[i];
                        STUModelComponent component = GetInstanceNew<STUModelComponent>(entity.Header.EntityDefinition);
                        //STUEntityDefinition entityDefinition = GetInstanceNew<STUEntityDefinition>(entity.Header.EntityDefinition);
                        if (component != null) {
                            entitiesWithModelCount++;
                            modelComponents[i] = component;
                        }
                    }
                    
                    writer.Write(singleModels.Header.PlaceableCount + model8s.Header.PlaceableCount +
                                 entitiesWithModelCount); // nr details
    
                    /*// Extension 1.1 - Lights
                    //size = 0;
                    //foreach (IMapFormat t in lights.Records) {
                    //    if (t != null && t.GetType() != typeof(Map09)) {
                    //        continue;
                    //    }
                    //    size++;
                    //}
                    writer.Write(lights.Header.PlaceableCount); // nr Lights
    
                    foreach (IMapPlaceable mapPlaceable in modelGroups.Placeables) {
                        teMapPlaceableModelGroup modelGroup = (teMapPlaceableModelGroup) mapPlaceable;
                        
                        FindLogic.Combo.Find(info, modelGroup.Header.Model);
                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[modelGroup.Header.Model];
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        writer.Write(modelFn);
                        writer.Write(modelGroup.Header.GroupCount);
                        for (int j = 0; j < modelGroup.Header.GroupCount; ++j) {
                            teMapPlaceableModelGroup.Group group = modelGroup.Groups[j];
                            FindLogic.Combo.Find(info, group.ModelLook, null, new FindLogic.Combo.ComboContext {Model = modelGroup.Header.Model});
                            FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[group.ModelLook];
                            string materialFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                            
                            writer.Write(materialFn);
                            writer.Write(group.EntryCount);
                            for (int k = 0; k < group.EntryCount; ++k) {
                                teMapPlaceableModelGroup.Entry record = modelGroup.Entries[j][k];
                                
                                writer.Write(record.Translation);
                                writer.Write(record.Scale);
                                writer.Write(record.Rotation);
                            }
                        }
                    }
    
                    foreach (IMapPlaceable mapPlaceable in singleModels.Placeables) {
                        teMapPlaceableSingleModel singleModel = (teMapPlaceableSingleModel)mapPlaceable;
                        
                        FindLogic.Combo.Find(info, singleModel.Header.Model);
                        FindLogic.Combo.Find(info, singleModel.Header.ModelLook, null, new FindLogic.Combo.ComboContext {Model = singleModel.Header.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[singleModel.Header.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[singleModel.Header.ModelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(singleModel.Header.Translation);
                        writer.Write(singleModel.Header.Scale);
                        writer.Write(singleModel.Header.Rotation);
                    }
    
                    foreach (IMapPlaceable mapPlaceable in model8s.Placeables) {
                        //var obj = (teMa) mapPlaceable;
                        
                        FindLogic.Combo.Find(info, obj.Header.Model);
                        FindLogic.Combo.Find(info, obj.Header.ModelLook, null, new FindLogic.Combo.ComboContext {Model = obj.Header.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[obj.Header.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[obj.Header.ModelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(obj.Header.position.x);
                        writer.Write(obj.Header.position.y);
                        writer.Write(obj.Header.position.z);
                        writer.Write(obj.Header.scale.x);
                        writer.Write(obj.Header.scale.y);
                        writer.Write(obj.Header.scale.z);
                        writer.Write(obj.Header.rotation.x);
                        writer.Write(obj.Header.rotation.y);
                        writer.Write(obj.Header.rotation.z);
                        writer.Write(obj.Header.rotation.w);
                    }
    
                    foreach (var mapPlaceable in entities.Placeables) {
                        var entity = (teMapPlaceableEntity) mapPlaceable;

                        STUEntityDefinition entityDefinition = GetInstanceNew<STUEntityDefinition>(entity.Header.EntityDefinition);

                        ulong model;
                        ulong modelLook;

                        //foreach (object container in mapEntity.STUContainers) {
                        //    ISTU realContainer = (ISTU) container;
                        //    foreach (Common.STUInstance instance in realContainer.Instances) {
                        //        if (instance is STUModelComponentInstanceData modelComponentInstanceData) {
                        //            if (modelComponentInstanceData.Look != 0) {
                        //                modelLook = modelComponentInstanceData.Look;
                        //            }
                        //        }
                        //    }
                        //}
                        
                        FindLogic.Combo.Find(info, mapEntity.Model);
                        FindLogic.Combo.Find(info, modelLook, null, new FindLogic.Combo.ComboContext {Model = mapEntity.Model});

                        FindLogic.Combo.ModelInfoNew modelInfo = info.Models[mapEntity.Model];
                        FindLogic.Combo.ModelLookInfo modelLookInfo = info.ModelLooks[modelLook]; 
                        string modelFn = $"Models\\{modelInfo.GetName()}\\{modelInfo.GetNameIndex()}{modelFormat.Format}";
                        string matFn = $"Models\\{modelInfo.GetName()}\\ModelLooks\\{modelLookInfo.GetNameIndex()}.owmat";
                        
                        writer.Write(modelFn);
                        writer.Write(matFn);
                        writer.Write(mapEntity.Header.Position.x);
                        writer.Write(mapEntity.Header.Position.y);
                        writer.Write(mapEntity.Header.Position.z);
                        writer.Write(mapEntity.Header.Scale.x);
                        writer.Write(mapEntity.Header.Scale.y);
                        writer.Write(mapEntity.Header.Scale.z);
                        writer.Write(mapEntity.Header.Rotation.x);
                        writer.Write(mapEntity.Header.Rotation.y);
                        writer.Write(mapEntity.Header.Rotation.z);
                        writer.Write(mapEntity.Header.Rotation.w);
                    }
    
                    // Extension 1.1 - Lights
                    foreach (IMapFormat t in lights.Records) {
                        if (t != null && t.GetType() != typeof(Map09)) {
                            continue;
                        }
                        Map09 obj = (Map09)t;
                        writer.Write(obj.Header.position.x);
                        writer.Write(obj.Header.position.y);
                        writer.Write(obj.Header.position.z);
                        writer.Write(obj.Header.rotation.x);
                        writer.Write(obj.Header.rotation.y);
                        writer.Write(obj.Header.rotation.z);
                        writer.Write(obj.Header.rotation.w);
                        writer.Write(obj.Header.LightType);
                        writer.Write(obj.Header.LightFOV);
                        writer.Write(obj.Header.Color.x);
                        writer.Write(obj.Header.Color.y);
                        writer.Write(obj.Header.Color.z);
                        writer.Write(obj.Header.unknown1A);
                        writer.Write(obj.Header.unknown1B);
                        writer.Write(obj.Header.unknown2A);
                        writer.Write(obj.Header.unknown2B);
                        writer.Write(obj.Header.unknown2C);
                        writer.Write(obj.Header.unknown2D);
                        writer.Write(obj.Header.unknown3A);
                        writer.Write(obj.Header.unknown3B);
    
                        writer.Write(obj.Header.unknownPos1.x);
                        writer.Write(obj.Header.unknownPos1.y);
                        writer.Write(obj.Header.unknownPos1.z);
                        writer.Write(obj.Header.unknownQuat1.x);
                        writer.Write(obj.Header.unknownQuat1.y);
                        writer.Write(obj.Header.unknownQuat1.z);
                        writer.Write(obj.Header.unknownQuat1.w);
                        writer.Write(obj.Header.unknownPos2.x);
                        writer.Write(obj.Header.unknownPos2.y);
                        writer.Write(obj.Header.unknownPos2.z);
                        writer.Write(obj.Header.unknownQuat2.x);
                        writer.Write(obj.Header.unknownQuat2.y);
                        writer.Write(obj.Header.unknownQuat2.z);
                        writer.Write(obj.Header.unknownQuat2.w);
                        writer.Write(obj.Header.unknownPos3.x);
                        writer.Write(obj.Header.unknownPos3.y);
                        writer.Write(obj.Header.unknownPos3.z);
                        writer.Write(obj.Header.unknownQuat3.x);
                        writer.Write(obj.Header.unknownQuat3.y);
                        writer.Write(obj.Header.unknownQuat3.z);
                        writer.Write(obj.Header.unknownQuat3.w);
    
                        writer.Write(obj.Header.unknown4A);
                        writer.Write(obj.Header.unknown4B);
                        writer.Write(obj.Header.unknown5);
                        writer.Write(obj.Header.unknown6A);
                        writer.Write(obj.Header.unknown6B);
                        writer.Write(obj.Header.unknown7A);
                        writer.Write(obj.Header.unknown7B);
                    }*/
                }
            }
        }
        
        /// <summary>
        /// OWMAP format
        /// </summary>
        public class OverwatchMap : IExportFormat {
            public string Extension => "owmap";

            public string Name;
            
            public OverwatchMap(string name, FindLogic.Combo.ComboInfo info, teMapPlaceableData modelGroups, teMapPlaceableData singleModels, teMapPlaceableData placeable8, teMapPlaceableData entities, teMapPlaceableData lights) {
                Name = name;
            }
        
            public void Write(Stream stream) {            
                throw new NotImplementedException();
            }
        }

        public static void Save(ICLIFlags flags, STUMap map, ulong key, string basePath) {
            string name = GetValidFilename(GetString(map.DisplayName)) ?? "Title Screen";
            //string name = map.m_506FA8D8;
            
            if (GetString(map.VariantName) != null) name = GetValidFilename(GetString(map.VariantName));

            LoudLog($"Extracting map {name}\\{GUID.Index(key):X}");

            // if (map.Gamemodes != null) {
            //     foreach (Common.STUGUID gamemodeGUID in map.Gamemodes) {
            //         STUGamemode gamemode = GetInstance<STUGamemode>(gamemodeGUID);
            //     }
            // }
            
            // TODO: MAP11 HAS CHANGED
            // TODO: MAP10 TOO?
            
            string mapPath = Path.Combine(basePath, "Maps", name, GUID.Index(key).ToString("X")) + Path.DirectorySeparatorChar;
            
            CreateDirectoryFromFile(mapPath);
            
            // if (map.UnknownArray != null) {
            //     Dictionary<ulong, List<TextureInfo>> textures = new Dictionary<ulong, List<TextureInfo>>();
            //     foreach (STUMap.STU_7D6D8405 stu_7D6D8405 in map?.UnknownArray) {
            //         ISTU overrideStu = OpenSTUSafe(stu_7D6D8405.Override);
            //         STUSkinOverride @override = GetInstance<STUSkinOverride>(stu_7D6D8405.Override);
            //         textures = FindLogic.Texture.FindTextures(textures, @override.SkinImage);
            //     }
            //     SaveLogic.Texture.Save(flags, Path.Combine(mapPath, "override"), textures);
            // }
            
            OWMDLWriter modelWriter = new OWMDLWriter();
            OWMap14Writer owmap = new OWMap14Writer();
            
            FindLogic.Combo.ComboInfo info = new FindLogic.Combo.ComboInfo();
            LoudLog("\tFinding");
            FindLogic.Combo.Find(info, map.MapDataResource1);

            //using (Stream mapEntitiesStream = OpenFile(map.GetDataKey((byte)Enums.teMAP_PLACEABLE_TYPE.TEXT))) {
            //    //WriteFile(mapEntitiesStream, Path.Combine(mapPath, "2.0BC"));
            //    mapEntitiesStream.Position = 0;
            //    teMapPlaceableData mapChunk = new teMapPlaceableData(mapEntitiesStream);
            //}

            //for (ushort i = 0; i < 255; i++) {
            //    using (Stream mapChunkStream = OpenFile(map.GetDataKey(i))) {
            //        if (mapChunkStream == null) continue;
            //        WriteFile(mapChunkStream, Path.Combine(mapPath, $"{(Enums.teMAP_PLACEABLE_TYPE)i}.0BC"));
            //    }
            //}

            /*using (Stream mapStream = OpenFile(map.GetDataKey(1))) {
                STULib.Types.Map.Map mapData = new STULib.Types.Map.Map(mapStream, BuildVersion);
                using (Stream map2Stream = OpenFile(map.GetDataKey(2))) {
                    if (map2Stream == null) return;
                    STULib.Types.Map.Map map2Data = new STULib.Types.Map.Map(map2Stream, BuildVersion);
                    using (Stream map8Stream = OpenFile(map.GetDataKey(8))) {
                        STULib.Types.Map.Map map8Data = new STULib.Types.Map.Map(map8Stream, BuildVersion);
                        using (Stream mapEntitiesStream = OpenFile(map.GetDataKey(0xB))) {
                            STULib.Types.Map.Map mapEntities =
                                new STULib.Types.Map.Map(mapEntitiesStream, BuildVersion, true);

                            mapEntitiesStream.Position =
                                (long) (Math.Ceiling(mapEntitiesStream.Position / 16.0f) * 16); // Future proofing (?)

                            for (int i = 0; i < mapEntities.Records.Length; ++i) {
                                if (mapEntities.Records[i] != null &&
                                    mapEntities.Records[i].GetType() != typeof(MapEntity)) {
                                    continue;
                                }

                                MapEntity mapEntity = (MapEntity) mapEntities.Records[i];

                                if (mapEntity == null) continue;
                                FindLogic.Combo.Find(info, mapEntity.Header.Entity);
                                STUModelComponent component =
                                    GetInstance<STUModelComponent>(mapEntity.Header.Entity);

                                if (component == null) continue;
                                mapEntity.ModelLook = component.Look;
                                mapEntity.Model = component.Model;
                                mapEntities.Records[i] = mapEntity;
                            }

                            using (Stream mapLStream = OpenFile(map.GetDataKey(9))) {
                                STULib.Types.Map.Map mapLData = new STULib.Types.Map.Map(mapLStream, BuildVersion);
                                using (Stream outputStream = File.Open(Path.Combine(mapPath, $"{name}.owmap"),
                                    FileMode.Create, FileAccess.Write)) {
                                    owmap.Write(outputStream, mapData, map2Data, map8Data, mapEntities, mapLData, name,
                                        modelWriter, info);
                                }
                            }
                        }
                    }
                }
            }*/

            //STULib.Types.Map.Map mapEntities;
            //
            //using (Stream mapEntitiesStream = OpenFile(map.GetDataKey(0xB))) {
            //    mapEntities =
            //        new STULib.Types.Map.Map(mapEntitiesStream, uint.MaxValue, true);
            //}

            teMapPlaceableData placeableModelGroups = GetPlaceableData(map, Enums.teMAP_PLACEABLE_TYPE.MODEL_GROUP);
            teMapPlaceableData placeableSingleModels = GetPlaceableData(map, Enums.teMAP_PLACEABLE_TYPE.SINGLE_MODEL);
            teMapPlaceableData placeable8 = GetPlaceableData(map, 8);
            teMapPlaceableData placeableLights = GetPlaceableData(map, Enums.teMAP_PLACEABLE_TYPE.LIGHT);
            teMapPlaceableData placeableEntities = GetPlaceableData(map, Enums.teMAP_PLACEABLE_TYPE.ENTITY);

            owmap.Write(new MemoryStream(), placeableModelGroups, placeableSingleModels, placeable8, placeableEntities,
                placeableLights, name, info);

            {
                FindLogic.Combo.Find(info, map.ImageResource1);
                FindLogic.Combo.Find(info, map.ImageResource2);
                FindLogic.Combo.Find(info, map.ImageResource3);

                if (map.Gamemodes != null) {
                    foreach (Common.STUGUID gamemodeGUID in map.Gamemodes) {
                        STUGameMode gameMode = GetInstanceNew<STUGameMode>(gamemodeGUID);
                        if (gameMode == null) continue;

                        FindLogic.Combo.Find(info, gameMode.m_6EB38130);  // 004
                        FindLogic.Combo.Find(info, gameMode.m_CF63B633);  // 01B

                        foreach (STUGameModeTeam team in gameMode.m_teams) {
                            FindLogic.Combo.Find(info, team.m_76E8C82A);  // 01B
                            FindLogic.Combo.Find(info, team.m_A2781AA4);  // 01B
                        }
                    }
                }
            }

            FindLogic.Combo.Find(info, map.EffectAnnouncer);
            info.SetEffectName(map.EffectAnnouncer, "LoadAnnouncer");
            FindLogic.Combo.Find(info, map.EffectMusic);
            info.SetEffectName(map.EffectMusic, "LoadMusic");
            
            LoudLog("\tSaving");
            Combo.Save(flags, mapPath, info);
            Combo.SaveLooseTextures(flags, Path.Combine(mapPath, "Textures"), info);
            
            // if (extractFlags.ConvertModels) {
            //     string physicsFile = Path.Combine(mapPath, "Models", "physics", "physics.owmdl");
            //     // CreateDirectoryFromFile(physicsFile);
            //     // using (Stream map10Stream = OpenFile(map.GetDataKey(0x10))) {
            //     //     Map10 physics = new Map10(map10Stream);
            //     //     using (Stream outputStream = File.Open(physicsFile, FileMode.Create, FileAccess.Write)) {
            //     //         modelWriter.Write(physics, outputStream, new object[0]);
            //     //     }
            //     // }
            // }

            if (map.VoiceSet != null) {
                FindLogic.Combo.ComboInfo soundInfo = new FindLogic.Combo.ComboInfo();
                FindLogic.Combo.Find(soundInfo, map.VoiceSet);

                if (soundInfo.VoiceSets.ContainsKey(map.VoiceSet)) {
                    string soundPath = Path.Combine(mapPath, "Sound");
                    FindLogic.Combo.VoiceSetInfo voiceSetInfo = soundInfo.VoiceSets[map.VoiceSet];
                    Combo.SaveVoiceSet(flags, soundPath, soundInfo, voiceSetInfo);
                }
            }
            
            LoudLog("\tDone");
        }

        public static teMapPlaceableData GetPlaceableData(STUMap map, Enums.teMAP_PLACEABLE_TYPE modelGroup) {
            return GetPlaceableData(map, (byte) modelGroup);
        }

        public static teMapPlaceableData GetPlaceableData(STUMap map, byte type) {
            using (Stream stream = OpenFile(map.GetDataKey(type))) {
                if (stream == null) return null;
                return new teMapPlaceableData(stream);
            }
        }
    }
}