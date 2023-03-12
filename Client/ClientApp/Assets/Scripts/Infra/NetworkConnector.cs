using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Infra
{
    public class NetworkConnector : MonoBehaviour
    {
        private const string SERVER_URL = "https://localhost:7203/game";
        public static NetworkConnector Instance { get; private set; }

        private int? _highScore;
        private int _sequenceId = 0;
        private string _uid;

        private void OnEnable()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void SetUId(string uid)
        {
            _uid = uid;
        }

        public void SubmitHighScore(int highScore, Action onComplete)
        {
            _highScore = highScore;
            
            Post(_sequenceId++, _uid, new JObject
            {
                { "action", "set_player_score" },
                { "score", highScore }
            }, _ => { onComplete?.Invoke(); });
        }

        public void GetHighScore(Action<int> onComplete)
        {
            if (_highScore.HasValue)
            {
                onComplete?.Invoke(_highScore.Value);
                return;
            }
            
            Post(_sequenceId++, _uid, new JObject
            {
                { "action", "get_player_score" }
            }, x =>
            {
                _highScore = x.Value<int>("score");
                onComplete?.Invoke(_highScore.Value);
            });
        }

        public void RequestHighScoreBoard(Action<UserHighScore> onComplete)
        {
            // todo
        }

        private void Post(int seqId, string uid, JObject action, Action<JToken> onComplete = null)
        {
            var o = new JObject
            {
                { "seqId", seqId },
                { "uid", uid },
                { "actions", new JArray(action) }
            };

            IEnumerator PostInternal(string json)
            {
                using UnityWebRequest www = UnityWebRequest.Post(SERVER_URL, json);
                
                var jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                www.uploadHandler = new UploadHandlerRaw(jsonToSend);
                www.downloadHandler = new DownloadHandlerBuffer();
                www.SetRequestHeader("Content-Type", "application/json");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                    Debug.LogError(www.error);
                else
                {
                    var download = www.downloadHandler.text;
                    Debug.Log(download);
                    onComplete?.Invoke(JArray.Parse(download)[0]);
                }
            }
            
            StartCoroutine(PostInternal(o.ToString()));
        }
    }
}