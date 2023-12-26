﻿using R2API.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Moonstorm;


namespace IEye.RRP.Modules
{
    public class Scenes : SceneModuleBase
    {
        public static Scenes Instance { get; set; }
        public override R2APISerializableContentPack SerializableContentPack { get; } = RRPContent.Instance.SerializableContentPack;

        public override void Initialize()
        {
            Instance = this;
            base.Initialize();
            RRPMain.logger.LogInfo($"Initializing Stages.");
            GetSceneBases();
        }
        public override IEnumerable<SceneBase> GetSceneBases()
        {
            base.GetSceneBases()
                .ToList()
                .ForEach(scene => AddScene(scene));
            return null;
        }
    }
}
