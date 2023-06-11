using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace ShunLib.Scenario.CharacterView
{
    public class StandCharacter : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("キャンバスグループ")] protected CanvasGroup _canvasGroup = default;
        [SerializeField, Tooltip("キャラ画像")] protected Image _charaImage = default;
        [SerializeField, Tooltip("移動アニメーション")] protected bool _isMoveAnim = true;
        [SerializeField, Tooltip("移動時間")] protected float _moveTime = 0.5f;
        [SerializeField, Tooltip("立ち絵をフェードさせる")] protected bool _isFade = true;
        [SerializeField, Tooltip("フェード時間")] protected float _fadeTime = 0.5f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            _canvasGroup.alpha = 0f;
        }

        // 立ち絵を表示・非表示
        public void SetActive(bool b, bool isAnim = true)
        {
            if (_isFade && isAnim)
            {
                _canvasGroup.DOFade(
                    endValue: b ? 1f : 0f,
                    duration: _fadeTime
                );
            }
            else
            {
                _canvasGroup.alpha = b ? 1f : 0f;
            }
        }

        // 画像設定
        public void SetCharacterImage(Sprite sprite)
        {
            _charaImage.sprite = sprite;
            _charaImage.SetNativeSize();
        }

        // 座標移動
        public void MovePosition(Transform trans, Action callback = null)
        {
            // 非表示なら表示する
            if (_canvasGroup.alpha == 0f)
            {
                SetActive(true);
            }

            // 移動
            if (this.gameObject.transform.position == trans.position) return;
            if (_isMoveAnim)
            {
                this.gameObject.transform.DOMove(trans.position, _moveTime).OnComplete(() => {
                    callback?.Invoke();
                });
            }
            else
            {
                this.gameObject.transform.position = trans.position;
                callback?.Invoke();
            }
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }
}

