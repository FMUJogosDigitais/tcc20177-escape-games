using UnityEngine;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;

namespace Badin
{
	/// <summary>
	/// Extension class for Unity
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Extension method to check if a layer is in a layermask
		/// </summary>
		/// <param name="mask"></param>
		/// <param name="layer"></param>
		/// <returns></returns>
		public static bool LayerMaskContainsInLayer(this LayerMask mask, int layer)
		{
			return mask == (mask | (1 << layer));
		}

		/// <summary>
		/// Extension method to convert Float to Time
		/// </summary>
		/// <param name="toConvert">Time in Float</param>
		/// <param name="format">Format Mask</param>
		/// <returns>Time in String Format</returns>
		public static string FloatToTime(float toConvert, string format)
		{
			switch (format)
			{
				case "00":
					return string.Format("{00}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 10) % 10));//miliseconds
				case "00.0":
					return string.Format("{0:00}: {1:0}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 10) % 10));//miliseconds

				case "#0.0":
					return string.Format("{0:#0}:{1:0}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 10) % 10));//miliseconds

				case "00.00":
					return string.Format("{0:00}:{1:00}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 100) % 100));//miliseconds

				case "00.000":
					return string.Format("{0:00}:{1:000}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

				case "#00.000":
					return string.Format("{0:#00}:{1:000}",
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

				case "#0:00":
					return string.Format("{0:#0}:{1:00}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60);//seconds

				case "#00:00":
					return string.Format("{0:#00}:{1:00}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60);//seconds

				case "0:00.0":
					return string.Format("{0:0}:{1:00}.{2:0}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 10) % 10));//miliseconds

				case "#0:00.0":
					return string.Format("{0:#0}:{1:00}.{2:0}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 10) % 10));//miliseconds

				case "0:00.00":
					return string.Format("{0:0}:{1:00}.{2:00}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 100) % 100));//miliseconds

				case "#0:00.00":
					return string.Format("{0:#0}:{1:00}.{2:00}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 100) % 100));//miliseconds

				case "0:00.000":
					return string.Format("{0:0}:{1:00}.{2:000}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

				case "#0:00.000":
					return string.Format("{0:#0}:{1:00}.{2:000}",
						Mathf.Floor(toConvert / 60),//minutes
						Mathf.Floor(toConvert) % 60,//seconds
						Mathf.Floor((toConvert * 1000) % 1000));//miliseconds

				case "#0:00:00.00":
					return string.Format("{0:#0}:{1:00}:{2:00}.{3:00}",
						Mathf.Floor(toConvert / 3600),//hours
						Mathf.Floor(toConvert / 60),  //minutes
						Mathf.Floor(toConvert) % 60,  //seconds
						Mathf.Floor((toConvert * 100) % 100));//miliseconds
			}
			return "error";
		}

		/// <summary>
		/// Extension method to convert from (string) "hours:minutes:seconds.miliseconds" to float
		/// </summary>
		/// <param name="timeString"></param>
		/// <returns></returns>
		public static float StringTimeToFloat(string timeString)
		{
			float timeFloat;

			string[] times = timeString.Split(':', '.');

			float timeHours = float.Parse(times[0]);
			float timeMinutes = float.Parse(times[1]);
			float timeSeconds = float.Parse(times[2]);
			float timeMiliseconds = float.Parse(times[3]);

			timeFloat = (timeHours * 3600) + (timeMinutes * 60) + timeSeconds + (timeMiliseconds * 0.01f);

			return timeFloat;
		}

		/// <summary>
		/// Extension method to convert from videoTime (double) to time (string) as "hours:minutes:seconds.miliseconds"
		/// </summary>
		/// <param name="videoTime"></param>
		/// <returns>String no formato "#0:00.00")</returns>
		public static string VideoTimeToString(double videoTime)
		{
			return FloatToTime((float)videoTime, "#0:00:00.00");
		}

		public static bool isFading;
		public static float fadeDuration = 1f;
		/// <summary>
		/// Extension method to fade a canvasGroup
		/// </summary>
		/// <param name="canvasGroup"></param>
		/// <param name="finalAlpha"></param>
		/// <returns></returns>
		public static IEnumerator FadeCanvas(CanvasGroup canvasGroup, float finalAlpha)
		{
			if (isFading)
			{
				canvasGroup.alpha = finalAlpha;
				isFading = false;
				yield break;
			}
			isFading = true;
			canvasGroup.blocksRaycasts = true;

			float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeDuration;

			while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
			{
				canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
					fadeSpeed * Time.deltaTime);

				yield return null;
			}

			isFading = false;
			canvasGroup.blocksRaycasts = false;
		}

		/// <summary>
		/// Crops the string to a maximum lenght
		/// </summary>
		/// <param name="value"></param>
		/// <param name="maxLength"></param>
		/// <returns></returns>
		public static string WithMaxLength(this string value, int maxLength)
		{
			if (value == null)
			{
				return null;
			}
			if (maxLength < 0)
			{
				return "";
			}
			return value.Substring(0, Mathf.Min(value.Length, maxLength));
		}

		/// <summary>
		/// Extension method to shuffle a list
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		public static void Shuffle<T>(this IList<T> list)
		{
			RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
			int n = list.Count;
			while (n > 1)
			{
				byte[] box = new byte[1];
				do provider.GetBytes(box);
				while (!(box[0] < n * (System.Byte.MaxValue / n)));
				int k = (box[0] % n);
				n--;
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		/// <summary>
		/// Check if an array of booleans are all true
		/// </summary>
		/// <param name="array">Array of bools</param>
		/// <returns></returns>
		public static bool AreAllTrue(bool[] array)
		{
			foreach (bool b in array)
			{
				if (!b) return false;
			}
			return true;
		}

		// More info about bit processing can be found at: http://derekwill.com/2015/03/05/bit-processing-in-c/
		//
		//
		// The main purpose of this class is to make it easier to save and load bool values using PlayerPrefs
		// It convert a bool array to a single int and vice-versa
		//
		//
		// HOW TO
		//
		// LOAD
		// myBool = BoolCasting.IntToBoolArray(PlayerPrefs.GetInt("BoolCast"), myBool.Length);
		//
		// SAVE
		// PlayerPrefs.SetInt("BoolCast", BoolCasting.BoolArrayToInt(myBool));


		/// <summary>
		/// Convert a bool array to a single int
		/// </summary>
		/// <param name="array">The bool array to be used in the conversion</param>
		/// <returns>The converted int</returns>
		public static int BoolArrayToInt(bool[] array)
		{
			int result = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i])
					result = result | (1 << i);
			}
			return result;
		}

		/// <summary>
		/// Convert a single int to a bool array
		/// </summary>
		/// <param name="source">The int to be used in the conversion</param>
		/// <param name="arrayLenght">The desired lenght of the bool, this prevent having to return an array of lenght 32</param>
		/// <returns>The converted bool array</returns>
		public static bool[] IntToBoolArray(int source, int arrayLenght)
		{
			bool[] result = new bool[arrayLenght];
			for (int i = 0; i < arrayLenght; i++)
			{
				result[i] = (source & (1 << i)) != 0;
			}
			return result;
		}

		/// <summary>
		/// Change just one bit in the int based on the bool value
		/// </summary>
		/// <param name="source">The int to be updated</param>
		/// <param name="index">The bit position to be changed (Usually it's used "myBool[index]")</param>
		/// <param name="value">The desired value</param>
		/// <returns>Return the updated int</returns>
		public static int BoolToIntBit(int source, int index, bool value)
		{
			return value ? source | (1 << index) : source & ~(1 << index);
		}

		/// <summary>
		/// Returns a bool based on the bit value from the desired position
		/// </summary>
		/// <param name="source">Int used to find the bool value</param>
		/// <param name="index">The bit position to be checked (Usually it's used "myBool[index]")</param>
		/// <returns>The value found in the bit</returns>
		public static bool IntBitToBool(int source, int index)
		{
			return (source & (1 << index)) != 0;
		}
	}
}


