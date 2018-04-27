using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NanoBuffers;

public class GameSerialize : MonoBehaviour {
	private const byte CMD_HANDSHAKE = 0;
	private const byte CMD_PIECE = 1;
	private const byte CMD_LOSE = 2;

	public static byte[] toBytes(Hashtable values) {
		NanoWriter nanoWriter = new NanoWriter ();

		string name = (string) values["name"];
		switch(name) {
		case "handshake":
			nanoWriter.putInt (CMD_HANDSHAKE).putString ((string) values["v"]).putString((string)values["ImgStr"]);
			break;

		case "piece":
			nanoWriter.putInt (CMD_PIECE).putInt ((int) values ["FromX"]).putInt((int)values["FromY"]).putInt((int)values["ToX"]).putInt((int)values["ToY"]).putString ((string) values ["Move"]).putString((string)values["YidongOrChizi"]).putInt((int)values["Regame"]).putInt((int)values["GoBack"]);
			break;

		case "lose":
			nanoWriter.putInt (CMD_LOSE).putInt ((byte) values["v"]);
			break;

		default:
			Debug.Log ("GameSerialize::toBytes 未支持的命令: " + name);
			break;
		}

		return nanoWriter.getBytes ();
	}

	public static Hashtable fromBytes(byte[] buf) {
		NanoReader nanoReader = new NanoReader (buf);

		Hashtable values = new Hashtable ();

		byte cmd;
		nanoReader.getInt (out cmd);

		switch(cmd) {
		case CMD_HANDSHAKE:
			{
				string v;
                string ImgStr;
				nanoReader.getString (out v);
                nanoReader.getString(out ImgStr);

                values.Add ("name", "handshake");
				values.Add ("v", v);
                values.Add("ImgStr", ImgStr);
			}
			break;

		case CMD_PIECE:
            int FromX;
            int FromY;
            int ToX;
            int ToY;
            string Move;
            string YidongOrChizi;
            int Regame;
            int GoBack;
			nanoReader.getInt (out FromX);
			nanoReader.getInt (out FromY);
            nanoReader.getInt(out ToX);
            nanoReader.getInt(out ToY);
            nanoReader.getString (out Move);
            nanoReader.getString(out YidongOrChizi);
            nanoReader.getInt(out Regame);
            nanoReader.getInt(out GoBack);

            values.Add ("name", "piece");
			values.Add ("FromX", FromX);
			values.Add ("FromY", FromY);
            values.Add("ToX", ToX);
            values.Add("ToY", ToY);
            values.Add ("Move", Move);
            values.Add("YidongOrChizi", YidongOrChizi);
            values.Add("Regame", Regame);
            values.Add("GoBack", GoBack);

            break;

		case CMD_LOSE:
			{
				byte v;
				nanoReader.getInt (out v);

				values.Add ("name", "lose");
				values.Add ("v", (byte) v);
			}
			break;

		default:
			Debug.Log ("GameSerialize::fromBytes 未支持的命令: " + cmd);
			break;
		}

		return values;
	}

}
