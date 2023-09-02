/*
Generated from a ROR2EK Template. Feel free to remove this comment section.
0 = modName; 1 = Nicified mod name; 2 = authorName; 3 = using clauses; 4 = attributes; 
*/

using BepInEx;
using R2API;
using R2API.ScriptableObjects;
using R2API.Utils;
using R2API.Networking;
using R2API.ContentManagement;
using UnityEngine;
using Moonstorm;
using IEye.RRP.Items;

namespace IEye.RRP
{
    #region R2API
    [BepInDependency("com.bepis.r2api.dot")]
    [BepInDependency("com.bepis.r2api.networking")]
    [BepInDependency("com.bepis.r2api.prefab")]
    [BepInDependency("com.bepis.r2api.difficulty")]
    [BepInDependency("com.rune580.riskofoptions")]
    #endregion
    [BepInDependency("com.TeamMoonstorm.MoonstormSharedUtils", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [R2APISubmoduleDependency(
        nameof(DotAPI),
        nameof(PrefabAPI),
        nameof(NetworkingAPI))]
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class RRPMain : BaseUnityPlugin
	{
		public const string GUID = "com.I_Eye.RulersOfTheRedPlane";
		public const string MODNAME = "Rulers of the Red Plane";
		public const string VERSION = "0.0.5";

        public static RRPMain Instance;
        public static PluginInfo pluginInfo;
        public static DefNotSS2Log logger;
        

		private void Awake()
		{
			Instance = this;
            pluginInfo = Info;
            logger = new DefNotSS2Log(Logger);

            new RRPAssets().Init();
            new RRPConfig().Init();
            new RRPContent().Init();
            new RRPLanguage().Init();
            //ConfigurableFieldManager.AddMod(this);
            ConfigSystem.AddMod(this);
            
		}	
	}
}