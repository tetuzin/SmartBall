using System.Threading.Tasks;
using UnityEngine;

using Pachinko.LightController;

namespace Pachinko.Manager.Light
{
    public enum LightState
    {
        NONE,                   //
        OFF,                    // OFF
        LIGHTING,               // 点灯
        LIGHTING_KILL,          // 点灯OFF
        FLASHINITIALIZE,        // フラッシュ初期化
        FLASH,                  // フラッシュ
        RAINDOW_FLASH,          // 虹フラッシュ
        MINIFLASH,
        RIGHT_ROTATE,           // 時計回り
        LEFT_ROTATE,            // 反時計回り
        RIGHT_ROTATE_ONE,       // 時計回り(一つずつ)
        LEFT_ROTATE_ONE,        // 反時計回り(一つずつ)
        TOP_BOTTOM_FLOW,        // 上から下に
        BOTTOM_TOP_FLOW,        // 下から上に
        TOP_BOTTOM_FLOW_ONE,    // 上から下に(一つずつ)
        BOTTOM_TOP_FLOW_ONE     // 下から上に(一つずつ)
    }

    public enum ColorState
    {
        Color_RED,           // 赤
        Color_ORANGE,         // オレンジ (なんか発色悪いからおすすめしないです。。。)
        Color_YELLOW,        // 黄色
        Color_GREEN,           // 緑
        Color_BLUE,           // 青
        Color_INDIGO,          // 藍色(青)
        Color_PURPLE,         // 紫
        Color_WHITE       // 白
    }

    public class PachinkoLightManager : MonoBehaviour
    {
        // ---------- 定数宣言 ----------
        Color RED = new Color(255, 0, 0);           // 赤
        Color ORANGE = new Color(175,42,0);         // オレンジ (なんか発色悪いからおすすめしないです。。。)
        Color YELLOW = new Color(255,255,0);        // 黄色
        Color GREEN = new Color(0,255,0);           // 緑
        Color BLUE = new Color(0,40,255);           // 青
        Color INDIGO = new Color(0,0,255);          // 藍色(青)
        Color PURPLE = new Color(50,0,255);         // 紫
        Color WHITE = new Color(255,255,255);       // 白

        // ---------- ゲームオブジェクト参照変数宣言 ----------
        [SerializeField, Tooltip("パチンコライト_Top_L")]   protected PachinkoLightController _pachiLight_Top_L;
        [SerializeField, Tooltip("パチンコライト_Top_R")]   protected PachinkoLightController _pachiLight_Top_R;
        [SerializeField, Tooltip("パチンコライト_Left")]    protected PachinkoLightController _pachiLight_LIght_Left;
        [SerializeField, Tooltip("パチンコライト_Right")]   protected PachinkoLightController _pachiLight_LIght_Right;
        [SerializeField, Tooltip("パチンコライト_Bottom_L")]  protected PachinkoLightController _pachiLight_LIght_Bottom_L;
        [SerializeField, Tooltip("パチンコライト_Bottom_R")]  protected PachinkoLightController _pachiLight_LIght_Bottom_R;

        // ---------- プレハブ ----------
        // ---------- プロパティ ----------
        // ---------- クラス変数宣言 ----------

        public int FLASH_SW = default;
        public int RainbowFLASH_SW = default;
        
        // ---------- インスタンス変数宣言 ----------
        // ---------- Unity組込関数 ----------
        
        async void Start()
        {
            //確認用
            // DemoLight();
        }

        // ---------- Public関数 ----------

        // delayTime オススメ
        //      ・フラッシュ 35   
        //      ・流れるやつ 25
        public async Task LightStatus(
            LightState lightState = default,
            int delayTime = default,
            ColorState setColor = default
            )
        {
            Color lightColor = changeColor(setColor);
            // Debug.Log("ステータス:"+lightState);
            // Debug.Log("色:"+lightColor);
            switch (lightState)
            {
                case LightState.NONE:
                    await Initialize();
                    // ALLKill_Lighting();
                    FLASH_SW = 9999;
                    RainbowFLASH_SW = 99999;
                    break;

                case LightState.OFF:
                    ALL_LightOFF();
                    break;

                // Todo 無理やり実装中なので直す
                // フラッシュ
                case LightState.FLASH:
                    // FLASH_SW = 0;
                    await flash(lightColor, delayTime);
                    break;

                case LightState.FLASHINITIALIZE:
                    FLASH_SW = 0;
                    break;

                // 虹フラッシュ
                case LightState.RAINDOW_FLASH:
                    RainbowFLASH_SW = 0;
                    for (;9999 > RainbowFLASH_SW; RainbowFLASH_SW++) await RainbowFlash(delayTime);
                    break;
                
                case LightState.MINIFLASH:
                    LightStatus(LightState.FLASHINITIALIZE);
                    flash(lightColor,35);
                    await Task.Delay(delayTime);
                    await LightStatus();
                    break;

                case LightState.LIGHTING:
                    ALL_LightON(WHITE);
                    break;

                case LightState.LIGHTING_KILL:
                    // ALLKill_Lighting();
                    break;

                // 時計回り
                case LightState.RIGHT_ROTATE:
                    await RightRotate(lightColor,delayTime);
                    break;

                // 反時計回り
                case LightState.LEFT_ROTATE:
                    await LeftRotate(lightColor,delayTime);
                    break;

                // 時計回り(一つずつ)
                case LightState.RIGHT_ROTATE_ONE:
                    await RightRotate_one(lightColor,delayTime);
                    break;

                // 反時計回り(一つずつ)
                case LightState.LEFT_ROTATE_ONE:
                    await LeftRotate_one(lightColor,delayTime);
                    break;

                // 上から下に
                case LightState.TOP_BOTTOM_FLOW:
                    await TopBottomFlow(lightColor,delayTime);
                    break;

                // 下から上に
                case LightState.BOTTOM_TOP_FLOW:
                    await BottomTopFlow(lightColor,delayTime);
                    break;

                // 上から下に(一つずつ)
                case LightState.TOP_BOTTOM_FLOW_ONE:
                    await TopBottomFlow_one(lightColor,delayTime);
                    break;

                // 下から上に(一つずつ)
                case LightState.BOTTOM_TOP_FLOW_ONE:
                    await BottomTopFlow_one(lightColor,delayTime);
                    break;

                default:
                    ALL_LightOFF();
                    break;
            }
            await Task.CompletedTask;
        }


        // ---------- Private関数 ----------

        // ライトON
        async void ALL_LightON()
        {
            _pachiLight_Top_L.ALL_LightON();
            _pachiLight_Top_R.ALL_LightON();
            _pachiLight_LIght_Left.ALL_LightON();
            _pachiLight_LIght_Right.ALL_LightON();
            _pachiLight_LIght_Bottom_L.ALL_LightON();
            _pachiLight_LIght_Bottom_R.ALL_LightON();
        }

        // ライトON(色変更付き)
        async Task ALL_LightON(Color lightColor)
        {
            _pachiLight_Top_L.ALL_LightON(lightColor);
            _pachiLight_Top_R.ALL_LightON(lightColor);
            _pachiLight_LIght_Left.ALL_LightON(lightColor);
            _pachiLight_LIght_Right.ALL_LightON(lightColor);
            _pachiLight_LIght_Bottom_L.ALL_LightON(lightColor);
            _pachiLight_LIght_Bottom_R.ALL_LightON(lightColor);
        }

        // ライトOFF
        void ALL_LightOFF()
        {
            _pachiLight_Top_L.ALL_LightOFF();
            _pachiLight_Top_R.ALL_LightOFF();
            _pachiLight_LIght_Left.ALL_LightOFF();
            _pachiLight_LIght_Right.ALL_LightOFF();
            _pachiLight_LIght_Bottom_L.ALL_LightOFF();
            _pachiLight_LIght_Bottom_R.ALL_LightOFF();
        }

        // 点灯
        // void ALL_Lighting()
        // {
        //     _pachiLight_Top_L.ALL_Lighting(Color.white,false);
        //     _pachiLight_Top_R.ALL_Lighting(Color.white,false);
        //     _pachiLight_LIght_Left.ALL_Lighting(Color.white,false);
        //     _pachiLight_LIght_Right.ALL_Lighting(Color.white,false);
        //     _pachiLight_LIght_Bottom_L.ALL_Lighting(Color.white,false);
        //     _pachiLight_LIght_Bottom_R.ALL_Lighting(Color.white,false);
        // }

        // // 点灯消す
        // async Task ALLKill_Lighting()
        // {
        //     _pachiLight_Top_L.ALL_Lighting(Color.white,true);
        //     _pachiLight_Top_R.ALL_Lighting(Color.white,true);
        //     _pachiLight_LIght_Left.ALL_Lighting(Color.white,true);
        //     _pachiLight_LIght_Right.ALL_Lighting(Color.white,true);
        //     _pachiLight_LIght_Bottom_L.ALL_Lighting(Color.white,true);
        //     _pachiLight_LIght_Bottom_R.ALL_Lighting(Color.white,true);
        //     ALL_LightOFF();
        // }

        // 上から下に(一つずつ)
        async Task TopBottomFlow_one(Color lightColor, int delayTime)
        {
            Initialize();

                  _pachiLight_Top_L.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_Top_R.UpFlow_one(lightColor ,delayTime);

                  _pachiLight_LIght_Right.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Left.DownFlow_one(lightColor ,delayTime);

                  _pachiLight_LIght_Bottom_L.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.DownFlow_one(lightColor ,delayTime);
        }

        // 下から上に(一つずつ)
        async Task BottomTopFlow_one(Color lightColor, int delayTime)
        {
            Initialize();

                  _pachiLight_LIght_Bottom_L.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.UpFlow_one(lightColor ,delayTime);

                  _pachiLight_LIght_Right.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Left.UpFlow_one(lightColor ,delayTime);

                  _pachiLight_Top_L.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_Top_R.DownFlow_one(lightColor ,delayTime);
        }

        // 上から下に
        async Task TopBottomFlow(Color lightColor, int delayTime)
        {
            Initialize();

                  _pachiLight_Top_L.UpFlow(lightColor ,delayTime);
            await _pachiLight_Top_R.UpFlow(lightColor ,delayTime);

                  _pachiLight_LIght_Right.DownFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Left.DownFlow(lightColor ,delayTime);

                  _pachiLight_LIght_Bottom_L.DownFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.DownFlow(lightColor ,delayTime);
        }

        // 下から上に
        async Task BottomTopFlow(Color lightColor, int delayTime)
        {
            Initialize();

                  _pachiLight_LIght_Bottom_L.UpFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.UpFlow(lightColor ,delayTime);

                  _pachiLight_LIght_Right.UpFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Left.UpFlow(lightColor ,delayTime);

                  _pachiLight_Top_L.DownFlow(lightColor ,delayTime);
            await _pachiLight_Top_R.DownFlow(lightColor ,delayTime);
        }

        // 時計回り(一つずつ)
        async Task RightRotate_one(Color lightColor, int delayTime)
        {
            Initialize();
            await _pachiLight_Top_L.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_Top_R.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Right.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_L.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Left.UpFlow_one(lightColor ,delayTime);
        }

        // 反時計回り(一つずつ)
        async Task LeftRotate_one(Color lightColor, int delayTime)
        {
            Initialize();
            await _pachiLight_Top_L.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Left.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_L.DownFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Bottom_R.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_LIght_Right.UpFlow_one(lightColor ,delayTime);
            await _pachiLight_Top_R.DownFlow_one(lightColor ,delayTime);
        }

        // 時計回り
        async Task RightRotate(Color lightColor, int delayTime)
        {
            Initialize();

            _pachiLight_LIght_Bottom_L.UpFlow(lightColor ,delayTime);
            await _pachiLight_Top_R.UpFlow(lightColor ,delayTime);

            _pachiLight_LIght_Left.UpFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Right.DownFlow(lightColor ,delayTime);

            _pachiLight_LIght_Bottom_R.DownFlow(lightColor ,delayTime);
            await _pachiLight_Top_L.DownFlow(lightColor ,delayTime);
        }

        // 反時計回り
        async Task LeftRotate(Color lightColor, int delayTime)
        {
            Initialize();

            _pachiLight_LIght_Bottom_R.UpFlow(lightColor ,delayTime);
            await _pachiLight_Top_L.UpFlow(lightColor ,delayTime);

            _pachiLight_LIght_Left.DownFlow(lightColor ,delayTime);
            await _pachiLight_LIght_Right.UpFlow(lightColor ,delayTime);

            _pachiLight_LIght_Bottom_L.DownFlow(lightColor ,delayTime);
            await _pachiLight_Top_R.DownFlow(lightColor ,delayTime);
        }


        // Todo 無理やり実装してるのであとで直したい  (いつか止まる可能性がある)
        // フラッシュ
        async Task flash(Color lightColor, int delayTime)
        {
            for (;9999 > FLASH_SW; FLASH_SW++)
            {
                _pachiLight_Top_L.flash(lightColor ,delayTime);
                _pachiLight_Top_R.flash(lightColor ,delayTime);
                _pachiLight_LIght_Left.flash(lightColor ,delayTime);
                _pachiLight_LIght_Right.flash(lightColor ,delayTime);
                _pachiLight_LIght_Bottom_L.flash(lightColor ,delayTime);
                await _pachiLight_LIght_Bottom_R.flash(lightColor ,delayTime);
            }
        }

        // 虹フラッシュ
        async Task RainbowFlash(int delayTime)
        {
                LightStatus(LightState.FLASHINITIALIZE);
                LightStatus(LightState.FLASH,35,ColorState.Color_RED);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_ORANGE);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_YELLOW);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_GREEN);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_BLUE);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_INDIGO);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);

                await Task.Delay(50);

                LightStatus(LightState.FLASH,35,ColorState.Color_PURPLE);
                await Task.Delay(delayTime);
                await LightStatus(LightState.OFF);
        }

        // 色変換
        Color changeColor(ColorState setColor)
        {
            switch (setColor)
            {
                case ColorState.Color_RED:
                    return RED;
                    break;

                case ColorState.Color_ORANGE:
                    return ORANGE;
                    break;

                case ColorState.Color_YELLOW:
                    return YELLOW;
                    break;

                case ColorState.Color_GREEN:
                    return GREEN;
                    break;

                case ColorState.Color_BLUE:
                    return BLUE;
                    break;

                case ColorState.Color_INDIGO:
                    return INDIGO;
                    break;

                case ColorState.Color_PURPLE:
                    return PURPLE;
                    break;

                case ColorState.Color_WHITE:
                    return WHITE;
                    break;
                default:
                    return WHITE;
                    break;
            }
        }

        // Todo 使い方の確認用
        // Demo
        async void DemoLight()
        {
            await Task.Delay(2000);
            LightStatus(LightState.FLASHINITIALIZE);
            LightStatus(LightState.FLASH,35,ColorState.Color_RED);
            await Task.Delay(2000);
            await LightStatus(LightState.NONE);

            for (int i = 0; i < 10 ; i++) await LightStatus(LightState.RAINDOW_FLASH,35);

            // await Task.Delay(4000);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.RIGHT_ROTATE,25,ColorState.Color_ORANGE);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.LEFT_ROTATE,25,ColorState.Color_YELLOW);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.RIGHT_ROTATE_ONE,25,ColorState.Color_GREEN);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.LEFT_ROTATE_ONE,25,ColorState.Color_BLUE);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.TOP_BOTTOM_FLOW,25,ColorState.Color_INDIGO);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.BOTTOM_TOP_FLOW,25,ColorState.Color_PURPLE);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.TOP_BOTTOM_FLOW_ONE,25,ColorState.Color_RED);
            await LightStatus(LightState.NONE);

            await LightStatus(LightState.BOTTOM_TOP_FLOW_ONE,25,ColorState.Color_ORANGE);
            await LightStatus(LightState.NONE);
        }

        // 初期化
        async Task Initialize()
        {
            _pachiLight_Top_L.Initialize();
            _pachiLight_Top_R.Initialize();
            _pachiLight_LIght_Left.Initialize();
            _pachiLight_LIght_Right.Initialize();
            _pachiLight_LIght_Bottom_L.Initialize();
            _pachiLight_LIght_Bottom_R.Initialize();
            await Task.CompletedTask;
        }
    }
}

