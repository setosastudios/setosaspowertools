using System.Collections.Generic;
using System.Linq;
using Verse;

namespace Setosa.PowerTools;

public class Settings : ModSettings {
	public static Settings Default { get; internal set; } = null!;

	private StatsPreset _preset = StatsPreset.Normal;

	private ThingStatOffsetCollection? _customOffsets;

	public StatsPreset Preset {
		get => _preset;
		internal set => _preset = value;
	}

	public ThingStatOffsetCollection CustomOffsets {
		get => _customOffsets ??= Presets.Normal.Clone();
		internal set => _customOffsets = value;
	}

	public ThingStatOffsetCollection Offsets => Preset switch {
		StatsPreset.Nerfed => Presets.Nerfed,
		StatsPreset.Custom => CustomOffsets,
		_                  => Presets.Normal
	};

	public override void ExposeData() {
		base.ExposeData();
		Scribe_Values.Look(ref _preset, "preset");
		Scribe_Deep.Look(ref _customOffsets, "customOffsets");
		if (Scribe.mode == LoadSaveMode.LoadingVars && _customOffsets is not null) {
			// Older settings contain no entries for tools introduced after they were saved.
			var savedTools = new HashSet<string>(_customOffsets.Select(offset => offset.Item1));
			foreach (var offset in Presets.Normal) {
				if (!savedTools.Contains(offset.Item1))
					_customOffsets.Add(offset);
			}
		}
	}

	public void Apply() => Offsets.Apply();
}