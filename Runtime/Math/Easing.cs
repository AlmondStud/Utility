using UnityEngine;

namespace Almond {
	public enum Ease {
		InSine,
		OutSine,
		InOutSine,
		InQuad,
		OutQuad,
		InOutQuad,
		InCubic,
		OutCubic,
		InOutCubic,
		InQuart,
		OutQuart,
		InOutQuart,
		InQuint,
		OutQuint,
		InOutQuint,
		InExpo,
		OutExpo,
		InOutExpo,
		InCirc,
		OutCirc,
		InOutCirc,
		InBack,
		OutBack,
		InOutBack,
		InElastic,
		OutElastic,
		InOutElastic,
		InBounce,
		OutBounce,
		InOutBounce,
		Max
	}

	public class Easing {
		public static float GetEaseValue(Ease ease, float ratio) {
			var x = Mathf.Clamp(ratio, 0, 1);
			const float c1 = 1.70158f;
			const float c2 = c1 * 1.525f;
			const float c3 = c1 + 1;
			const float c4 = (2 * Mathf.PI) / 3f;
			const float c5 = (2 * Mathf.PI) / 4.5f;

			const float n1 = 7.5625f;
			const float d1 = 2.75f;

			return ease switch {
				Ease.InSine => 1f - Mathf.Cos((x * Mathf.PI) * 0.5f),
				Ease.OutSine => Mathf.Sin((x * Mathf.PI) * 0.5f),
				Ease.InOutSine => -Mathf.Cos((x * Mathf.PI) - 1f) * 0.5f,
				Ease.InQuad => x * x,
				Ease.OutQuad => 1 - (1 - x) * (1 - x),
				Ease.InOutQuad => x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2,
				Ease.InCubic => x * x * x,
				Ease.OutCubic => 1 - Mathf.Pow(1 - x, 3),
				Ease.InOutCubic => x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2,
				Ease.InQuart => x * x * x * x,
				Ease.OutQuart => 1 - Mathf.Pow(1 - x, 4),
				Ease.InOutQuart => x < 0.5 ? 8 * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 4) / 2,
				Ease.InQuint => x * x * x * x * x,
				Ease.OutQuint => 1 - Mathf.Pow(1 - x, 5),
				Ease.InOutQuint => x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2,
				Ease.InExpo => x == 0 ? 0 : Mathf.Pow(2, 10 * x - 10),
				Ease.OutExpo => x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x),
				Ease.InOutExpo => x == 0 ? 0
								  : x == 1 ? 1
								  : x < 0.5 ? Mathf.Pow(2, 20 * x - 10) / 2
								  : (2 - Mathf.Pow(2, -20 * x + 10)) / 2,
				Ease.InCirc => 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2)),
				Ease.OutCirc => Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2)),
				Ease.InOutCirc => x < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * x, 2))) / 2
								  : (Mathf.Sqrt(1 - Mathf.Pow(-2 * x + 2, 2)) + 1) / 2,
				Ease.InBack => c3 * x * x * x - c1 * x * x,
				Ease.OutBack => 1 + c3 * Mathf.Pow(x - 1, 3) + c1 * Mathf.Pow(x - 1, 2),
				Ease.InOutBack => x < 0.5 ? (Mathf.Pow(2 * x, 2) * ((c2 + 1) * 2 * x - c2)) / 2
								  : (Mathf.Pow(2 * x - 2, 2) * ((c2 + 1) * (x * 2 - 2) + c2) + 2) / 2,
				Ease.InElastic => x == 0 ? 0
								  : x == 1 ? 1
								  : -Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4),
				Ease.OutElastic => x == 0 ? 0
								  : x == 1 ? 1
								  : Mathf.Pow(2, -10 * x) * Mathf.Sin((x * 10 - 0.75f) * c4) + 1,
				Ease.InOutElastic => x == 0 ? 0
								  : x == 1 ? 1
								  : x < 0.5 ? -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2
								  : (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1,
				Ease.InBounce => 1 - GetEaseValue(Ease.OutBounce, 1 - x),
				Ease.OutBounce => x switch {
					< (1 / d1) => n1 * x * x,
					< (2 / d1) => n1 * (x -= 1.5f / d1) * x + 0.75f,
					< (2.5f / d1) => n1 * (x -= 2.25f / d1) * x + 0.9375f,
					_ => n1 * (x -= 2.625f / d1) * x + 0.984375f
				},
				Ease.InOutBounce => x < 0.5f ? (1 - GetEaseValue(Ease.OutBounce, 1 - 2 * x)) / 2
								 : (1 + GetEaseValue(Ease.OutBounce, 2 * x - 1)) / 2,
				_ => x
			};
		}
	}
}