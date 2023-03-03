﻿using Moonstorm.Loaders;
using R2API.ScriptableObjects;
using RoR2;
using RoR2.ContentManagement;
using System;
using System.Linq;
using UnityEngine;

namespace IEye.RulersOfTheRedPlane
{
    public class RRPContent : ContentLoader<RRPContent>
    {
        public static class Artifacts
        {

        }
        public static class Items
        {
            public static ItemDef FourDimensionalDagger;
        }
        public override string identifier => RulersOfTheRedPlaneMain.GUID;

        public override R2APISerializableContentPack SerializableContentPack { get; protected set; } = RRPAssets.LoadAsset<R2APISerializableContentPack>("ContentPack", RRPBundle.Main);

        public override Action[] LoadDispatchers { get; protected set; }

        public override Action[] PopulateFieldsDispatchers { get; protected set; }

        public override void Init()
        {
            base.Init();

            LoadDispatchers = new Action[] 
            {
            delegate
            {
                new Modules.Items().Initialize();
            },
            delegate
            {
                DefNotSS2Log.Info($"Populating entity state array");
                GetType().Assembly.GetTypes()
                                      .Where(type => typeof(EntityStates.EntityState).IsAssignableFrom(type))
                                      .ToList()
                                      .ForEach(state => HG.ArrayUtils.ArrayAppend(ref SerializableContentPack.entityStateTypes, new EntityStates.SerializableEntityStateType(state)));
            },
            delegate{
                    DefNotSS2Log.Info($"Populating effect prefabs");
                    SerializableContentPack.effectPrefabs = SerializableContentPack.effectPrefabs.Concat(RRPAssets.LoadAllAssetsOfType<GameObject>(RRPBundle.All)
                    .Where(go => go.GetComponent<EffectComponent>()))
                    .ToArray();
            },
            delegate
                {
                    DefNotSS2Log.Info($"Populating EntityStateConfigurations");
                    SerializableContentPack.entityStateConfigurations = RRPAssets.LoadAllAssetsOfType<EntityStateConfiguration>(RRPBundle.All);
                }
            };
            PopulateFieldsDispatchers = new Action[]
            {
                delegate
                {
                    PopulateTypeFields(typeof(Artifacts), ContentPack.artifactDefs);
                },
                delegate
                {
                    PopulateTypeFields(typeof(Items), ContentPack.itemDefs);
                },
            };
        }
    }
}