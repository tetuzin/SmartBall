using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

using ShunLib.Dict;
using ShunLib.Utils.Debug;

namespace ShunLib.Manager.Video
{
    public class VideoManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("動画再生コンポーネント")] private VideoPlayerTable _videoPlayerTable = default;
        [SerializeField, Tooltip("動画リスト")] private VideoClipTable _videoClipTable = default;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public AudioSource AudioSource { get; set; }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        // 選択中のビデオクリップ
        private VideoClip _videoClip = default;
        // 動画終了時処理コールバック
        private Dictionary<VideoPlayer, Action<VideoPlayer>> _videoEndCallbackTable = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            InitializeVideoPlayer();
        }

        // 各パラメータの設定
        public void SetParameter(VideoPlayerTable videoPlayerTable = null, VideoClipTable videoClipTable = null)
        {
            _videoClipTable = videoClipTable;
            _videoPlayerTable = videoPlayerTable;
        }

        // VideoPlayerの取得
        public VideoPlayer GetVideoPlayer(string playerKey)
        {
            return _videoPlayerTable.GetValue(playerKey);
        }

        // ビデオの再生(VideoClip)
        public void PlayVideoClip(
            string playerKey,
            string clipKey,
            Action beforeCallback = null,
            Action<VideoPlayer> endCallback = null
        )
        {
            if (!_videoPlayerTable.GetKeyList().Contains(playerKey))
            {
                DebugUtils.LogError("VideoPlayer:Key[" + playerKey + "]は設定されていません");
                beforeCallback?.Invoke();
                return;
            }
            if (!_videoClipTable.GetKeyList().Contains(clipKey))
            {
                DebugUtils.LogError("VideoClip:Key[" + clipKey + "]は設定されていません");
                beforeCallback?.Invoke();
                endCallback?.Invoke(_videoPlayerTable.GetValue(playerKey));
                return;
            }
            SetVideoClip(playerKey, clipKey, (vp) => {
                PlayVideo(playerKey, endCallback);
                beforeCallback?.Invoke();
            });
        }

        // ビデオの再生(URL)
        public void PlayVideoURL(
            string playerKey,
            string clipKey,
            Action beforeCallback = null,
            Action<VideoPlayer> endCallback = null
        )
        {
            if (!_videoPlayerTable.GetKeyList().Contains(playerKey))
            {
                DebugUtils.LogError("VideoPlayer:Key[" + playerKey + "]は設定されていません");
                beforeCallback?.Invoke();
                return;
            }
            if (!_videoClipTable.GetKeyList().Contains(clipKey))
            {
                DebugUtils.LogError("VideoClip:Key[" + clipKey + "]は設定されていません");
                beforeCallback?.Invoke();
                endCallback?.Invoke(_videoPlayerTable.GetValue(playerKey));
                return;
            }
            SetVideoURL(playerKey, clipKey, (vp) => {
                beforeCallback?.Invoke();
                PlayVideo(playerKey, endCallback);
            });
        }

        // ビデオのループ再生(VideoClip)
        public void PlayLoopVideoClip(string playerKey, string clipKey, Action beforeCallback = null)
        {
            if (!_videoPlayerTable.GetKeyList().Contains(playerKey))
            {
                DebugUtils.LogError("VideoPlayer:Key[" + playerKey + "]は設定されていません");
                beforeCallback?.Invoke();
                return;
            }
            if (!_videoClipTable.GetKeyList().Contains(clipKey))
            {
                DebugUtils.LogError("VideoClip:Key[" + clipKey + "]は設定されていません");
                beforeCallback?.Invoke();
                return;
            }
            SetVideoClip(playerKey, clipKey, (vp) => {
                beforeCallback?.Invoke();
                PlayLoopVideo(playerKey);
            });
        }

        // ビデオのループ再生(URL)
        public void PlayLoopVideoURL(string playerKey, string url, Action beforeCallback = null)
        {
            if (!_videoPlayerTable.GetKeyList().Contains(playerKey))
            {
                DebugUtils.LogError("VideoPlayer:Key[" + playerKey + "]は設定されていません");
                beforeCallback?.Invoke();
                return;
            }
            SetVideoURL(playerKey, url, (vp) => {
                beforeCallback?.Invoke();
                PlayLoopVideo(playerKey);
            });
        }

        // ビデオの一時停止
        public void PauseVideo(string playerKey)
        {
            _videoPlayerTable.GetValue(playerKey).Pause();
        }

        // ビデオの停止
        public void StopVideo(string playerKey)
        {
            _videoPlayerTable.GetValue(playerKey).Stop();
        }

        // ---------- Private関数 ----------

        // VideoPlayer初期化
        private void InitializeVideoPlayer()
        {
            _videoEndCallbackTable = new Dictionary<VideoPlayer, Action<VideoPlayer>>();
            foreach (VideoPlayer vp in _videoPlayerTable.GetValueList())
            {
                if (vp == null || vp == default) continue;
                vp.isLooping = false;
                vp.playOnAwake = false;
                vp.source = VideoSource.VideoClip;
                _videoEndCallbackTable.Add(vp, null);
                vp.loopPointReached += FinishPlayingVideo;
            }
        }

        // ビデオクリップの設定
        private void SetVideoClip(string playerKey, string clipKey, Action<VideoPlayer> callback = null)
        {
            _videoClip = _videoClipTable.GetValue(clipKey);
            _videoPlayerTable.GetValue(playerKey).source = VideoSource.VideoClip;
            _videoPlayerTable.GetValue(playerKey).clip = _videoClip;
            _videoPlayerTable.GetValue(playerKey).audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayerTable.GetValue(playerKey).EnableAudioTrack (0, true);
            _videoPlayerTable.GetValue(playerKey).SetTargetAudioSource(0, AudioSource);
            _videoPlayerTable.GetValue(playerKey).prepareCompleted += (vp) => {
                callback?.Invoke(vp);
            };
            _videoPlayerTable.GetValue(playerKey).Prepare();
        }

        // 動画URLの設定
        private void SetVideoURL(string playerKey, string url, Action<VideoPlayer> callback = null)
        {
            _videoPlayerTable.GetValue(playerKey).source = VideoSource.Url;
            _videoPlayerTable.GetValue(playerKey).url = url;
            _videoPlayerTable.GetValue(playerKey).audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayerTable.GetValue(playerKey).EnableAudioTrack (0, true);
            _videoPlayerTable.GetValue(playerKey).SetTargetAudioSource(0, AudioSource);
            _videoPlayerTable.GetValue(playerKey).prepareCompleted += (vp) => {
                callback?.Invoke(vp);
            };
            _videoPlayerTable.GetValue(playerKey).Prepare();
        }

        // ビデオの再生
        private void PlayVideo(string playerKey, Action<VideoPlayer> callback = null)
        {
            _videoPlayerTable.GetValue(playerKey).isLooping = false;
            _videoEndCallbackTable[_videoPlayerTable.GetValue(playerKey)] = callback;
            _videoPlayerTable.GetValue(playerKey).Play();
        }

        // ビデオのループ再生
        private void PlayLoopVideo(string playerKey)
        {
            _videoPlayerTable.GetValue(playerKey).isLooping = true;
            _videoPlayerTable.GetValue(playerKey).Play();
        }

        // ---------- protected関数 ---------

        // 動画終了時の処理
        protected void FinishPlayingVideo(VideoPlayer vp)
        {
            Action<VideoPlayer> videoEndCallback = _videoEndCallbackTable[vp];
            videoEndCallback?.Invoke(vp);
        }
    }
}