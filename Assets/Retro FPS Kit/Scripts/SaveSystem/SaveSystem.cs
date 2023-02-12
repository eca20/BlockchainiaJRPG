using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

namespace FPSRetroKit
{
	public static class SaveSystem
	{
		#region Save Player Only
		public static void SavePlayer (PlayerHealth playerHealthScript)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + "/playerSave.sampletext";
			FileStream stream = new FileStream(path, FileMode.Create);

			PlayerData dataPlayer = new PlayerData(playerHealthScript); //Saving PlayerData Script for PlayerHealth

			formatter.Serialize(stream, dataPlayer);
			stream.Close();
		}

		public static PlayerData LoadPlayer()
		{
			string path = Application.persistentDataPath + "/playerSave.sampletext";
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				PlayerData dataPlayer = formatter.Deserialize(stream) as PlayerData;
				stream.Close();

				return dataPlayer;
			}
			else
			{
				Debug.LogError("Save File not found in" + path);
				return null;
			}
		}
		#endregion

		#region Save Weapons Ammo (separate scripts)
		public static void SaveWeapons(Pistol pistolScript, Shotgun shotgunScript, RocketLauncher rocketLauncherScript)
		{
			BinaryFormatter formatter = new BinaryFormatter();
			string path = Application.persistentDataPath + "/pistolSave.sampletext";
			FileStream stream = new FileStream(path, FileMode.Create);

			PlayerData dataWeapons = new PlayerData(pistolScript, shotgunScript, rocketLauncherScript); //Saving Weapon PistolScript

			formatter.Serialize(stream, dataWeapons);
			stream.Close();
		}

		public static PlayerData LoadWeapons()
		{
			string path = Application.persistentDataPath + "/pistolSave.sampletext";
			if (File.Exists(path))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);

				PlayerData dataWeapons = formatter.Deserialize(stream) as PlayerData;
				stream.Close();

				return dataWeapons;
			}
			else
			{
				Debug.LogError("Save File not found in" + path);
				return null;
			}
		}
		#endregion

	
	}
}
