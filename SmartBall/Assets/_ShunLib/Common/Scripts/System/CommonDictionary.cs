using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
using Cinemachine;

using ShunLib.UI;
using ShunLib.Popup;
using ShunLib.Btn.Common;
using ShunLib.Toggle.Common;
using ShunLib.Particle;
using ShunLib.UI.Cutin;
using ShunLib.UI.Panel;
using ShunLib.UI.Input;
using ShunLib.Room;
using ShunLib.Lib3D.Block.Block;
using ShunLib.Lib3D.Block.Const;

namespace ShunLib.Dict
{
    [System.Serializable]
    public class CommonDictionary<T1, T2> : TableBase<T1, T2, KeyAndValue<T1, T2>>
    {

    }

    /// --------------------  UIManager  -------------------- ///
    /// キャンバス ///
    [System.Serializable]
    public class CanvasTable : TableBase<string, Canvas, CanvasPair>{}

    [System.Serializable]
    public class CanvasPair : KeyAndValue<string, Canvas>{}

    /// テキスト ///
    [System.Serializable]
    public class TextMeshProUGUITable : TableBase<string, TextMeshProUGUI, TextMeshProUGUIPair>{}

    [System.Serializable]
    public class TextMeshProUGUIPair : KeyAndValue<string, TextMeshProUGUI>{}

    /// ボタン ///
    [System.Serializable]
    public class CommonButtonTable : TableBase<string, CommonButton, CommonButtonPair>{}

    [System.Serializable]
    public class CommonButtonPair : KeyAndValue<string, CommonButton>{}

    /// トグル ///
    [System.Serializable]
    public class CommonToggleTable : TableBase<string, CommonToggle, CommonTogglePair>{}

    [System.Serializable]
    public class CommonTogglePair : KeyAndValue<string, CommonToggle>{}

    /// 画像 ///
    [System.Serializable]
    public class ImageTable : TableBase<string, Image, ImagePair>{}

    [System.Serializable]
    public class ImagePair : KeyAndValue<string, Image>{}

    /// キャンバスグループ ///
    [System.Serializable]
    public class CanvasGroupTable : TableBase<string, CanvasGroup, CanvasGroupPair>{}

    [System.Serializable]
    public class CanvasGroupPair : KeyAndValue<string, CanvasGroup>{}

    /// ポップアップ ///
    [System.Serializable]
    public class BasePopupTable : TableBase<string, BasePopup, BasePopupPair>{}

    [System.Serializable]
    public class BasePopupPair : KeyAndValue<string, BasePopup>{}

    /// AudioClip ///
    [System.Serializable]
    public class AudioClipTable : TableBase<string, AudioClip, AudioClipPair>{}

    [System.Serializable]
    public class AudioClipPair : KeyAndValue<string, AudioClip>{}

    /// VideoClip ///
    [System.Serializable]
    public class VideoClipTable : TableBase<string, VideoClip, VideoClipPair>{}

    [System.Serializable]
    public class VideoClipPair : KeyAndValue<string, VideoClip>{}

    /// VideoPlayer ///
    [System.Serializable]
    public class VideoPlayerTable : TableBase<string, VideoPlayer, VideoPlayerPair>{}

    [System.Serializable]
    public class VideoPlayerPair : KeyAndValue<string, VideoPlayer>{}

    /// ParticleSystem ///
    [System.Serializable]
    public class ParticleSystemTable : TableBase<string, ParticleSystem, ParticleSystemPair>{}

    [System.Serializable]
    public class ParticleSystemPair : KeyAndValue<string, ParticleSystem>{}

    /// CommonParticle ///
    [System.Serializable]
    public class CommonParticleTable : TableBase<string, CommonParticle, CommonParticlePair>{}

    [System.Serializable]
    public class CommonParticlePair : KeyAndValue<string, CommonParticle>{}

    /// TextAsset ///
    [System.Serializable]
    public class TextAssetTable : TableBase<string, TextAsset, TextAssetPair>{}

    [System.Serializable]
    public class TextAssetPair : KeyAndValue<string, TextAsset>{}

    /// CinemachineVirtualCamera ///
    [System.Serializable]
    public class CinemachineVirtualCameraTable : TableBase<string, CinemachineVirtualCamera, CinemachineVirtualCameraPair>{}

    [System.Serializable]
    public class CinemachineVirtualCameraPair : KeyAndValue<string, CinemachineVirtualCamera>{}

    /// Cutin ///
    [System.Serializable]
    public class CutinTable : TableBase<string, BaseCutin, CutinPair>{}

    [System.Serializable]
    public class CutinPair : KeyAndValue<string, BaseCutin>{}

    /// ActiveSwitchUI ///
    [System.Serializable]
    public class ActiveSwitchUITable : TableBase<string, ActiveSwitchUI, ActiveSwitchUIPair>{}

    [System.Serializable]
    public class ActiveSwitchUIPair : KeyAndValue<string, ActiveSwitchUI>{}

    /// CommonPanel ///
    [System.Serializable]
    public class CommonPanelTable : TableBase<string, CommonPanel, CommonPanelPair>{}

    [System.Serializable]
    public class CommonPanelPair : KeyAndValue<string, CommonPanel>{}

    /// CommonInputField ///
    [System.Serializable]
    public class CommonInputFieldTable : TableBase<string, CommonInputField, CommonInputFieldPair>{}

    [System.Serializable]
    public class CommonInputFieldPair : KeyAndValue<string, CommonInputField>{}

    /// TabContent ///
    [System.Serializable]
    public class TabContentTable : TableBase<CommonButton, CommonPanel, TabContentPair>{}

    [System.Serializable]
    public class TabContentPair : KeyAndValue<CommonButton, CommonPanel>{}

    /// RoomInterior ///
    [System.Serializable]
    public class RoomInteriorTable : TableBase<int, BaseRoomInterior, RoomInteriorPair>{}

    [System.Serializable]
    public class RoomInteriorPair : KeyAndValue<int, BaseRoomInterior>{}

    /// GameObject ///
    [System.Serializable]
    public class GameObjectTable : TableBase<string, GameObject, GameObjectPair>{}

    [System.Serializable]
    public class GameObjectPair : KeyAndValue<string, GameObject>{}

    /// BaseBlock ///
    [System.Serializable]
    public class BaseBlockTable : TableBase<BlockState, BaseBlock, BaseBlockPair>{}

    [System.Serializable]
    public class BaseBlockPair : KeyAndValue<BlockState, BaseBlock>{}


    /// TableBaseサンプル用 ///
    [System.Serializable]
    public class SampleTable : TableBase<string, Vector3, SamplePair>{}

    [System.Serializable]
    public class SamplePair : KeyAndValue<string, Vector3>{}



    /// OldTableBaseサンプル用 ///
    [System.Serializable]
    public class OldSampleTable : OldTableBase<string, Vector3, OldSamplePair>{}

    [System.Serializable]
    public class OldSamplePair : OldKeyAndValue<string, Vector3>{

        public OldSamplePair (string key, Vector3 value) : base (key, value) {

        }
    }
}
