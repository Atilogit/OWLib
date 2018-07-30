﻿using System;
using DataTool.Helper;
using DataTool.JSON;
using Newtonsoft.Json;
using TankLib.STU.Types;
using TankLib.STU.Types.Enums;
using static DataTool.Helper.IO;

namespace DataTool.DataModels {
    [JsonObject(MemberSerialization.OptOut)]
    public class Loadout {
        public string Name;
        public string Description;
        public string Category;
        
        [JsonConverter(typeof(GUIDConverter))]
        public ulong GUID;
        [JsonConverter(typeof(GUIDConverter))]
        public ulong MovieGUID;

        public Loadout(ulong key) {
            STULoadout loadout = STUHelper.GetInstanceNew<STULoadout>(key);
            if (loadout == null) return;
            Init(key, loadout);
        }

        public Loadout(ulong key, STULoadout loadout) {
            Init(key, loadout);
        }

        private void Init(ulong key, STULoadout loadout) {
            GUID = key;
            MovieGUID = loadout.m_infoMovie;
            
            Category = Enum.GetName(typeof(LoadoutCategory), loadout.m_category);
            
            Name = GetString(loadout.m_name);
            Description = GetString(loadout.m_description);
        }
    }
}