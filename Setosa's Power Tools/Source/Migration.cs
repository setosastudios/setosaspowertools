using System;
using HarmonyLib;
using Verse;

namespace Setosa.PowerTools;

[HarmonyPatch(typeof(BackCompatibility))]
internal class Migration {
	internal const string OLD_THING_PREFIX = "advancedTool_";

	internal const string NEW_THING_PREFIX = "AdvancedTool_";

	[HarmonyPatch(nameof(BackCompatibility.BackCompatibleDefName))]
	[HarmonyPostfix]
	internal static void BackCompatibleDefName_Postfix(Type defType, string defName, ref string? __result) {
		if (defType == typeof(ThingDef) && defName.StartsWith(OLD_THING_PREFIX)) {
			__result = NEW_THING_PREFIX + defName[OLD_THING_PREFIX.Length..];
			Logger.Message($"Migrated {defName} to {__result}");
		}
	}
}