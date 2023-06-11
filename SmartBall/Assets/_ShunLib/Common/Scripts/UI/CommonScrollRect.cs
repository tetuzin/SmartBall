using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace ShunLib.UI.Scroll
{
    public class CommonScrollRect : ScrollRect
    {
        // ---------- 定数宣言 ----------
        // ---------- ゲームオブジェクト参照変数宣言 ----------

        [SerializeField, Tooltip("縦スクロールバー")] public Scrollbar _verticalScrollbar = default;
        [SerializeField, Tooltip("横スクロールバー")] public Scrollbar _horizontalScrollbar = default;
        [SerializeField, Tooltip("ページ移動速度")] private float _slideSpeed = 0.5f;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------

        public bool IsMove
        {
            get { return _isMove; }
        }

        // ---------- クラス変数宣言 ----------
        // ---------- インスタンス変数宣言 ----------

        private bool _isMove = default;
        private int _curPage = default;
        private bool _isPause = default;
        private int _hIndex = default;
        private List<Vector2> _posList = default;

        // ---------- Unity組込関数 ----------
        // ---------- Public関数 ----------

        // 初期化
        public void Initialize()
        {
            RectTransform mainTrans = gameObject.GetComponent<RectTransform>();
            viewport.sizeDelta = mainTrans.sizeDelta;
            
            // AllRemove();
            ResetScrollPosition();
        }

        // ページスクロール機能の初期化
        public void InitializePageScroll(List<Vector2> posList)
        {
            _isMove = false;
            _posList = posList;
            _hIndex = posList.Count;
            _curPage = 0;

            SetVerticalScroll(false);
            SetHorizontalScroll(false);
        }

        // オブジェクトの追加
        public void AddContent(GameObject obj)
        {
            obj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            obj.transform.SetParent(content.transform);
        }
        
        // オブジェクトの全削除
        public void AllRemove()
        {
            foreach (Transform c in content.transform)
            {
                Destroy (c.gameObject);
            }
        }

        // 縦スクロールの設定
        public void SetVerticalScroll(bool b)
        {
            vertical = b;
        }

        // 横スクロールの設定
        public void SetHorizontalScroll(bool b)
        {
            horizontal = b;
        }

        // スクロール位置の初期化
        public void ResetScrollPosition()
        {
            SetVerticalPosition();
            SetHorizontalPosition();
        }

        // 縦スクロールの位置設定
        public void SetVerticalPosition(float position = 1.0f)
        {
            verticalNormalizedPosition = position;
        }

        // 横スクロールの位置設定
        public void SetHorizontalPosition(float position = 0.0f)
        {
            horizontalNormalizedPosition = position;
        }

        // ページ移動
        public void MovePage(int index)
        {
            if (_isMove) return;

            // _nextIndexの算出
            int nextPage = index;
            if (nextPage >= _hIndex)
            {
                nextPage = 0;
            }
            if (nextPage < 0)
            {
                nextPage = _hIndex - 1;
            }
            _isMove = true;

            // ページ移動
            float pos = _posList[nextPage].x - content.anchoredPosition.x;
            content.DOAnchorPos(_posList[nextPage],_slideSpeed)
            // .SetEase(Ease.InOutQuart)
            .OnComplete(() => {
                _curPage = nextPage;
                _isMove = false;
            });
        }

        // ---------- Private関数 ----------
        // ---------- protected関数 ---------
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CommonScrollRect))]
    public class CommonScrollRectEditor : UnityEditor.UI.ScrollRectEditor {
        public override void OnInspectorGUI() {
            base.OnInspectorGUI();
            var component = (CommonScrollRect) target;
            PropertyField(nameof(component._verticalScrollbar), "VerticalScrollbar");
            PropertyField(nameof(component._horizontalScrollbar), "HorizontalScrollbar");
            serializedObject.ApplyModifiedProperties();
        }

        private void PropertyField(string property, string label) {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label));
        }
    }
#endif
}

