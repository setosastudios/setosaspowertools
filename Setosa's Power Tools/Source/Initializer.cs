using System;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace Setosa.PowerTools;

[StaticConstructorOnStartup]
public static class Initializer {
	static Initializer() {
#if DEBUG
		Harmony.DEBUG = true;
#endif
		var harmony = new Harmony(ThisAssembly.Project.PackageId);
		harmony.PatchAll(Assembly.GetExecutingAssembly());
		LongEventHandler.QueueLongEvent(
			() => {
				var mods = ModsConfig.ActiveModsInLoadOrder
					.Select(m => m.PackageId)
					.ToHashSet(StringComparer.OrdinalIgnoreCase);
				if (mods.Contains("TreeChoppingSpeed.velcroboy333")) {
					const string chainsawDef = "AdvancedTool_IndustrialChainsaw";
                    foreach (var preset in Presets.All) {
						if (!preset.TryGetValue(chainsawDef, "PlantWorkSpeed", out var value))
							continue;
						preset.Remove(chainsawDef, "PlantWorkSpeed");
						preset.Add(chainsawDef, "VBY_TreeChopWorkSpeed", value);
					}
				}
				Settings.Default.Apply();
				Logger.Message("Initialized");
			},
			$"{ThisAssembly.Info.Title}-Apply",
			true,
			null
		);
	}
}