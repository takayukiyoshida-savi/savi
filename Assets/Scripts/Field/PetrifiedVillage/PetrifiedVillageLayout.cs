using UnityEngine;

namespace PetrifiedVillage
{
    public enum HouseState
    {
        Intact,
        TwoStory,
        Collapsed
    }

    public enum StatuePose
    {
        Standing,
        Reaching,
        Running,
        Kneeling,
        Shielding
    }

    public struct HouseSpec
    {
        public string name;
        public Vector3 position;
        public Vector3 facing;
        public Vector2 footprint;
        public float height;
        public HouseState state;

        public HouseSpec(string name, Vector3 position, Vector3 facing, Vector2 footprint, float height, HouseState state)
        {
            this.name = name;
            this.position = position;
            this.facing = facing;
            this.footprint = footprint;
            this.height = height;
            this.state = state;
        }
    }

    public struct StatueSpec
    {
        public string name;
        public Vector3 position;
        public Vector3 facing;
        public StatuePose pose;
        public float scale;

        public StatueSpec(string name, Vector3 position, Vector3 facing, StatuePose pose, float scale = 1f)
        {
            this.name = name;
            this.position = position;
            this.facing = facing;
            this.pose = pose;
            this.scale = scale;
        }
    }

    public struct PropSpec
    {
        public Vector3 position;
        public Vector3 facing;

        public PropSpec(Vector3 position, Vector3 facing)
        {
            this.position = position;
            this.facing = facing;
        }
    }

    public struct InspectSpec
    {
        public string title;
        public string body;
        public Vector3 position;

        public InspectSpec(string title, string body, Vector3 position)
        {
            this.title = title;
            this.body = body;
            this.position = position;
        }
    }

    /// <summary>
    /// 村の配置データ。座標は村ルート基準のメートル（+X=東, +Z=北）。
    /// 配置を調整したいときはここの数値だけ触れば済むようにしてある。
    /// </summary>
    public static class PetrifiedVillageLayout
    {
        // 入口(南) → 中央広場 → 高台(北) の3エリア構成
        public const float GroundSize = 120f;
        public const float PlazaDiameter = 20f;
        public const float BasinRadius = 3.3f;

        public const float TerraceHeight = 3f;
        public const float TerraceSouthEdge = 14f;
        public const float TerraceNorthEdge = 30f;
        public const float TerraceWestEdge = -27f;
        public const float TerraceEastEdge = 22f;

        public const float StairWidth = 5f;
        public const int StairSteps = 6;

        // 崩れた家の裏から高台へ抜ける寄り道ルート
        public const float SideRampX = -24.5f;
        public const float SideRampWidth = 3f;
        public const float SideRampRun = 8f;

        public const float CliffRadius = 38f;
        public const float GateZ = -26f;

        private static readonly Vector3 East = Vector3.right;
        private static readonly Vector3 West = Vector3.left;
        private static readonly Vector3 North = Vector3.forward;
        private static readonly Vector3 South = Vector3.back;

        public static readonly HouseSpec[] Houses =
        {
            new HouseSpec("Inn", new Vector3(-14f, 0f, -12f), East, new Vector2(6f, 8f), 4f, HouseState.Intact),
            new HouseSpec("HouseWest", new Vector3(-17f, 0f, 2f), East, new Vector2(5f, 6f), 3.2f, HouseState.Intact),
            new HouseSpec("HouseCollapsed", new Vector3(-18f, 0f, 10f), East, new Vector2(6f, 6f), 3.2f, HouseState.Collapsed),
            new HouseSpec("ItemShop", new Vector3(14f, 0f, -12f), West, new Vector2(6f, 7f), 3.6f, HouseState.Intact),
            new HouseSpec("HouseEast", new Vector3(17f, 0f, -3f), West, new Vector2(5f, 5f), 3f, HouseState.Intact),
            new HouseSpec("HouseTwoStory", new Vector3(18f, 0f, 7f), West, new Vector2(6f, 6f), 3.4f, HouseState.TwoStory),
            new HouseSpec("GateHut", new Vector3(8f, 0f, -22f), West, new Vector2(4f, 4f), 2.8f, HouseState.Intact),
            new HouseSpec("ChiefHouse", new Vector3(-10f, TerraceHeight, 24f), South, new Vector2(8f, 6f), 4f, HouseState.TwoStory),
        };

        public static readonly StatueSpec[] Statues =
        {
            new StatueSpec("Statue_WaterSeeker", new Vector3(1.5f, 0f, -4.4f), North, StatuePose.Reaching),
            new StatueSpec("Statue_PrayingElder", new Vector3(-3.6f, 0f, -3.2f), new Vector3(1f, 0f, 1f), StatuePose.Kneeling),
            new StatueSpec("Statue_RunningChild", new Vector3(0.8f, 0f, -17f), South, StatuePose.Running, 0.7f),
            new StatueSpec("Statue_Mother", new Vector3(-0.5f, 0f, -13.5f), South, StatuePose.Reaching),
            new StatueSpec("Statue_Merchant", new Vector3(11.4f, 0f, -5f), West, StatuePose.Standing),
            new StatueSpec("Statue_SkyGazer", new Vector3(-9f, 0f, 3f), new Vector3(1f, 0f, -0.4f), StatuePose.Shielding),
            new StatueSpec("Statue_Innkeeper", new Vector3(-9.6f, 0f, -12f), East, StatuePose.Standing),
            new StatueSpec("Statue_CoupleA", new Vector3(-2f, TerraceHeight, 18f), East, StatuePose.Reaching),
            new StatueSpec("Statue_CoupleB", new Vector3(-1.3f, TerraceHeight, 18f), West, StatuePose.Reaching, 0.95f),
            new StatueSpec("Statue_Chief", new Vector3(-10f, TerraceHeight, 19.5f), South, StatuePose.Standing, 1.05f),
        };

        public static readonly PropSpec[] MarketStalls =
        {
            new PropSpec(new Vector3(10f, 0f, -5f), West),
            new PropSpec(new Vector3(11f, 0f, 2f), West),
            new PropSpec(new Vector3(-9f, 0f, -5f), East),
        };

        public static readonly Vector3[] DeadPalms =
        {
            new Vector3(4.6f, 0f, 3.6f),
            new Vector3(-4.8f, 0f, 2.6f),
            new Vector3(-5.5f, 0f, -22f),
            new Vector3(12f, 0f, 10f),
            new Vector3(-12f, 0f, 8.5f),
            new Vector3(3f, TerraceHeight, 27f),
        };

        public static readonly Vector3[] PotClusters =
        {
            new Vector3(-10.4f, 0f, -8.6f),
            new Vector3(9.8f, 0f, -8.6f),
            new Vector3(13.8f, 0f, -0.4f),
            new Vector3(-13.6f, 0f, 5.2f),
            new Vector3(14.6f, 0f, 10.4f),
            new Vector3(-5.2f, TerraceHeight, 22f),
        };

        public static readonly Vector3[] CrateStacks =
        {
            new Vector3(9.4f, 0f, 4.6f),
            new Vector3(5.2f, 0f, -20.6f),
            new Vector3(-10.6f, 0f, -15.6f),
            new Vector3(15f, TerraceHeight, 17f),
        };

        public static readonly Vector3 ShrinePosition = new Vector3(8f, TerraceHeight, 23f);
        public static readonly Vector3 WatchtowerPosition = new Vector3(18f, TerraceHeight, 26f);
        public const float WatchtowerHeight = 7f;

        // 寄り道の報酬。本筋(門→広場→階段)からは見えない位置に置く
        public static readonly Vector3 HiddenChestPosition = new Vector3(-22.4f, 0f, 6f);

        public static readonly InspectSpec[] InspectPoints =
        {
            new InspectSpec("門の立て札",
                "『隊商の村サルハ。泉の湧くかぎり、旅人を拒まず』\n文字の半分は砂に削られている。",
                new Vector3(3.2f, 0f, -23.4f)),
            new InspectSpec("涸れた泉",
                "底はひび割れ、一滴の水も残っていない。\n石になった女が、空の水瓶を泉へ差し出している。",
                new Vector3(0f, 0f, -3.6f)),
            new InspectSpec("走る子どもの石像",
                "門へ向かって駆け出した姿のまま固まっている。\n振り返ろうとした首が、わずかに傾いている。",
                new Vector3(0.8f, 0f, -16.2f)),
            new InspectSpec("露店の帳簿",
                "『水一杯、干しナツメ三つと交換。今日より値を倍にする』\n最後の頁は書きかけで止まっている。",
                new Vector3(9.2f, 0f, -5f)),
            new InspectSpec("宿帳",
                "『東の隊商、今夜到着予定。部屋を三つ空けておくこと』\n到着の印はどこにもない。",
                new Vector3(-9.4f, 0f, -10.8f)),
            new InspectSpec("崩れた家の日記",
                "『三日前から泉の水が引いている。\n今朝、東の空に白い砂の壁が立った。音がしない。鳥もいない』",
                new Vector3(-17.4f, 0f, 9.6f)),
            new InspectSpec("砂に埋もれた宝箱",
                "家の裏手に隠すように置かれている。\n誰かが、戻ってくるつもりで埋めたのだろう。",
                HiddenChestPosition + new Vector3(0f, 0f, -0.9f)),
            new InspectSpec("寄り添う二人の石像",
                "逃げるのをやめ、互いの肩に手を置いている。\n二人とも、村ではなく相手の顔を見ている。",
                new Vector3(-1.65f, TerraceHeight, 17.2f)),
            new InspectSpec("祠の石柱",
                "『泉は借りもの。返す日が来れば、時もまた止まる』\n古い文字で、そう刻まれている。",
                ShrinePosition + new Vector3(0f, 0f, -1.4f)),
            new InspectSpec("見張り台",
                "台の上の見張りは、東の地平を指さしたまま石になっている。\n鐘の紐は、握られたままだ。",
                WatchtowerPosition + new Vector3(-2.4f, 0f, 0f)),
        };
    }
}
