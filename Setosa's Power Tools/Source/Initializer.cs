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
		Settings.Default.Apply();
		Logger.Message("Initialized");
    }
}
