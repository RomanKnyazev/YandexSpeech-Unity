using UnityEngine;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace TTS
{
    class YandexManager:MonoBehaviour
    {
		[SerializeField] private AudioSource audioSource;
		[SerializeField] private Button playButton;
		[SerializeField] private InputField inputFieldText;
 		private void Awake() {
			playButton.onClick.AddListener(SpeechToText);
		}

		public void SpeechToText()
		{
			StartCoroutine(YandexTest());
		}

        private IEnumerator YandexTest()
        {
        	const string iamToken = "PAST IAM TOKEN"; // Укажите IAM-токен.
            const string folderId = "PAST FOLDER ID"; // Укажите ID каталога.\

            WWWForm form = new WWWForm();
        	form.AddField("text", inputFieldText.text);
        	form.AddField("lang", "ru-RU");
        	form.AddField("folderId", folderId);

            using (UnityWebRequest www = UnityWebRequest.Post("https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize", form))
            {
            	www.SetRequestHeader("Authorization", "Bearer " + iamToken);
                yield return www.SendWebRequest();

                if (www.isNetworkError || www.isHttpError)
                {
                    Debug.Log(www.error);
                }
                else
                {
					byte[] data = www.downloadHandler.data;
					 File.WriteAllBytes(Application.dataPath + "/audio.ogg", data);
					audioSource.clip =  Resources.Load<AudioClip>(Application.dataPath + "/audio.ogg");
					audioSource.Play();
                }

        	}
        }
    }
}