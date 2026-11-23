using System.Collections.Generic;
using UnityEngine;

namespace PetrifiedVillage
{
    /// <summary>
    /// プリミティブだけで「砂漠の石化した村」を組み立てる。
    /// 外部アセットに依存しないので、どのプロジェクトに持っていってもそのまま生成できる。
    /// </summary>
    public static class PetrifiedVillageBuilder
    {
        public const string RootName = "PetrifiedVillage";

        // 何度生成しても同じ見た目になるよう、ばらつきは固定シードで作る
        private const int ScatterSeed = 7021;
        private const float InspectRadius = 1.6f;

        private static readonly Color Sand = new Color(0.86f, 0.74f, 0.52f);
        private static readonly Color SandDark = new Color(0.74f, 0.62f, 0.42f);
        private static readonly Color DryMud = new Color(0.55f, 0.45f, 0.33f);
        private static readonly Color Paving = new Color(0.78f, 0.69f, 0.53f);
        private static readonly Color Rock = new Color(0.66f, 0.52f, 0.38f);
        private static readonly Color Wood = new Color(0.42f, 0.30f, 0.20f);
        private static readonly Color Shadow = new Color(0.13f, 0.10f, 0.08f);
        private static readonly Color Stone = new Color(0.60f, 0.60f, 0.58f);
        private static readonly Color StoneDark = new Color(0.47f, 0.47f, 0.46f);
        private static readonly Color Terracotta = new Color(0.72f, 0.42f, 0.28f);
        private static readonly Color Gold = new Color(0.85f, 0.68f, 0.25f);

        private static readonly Color[] AdobeTones =
        {
            new Color(0.88f, 0.78f, 0.60f),
            new Color(0.82f, 0.69f, 0.50f),
            new Color(0.91f, 0.83f, 0.68f),
        };

        private static readonly Color[] ClothTones =
        {
            new Color(0.66f, 0.30f, 0.24f),
            new Color(0.28f, 0.50f, 0.52f),
            new Color(0.78f, 0.60f, 0.26f),
        };

        private static readonly Dictionary<Color, Material> materialCache = new Dictionary<Color, Material>();
        private static Shader litShader;
        private static System.Random rng;

        public static GameObject Build(bool withEnvironment)
        {
            materialCache.Clear();
            litShader = FindLitShader();
            rng = new System.Random(ScatterSeed);

            GameObject root = new GameObject(RootName);
            Transform terrain = Group("Terrain", root.transform);
            Transform buildings = Group("Buildings", root.transform);
            Transform statues = Group("Statues", root.transform);
            Transform props = Group("Props", root.transform);
            Transform inspectPoints = Group("InspectPoints", root.transform);

            BuildGround(terrain);
            BuildTerrace(terrain);
            BuildCliffs(terrain);
            BuildDunes(terrain);

            BuildGate(buildings);
            for (int i = 0; i < PetrifiedVillageLayout.Houses.Length; i++)
            {
                BuildHouse(buildings, PetrifiedVillageLayout.Houses[i], AdobeTones[i % AdobeTones.Length]);
            }
            BuildShrine(buildings);
            BuildWatchtower(buildings, statues);

            foreach (StatueSpec statue in PetrifiedVillageLayout.Statues)
            {
                BuildStatue(statues, statue);
            }

            BuildDrySpring(props);
            BuildProps(props);

            foreach (InspectSpec inspect in PetrifiedVillageLayout.InspectPoints)
            {
                BuildInspectPoint(inspectPoints, inspect);
            }

            SetStaticRecursively(terrain);
            SetStaticRecursively(buildings);
            SetStaticRecursively(statues);
            SetStaticRecursively(props);

            if (withEnvironment)
            {
                BuildEnvironment(root.transform);
            }

            return root;
        }

        #region Terrain

        private static void BuildGround(Transform parent)
        {
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.SetParent(parent, false);
            // Planeは1スケール=10m
            float scale = PetrifiedVillageLayout.GroundSize / 10f;
            ground.transform.localScale = new Vector3(scale, 1f, scale);
            Paint(ground, Sand);

            Cyl("Plaza", parent, new Vector3(0f, 0.02f, 0f), PetrifiedVillageLayout.PlazaDiameter, 0.04f, Paving);

            // 門から広場へ続く石畳。ところどころ砂に埋もれて欠けている
            for (float z = PetrifiedVillageLayout.GateZ + 1.5f; z < -PetrifiedVillageLayout.PlazaDiameter * 0.5f; z += 1.4f)
            {
                for (int column = -1; column <= 1; column++)
                {
                    if (rng.NextDouble() < 0.2)
                    {
                        continue;
                    }

                    Vector3 position = new Vector3(column * 1.35f + Rand(-0.12f, 0.12f), 0.03f, z + Rand(-0.1f, 0.1f));
                    Vector3 size = new Vector3(Rand(1.05f, 1.25f), 0.06f, Rand(1.05f, 1.25f));
                    GameObject stone = Box("RoadStone", parent, position, size, Paving, false);
                    stone.transform.localRotation = Quaternion.Euler(0f, Rand(-6f, 6f), 0f);
                }
            }
        }

        private static void BuildTerrace(Transform parent)
        {
            float height = PetrifiedVillageLayout.TerraceHeight;
            float south = PetrifiedVillageLayout.TerraceSouthEdge;
            float north = PetrifiedVillageLayout.TerraceNorthEdge;
            float west = PetrifiedVillageLayout.TerraceWestEdge;
            float east = PetrifiedVillageLayout.TerraceEastEdge;

            Vector3 center = new Vector3((west + east) * 0.5f, height * 0.5f, (south + north) * 0.5f);
            Box("Terrace", parent, center, new Vector3(east - west, height, north - south), SandDark);

            // 正面階段
            float stepRise = height / PetrifiedVillageLayout.StairSteps;
            float stepRun = 0.5f;
            float stairStart = south - stepRun * PetrifiedVillageLayout.StairSteps;
            for (int i = 0; i < PetrifiedVillageLayout.StairSteps; i++)
            {
                float stepHeight = stepRise * (i + 1);
                float front = stairStart + stepRun * i;
                Vector3 stepCenter = new Vector3(0f, stepHeight * 0.5f, (front + south) * 0.5f);
                Box("Stair_" + i, parent, stepCenter, new Vector3(PetrifiedVillageLayout.StairWidth, stepHeight, south - front), Paving);
            }

            // 高台の縁の手すり壁。階段と寄り道スロープの到達点だけ開けておく
            float halfStair = PetrifiedVillageLayout.StairWidth * 0.5f + 0.5f;
            float rampEast = PetrifiedVillageLayout.SideRampX + PetrifiedVillageLayout.SideRampWidth * 0.5f + 0.5f;
            BuildParapetRun(parent, rampEast, -halfStair, south + 0.25f, height);
            BuildParapetRun(parent, halfStair, east, south + 0.25f, height);

            // 寄り道スロープ
            float run = PetrifiedVillageLayout.SideRampRun;
            float angle = Mathf.Atan2(height, run) * Mathf.Rad2Deg;
            float length = Mathf.Sqrt(height * height + run * run);
            Vector3 rampCenter = new Vector3(PetrifiedVillageLayout.SideRampX, height * 0.5f - 0.2f, south - run * 0.5f);
            GameObject ramp = Box("SideRamp", parent, rampCenter, new Vector3(PetrifiedVillageLayout.SideRampWidth, 0.4f, length), SandDark);
            ramp.transform.localRotation = Quaternion.Euler(-angle, 0f, 0f);
        }

        private static void BuildParapetRun(Transform parent, float fromX, float toX, float z, float baseY)
        {
            float length = toX - fromX;
            Box("TerraceParapet", parent, new Vector3(fromX + length * 0.5f, baseY + 0.3f, z), new Vector3(length, 0.6f, 0.5f), AdobeTones[1]);
        }

        private static void BuildCliffs(Transform parent)
        {
            Transform cliffs = Group("Cliffs", parent);
            const float step = 9f;
            const float gateGap = 14f;

            for (float degrees = 0f; degrees < 360f; degrees += step)
            {
                // 南(-Z)は村の入口なので岩を置かない
                if (Mathf.Abs(Mathf.DeltaAngle(degrees, 270f)) < gateGap)
                {
                    continue;
                }

                float radians = degrees * Mathf.Deg2Rad;
                float radius = PetrifiedVillageLayout.CliffRadius + Rand(0f, 5f);
                float height = Rand(6f, 14f);
                Vector3 position = new Vector3(Mathf.Cos(radians) * radius, height * 0.5f - 1f, Mathf.Sin(radians) * radius);
                Vector3 size = new Vector3(Rand(8f, 12f), height, Rand(8f, 12f));

                GameObject rock = Box("Cliff", cliffs, position, size, Rock);
                rock.transform.localRotation = Quaternion.Euler(Rand(-6f, 6f), Rand(0f, 90f), Rand(-6f, 6f));
            }
        }

        private static void BuildDunes(Transform parent)
        {
            Transform dunes = Group("Dunes", parent);
            for (int i = 0; i < 26; i++)
            {
                float radians = Rand(0f, Mathf.PI * 2f);
                float radius = Rand(30f, 36f);
                Vector3 position = new Vector3(Mathf.Cos(radians) * radius, 0f, Mathf.Sin(radians) * radius);

                // 入口の道・高台の上・寄り道ルートには砂丘を作らない
                bool onRoad = Mathf.Abs(position.x) < 6f && position.z < 0f;
                bool onTerrace = position.z > PetrifiedVillageLayout.TerraceSouthEdge - 2f
                    && position.x > PetrifiedVillageLayout.TerraceWestEdge - 2f
                    && position.x < PetrifiedVillageLayout.TerraceEastEdge + 2f;
                bool onSideRoute = position.x < -18f && position.z > 0f;
                if (onRoad || onTerrace || onSideRoute)
                {
                    continue;
                }

                Vector3 size = new Vector3(Rand(7f, 12f), Rand(1.2f, 3f), Rand(6f, 10f));
                GameObject dune = Ball("Dune", dunes, position, size, Sand);
                dune.transform.localRotation = Quaternion.Euler(0f, Rand(0f, 180f), 0f);
            }
        }

        #endregion

        #region Buildings

        private static void BuildGate(Transform parent)
        {
            Transform gate = Group("Gate", parent, new Vector3(0f, 0f, PetrifiedVillageLayout.GateZ));

            Box("PillarL", gate, new Vector3(-3.2f, 2.5f, 0f), new Vector3(1.2f, 5f, 1.2f), AdobeTones[1]);
            Box("PillarR", gate, new Vector3(3.2f, 2.5f, 0f), new Vector3(1.2f, 5f, 1.2f), AdobeTones[1]);
            Box("Lintel", gate, new Vector3(0f, 5.3f, 0f), new Vector3(8.4f, 0.7f, 1f), Wood);

            // 西側の塀は砂嵐で半分崩れている
            Box("WallEast", gate, new Vector3(9.3f, 1.5f, 0f), new Vector3(11f, 3f, 0.8f), AdobeTones[0]);
            Box("WallWest", gate, new Vector3(-7.3f, 1.5f, 0f), new Vector3(7f, 3f, 0.8f), AdobeTones[0]);
            Box("WallWestBroken", gate, new Vector3(-13f, 0.6f, 0f), new Vector3(4.4f, 1.2f, 0.8f), AdobeTones[0]);
            ScatterRubble(gate, new Vector3(-13.5f, 0f, 1.2f), 2.2f, 7, AdobeTones[0]);

            GameObject banner = Box("Banner", gate, new Vector3(1.6f, 4.1f, -0.56f), new Vector3(1.1f, 1.7f, 0.04f), ClothTones[0], false);
            banner.transform.localRotation = Quaternion.Euler(0f, 0f, 4f);

            // 立て札
            Cyl("SignPost", gate, new Vector3(3.2f, 0.7f, 2.2f), 0.12f, 1.4f, Wood);
            Box("SignBoard", gate, new Vector3(3.2f, 1.35f, 2.2f), new Vector3(1.3f, 0.6f, 0.08f), Wood, false);
        }

        private static void BuildHouse(Transform parent, HouseSpec spec, Color wallColor)
        {
            Transform house = Group(spec.name, parent, spec.position, Face(spec.facing));
            float width = spec.footprint.x;
            float depth = spec.footprint.y;

            if (spec.state == HouseState.Collapsed)
            {
                BuildCollapsedHouse(house, width, depth, spec.height, wallColor);
                return;
            }

            Box("Body", house, new Vector3(0f, spec.height * 0.5f, 0f), new Vector3(width, spec.height, depth), wallColor);
            BuildParapet(house, new Vector3(0f, spec.height, 0f), width, depth, wallColor);
            BuildFacade(house, width, depth, spec.height);

            if (spec.state == HouseState.TwoStory)
            {
                const float upperHeight = 2.6f;
                float upperWidth = width * 0.6f;
                float upperDepth = depth * 0.6f;
                Vector3 offset = new Vector3(width * 0.2f, spec.height, -depth * 0.2f);

                Box("UpperBody", house, offset + Vector3.up * (upperHeight * 0.5f), new Vector3(upperWidth, upperHeight, upperDepth), wallColor);
                BuildParapet(house, offset + Vector3.up * upperHeight, upperWidth, upperDepth, wallColor);
                Box("UpperWindow", house, offset + new Vector3(0f, upperHeight * 0.55f, upperDepth * 0.5f + 0.03f), new Vector3(0.7f, 0.8f, 0.1f), Shadow, false);
            }

            // 風下側の壁に吹き寄せられた砂
            Vector3 driftPosition = new Vector3(-width * 0.5f - 0.4f, 0f, Rand(-depth * 0.3f, depth * 0.3f));
            Ball("SandDrift", house, driftPosition, new Vector3(2.4f, Rand(1f, 1.8f), depth * 0.7f), Sand);
        }

        private static void BuildFacade(Transform house, float width, float depth, float height)
        {
            float front = depth * 0.5f;

            Box("Door", house, new Vector3(0f, 0.95f, front + 0.03f), new Vector3(1.1f, 1.9f, 0.1f), Shadow, false);
            Box("DoorLintel", house, new Vector3(0f, 2f, front + 0.08f), new Vector3(1.5f, 0.2f, 0.2f), Wood, false);
            Box("DoorStep", house, new Vector3(0f, 0.08f, front + 0.35f), new Vector3(1.6f, 0.16f, 0.7f), Paving, false);

            float windowX = width * 0.32f;
            float windowY = Mathf.Min(height * 0.55f, 1.8f);
            Box("WindowL", house, new Vector3(-windowX, windowY, front + 0.03f), new Vector3(0.7f, 0.7f, 0.1f), Shadow, false);
            Box("WindowR", house, new Vector3(windowX, windowY, front + 0.03f), new Vector3(0.7f, 0.7f, 0.1f), Shadow, false);

            // 壁から突き出た梁
            int beamCount = Mathf.Max(3, Mathf.RoundToInt(width / 1.5f));
            for (int i = 0; i < beamCount; i++)
            {
                float x = Mathf.Lerp(-width * 0.4f, width * 0.4f, i / (float)(beamCount - 1));
                GameObject beam = Cyl("Beam", house, new Vector3(x, height - 0.5f, front + 0.2f), 0.18f, 0.8f, Wood);
                beam.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            }
        }

        private static void BuildParapet(Transform parent, Vector3 roofCenter, float width, float depth, Color color)
        {
            const float parapetHeight = 0.4f;
            const float thickness = 0.25f;
            float y = roofCenter.y + parapetHeight * 0.5f;
            float halfWidth = width * 0.5f - thickness * 0.5f;
            float halfDepth = depth * 0.5f - thickness * 0.5f;

            Box("ParapetFront", parent, new Vector3(roofCenter.x, y, roofCenter.z + halfDepth), new Vector3(width, parapetHeight, thickness), color, false);
            Box("ParapetBack", parent, new Vector3(roofCenter.x, y, roofCenter.z - halfDepth), new Vector3(width, parapetHeight, thickness), color, false);
            Box("ParapetLeft", parent, new Vector3(roofCenter.x - halfWidth, y, roofCenter.z), new Vector3(thickness, parapetHeight, depth), color, false);
            Box("ParapetRight", parent, new Vector3(roofCenter.x + halfWidth, y, roofCenter.z), new Vector3(thickness, parapetHeight, depth), color, false);
        }

        private static void BuildCollapsedHouse(Transform house, float width, float depth, float height, Color wallColor)
        {
            const float thickness = 0.4f;
            float halfWidth = width * 0.5f;
            float halfDepth = depth * 0.5f;

            // 屋根が落ち、壁だけが高さ違いで残っている。正面は開いていて中に入れる
            Box("WallBack", house, new Vector3(0f, height * 0.5f, -halfDepth), new Vector3(width, height, thickness), wallColor);
            Box("WallLeft", house, new Vector3(-halfWidth, height * 0.35f, 0f), new Vector3(thickness, height * 0.7f, depth), wallColor);
            Box("WallRight", house, new Vector3(halfWidth, height * 0.18f, 0f), new Vector3(thickness, height * 0.36f, depth), wallColor);
            Box("WallFrontStubL", house, new Vector3(-halfWidth * 0.65f, height * 0.25f, halfDepth), new Vector3(width * 0.3f, height * 0.5f, thickness), wallColor);
            Box("WallFrontStubR", house, new Vector3(halfWidth * 0.75f, height * 0.12f, halfDepth), new Vector3(width * 0.2f, height * 0.24f, thickness), wallColor);

            GameObject beam = Cyl("FallenBeam", house, new Vector3(0.4f, 0.7f, -0.6f), 0.2f, width * 0.9f, Wood);
            beam.transform.localRotation = Quaternion.Euler(0f, 25f, 72f);

            ScatterRubble(house, new Vector3(0.6f, 0f, 0.4f), halfWidth * 0.8f, 12, wallColor);
            ScatterRubble(house, new Vector3(halfWidth + 1f, 0f, 1f), 1.4f, 6, wallColor);

            // 日記が置かれた机
            Box("Desk", house, new Vector3(-1.2f, 0.4f, -1.6f), new Vector3(1.2f, 0.8f, 0.7f), Wood);
            Box("Diary", house, new Vector3(-1.2f, 0.83f, -1.6f), new Vector3(0.35f, 0.06f, 0.26f), ClothTones[2], false);
        }

        private static void BuildShrine(Transform parent)
        {
            Transform shrine = Group("Shrine", parent, PetrifiedVillageLayout.ShrinePosition);
            Box("Platform", shrine, new Vector3(0f, 0.2f, 0f), new Vector3(6f, 0.4f, 6f), Paving);

            // 4本のうち2本は折れている
            float[] pillarHeights = { 3.2f, 3.2f, 1.4f, 2.1f };
            int index = 0;
            for (int x = -1; x <= 1; x += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    float pillarHeight = pillarHeights[index++];
                    Cyl("Pillar", shrine, new Vector3(x * 2.4f, 0.4f + pillarHeight * 0.5f, z * 2.4f), 0.55f, pillarHeight, AdobeTones[2], true);
                }
            }

            Box("ObeliskBase", shrine, new Vector3(0f, 0.6f, 0f), new Vector3(1.5f, 0.4f, 1.5f), StoneDark);
            Box("Obelisk", shrine, new Vector3(0f, 2.5f, 0f), new Vector3(0.9f, 3.4f, 0.9f), StoneDark);
            GameObject tip = Box("ObeliskTip", shrine, new Vector3(0f, 4.2f, 0f), new Vector3(0.64f, 0.64f, 0.64f), Gold, false);
            tip.transform.localRotation = Quaternion.Euler(45f, 0f, 45f);

            GameObject fallen = Cyl("FallenPillar", shrine, new Vector3(-3.6f, 0.28f, -3.4f), 0.55f, 1.8f, AdobeTones[2]);
            fallen.transform.localRotation = Quaternion.Euler(0f, 30f, 90f);
        }

        private static void BuildWatchtower(Transform parent, Transform statues)
        {
            float height = PetrifiedVillageLayout.WatchtowerHeight;
            Transform tower = Group("Watchtower", parent, PetrifiedVillageLayout.WatchtowerPosition);

            Box("Body", tower, new Vector3(0f, height * 0.5f, 0f), new Vector3(3f, height, 3f), AdobeTones[1]);
            Box("Deck", tower, new Vector3(0f, height + 0.15f, 0f), new Vector3(3.8f, 0.3f, 3.8f), Wood);
            BuildParapet(tower, new Vector3(0f, height + 0.3f, 0f), 3.8f, 3.8f, AdobeTones[1]);
            Box("Door", tower, new Vector3(0f, 0.95f, -1.53f), new Vector3(1f, 1.9f, 0.1f), Shadow, false);

            // 鐘
            Cyl("BellPost", tower, new Vector3(1.2f, height + 1.3f, 1.2f), 0.14f, 2f, Wood);
            Ball("Bell", tower, new Vector3(0.9f, height + 1.9f, 0.9f), new Vector3(0.5f, 0.6f, 0.5f), Gold);

            Vector3 lookoutPosition = PetrifiedVillageLayout.WatchtowerPosition + new Vector3(-0.3f, height + 0.3f, -0.4f);
            BuildStatue(statues, new StatueSpec("Statue_Lookout", lookoutPosition, new Vector3(1f, 0f, -0.3f), StatuePose.Reaching));
        }

        #endregion

        #region Statues

        // 石化した村人。体のパーツは全部同じ石の色で、ポーズだけで何をしていたかを語らせる
        private static void BuildStatue(Transform parent, StatueSpec spec)
        {
            Transform figure = Group(spec.name, parent, spec.position, Face(spec.facing));
            figure.localScale = Vector3.one * spec.scale;

            float bodyLean = 0f;
            float bodyDrop = 0f;
            float armLeft = 0f;
            float armRight = 0f;
            float legLeft = 0f;
            float legRight = 0f;

            // 腕・脚の角度は「前へ上げる」がマイナス
            switch (spec.pose)
            {
                case StatuePose.Reaching:
                    armLeft = -55f;
                    armRight = -105f;
                    bodyLean = 8f;
                    break;
                case StatuePose.Running:
                    armLeft = -50f;
                    armRight = 45f;
                    legLeft = 35f;
                    legRight = -40f;
                    bodyLean = 16f;
                    break;
                case StatuePose.Kneeling:
                    armLeft = -70f;
                    armRight = -70f;
                    legLeft = 90f;
                    legRight = 90f;
                    bodyDrop = 0.68f;
                    bodyLean = 12f;
                    break;
                case StatuePose.Shielding:
                    armLeft = -150f;
                    armRight = -140f;
                    bodyLean = -10f;
                    break;
            }

            Transform body = Group("Body", figure, new Vector3(0f, 0.8f - bodyDrop, 0f));

            BuildLimb(body, "LegL", new Vector3(-0.12f, 0f, 0f), legLeft, 0.8f, 0.2f);
            BuildLimb(body, "LegR", new Vector3(0.12f, 0f, 0f), legRight, 0.8f, 0.2f);

            Transform upper = Group("Upper", body, Vector3.zero, Quaternion.Euler(bodyLean, 0f, 0f));
            GameObject torso = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            torso.name = "Torso";
            torso.transform.SetParent(upper, false);
            torso.transform.localPosition = new Vector3(0f, 0.38f, 0f);
            torso.transform.localScale = new Vector3(0.44f, 0.42f, 0.3f);
            Paint(torso, Stone);

            Ball("Head", upper, new Vector3(0f, 0.95f, 0f), new Vector3(0.3f, 0.32f, 0.3f), Stone);
            BuildLimb(upper, "ArmL", new Vector3(-0.28f, 0.66f, 0f), armLeft, 0.62f, 0.13f);
            BuildLimb(upper, "ArmR", new Vector3(0.28f, 0.66f, 0f), armRight, 0.62f, 0.13f);

            // 足元は砂に埋もれかけている
            Ball("SandAtFeet", figure, new Vector3(0f, 0f, 0f), new Vector3(1.1f, 0.3f, 1.1f), Sand);
        }

        private static void BuildLimb(Transform parent, string name, Vector3 jointPosition, float swing, float length, float thickness)
        {
            Transform joint = Group(name, parent, jointPosition, Quaternion.Euler(swing, 0f, 0f));
            Cyl("Mesh", joint, new Vector3(0f, -length * 0.5f, 0f), thickness, length, Stone);
        }

        #endregion

        #region Props

        private static void BuildDrySpring(Transform parent)
        {
            Transform spring = Group("DrySpring", parent);
            float radius = PetrifiedVillageLayout.BasinRadius;

            Cyl("Bed", spring, new Vector3(0f, 0.05f, 0f), radius * 2f, 0.06f, DryMud);

            const int segments = 16;
            for (int i = 0; i < segments; i++)
            {
                float degrees = i * 360f / segments;
                float radians = degrees * Mathf.Deg2Rad;
                Vector3 position = new Vector3(Mathf.Cos(radians) * radius, 0.3f, Mathf.Sin(radians) * radius);
                GameObject rim = Box("Rim", spring, position, new Vector3(1.4f, Rand(0.5f, 0.7f), 0.5f), StoneDark);
                rim.transform.localRotation = Quaternion.Euler(0f, 90f - degrees, 0f);
            }

            // ひび割れた泉の底
            for (int i = 0; i < 9; i++)
            {
                float radians = Rand(0f, Mathf.PI * 2f);
                float distance = Rand(0.8f, radius - 0.8f);
                Vector3 position = new Vector3(Mathf.Cos(radians) * distance, 0.085f, Mathf.Sin(radians) * distance);
                GameObject crack = Box("Crack", spring, position, new Vector3(Rand(0.8f, 1.8f), 0.02f, 0.07f), Shadow, false);
                crack.transform.localRotation = Quaternion.Euler(0f, Rand(0f, 180f), 0f);
            }

            Cyl("Pedestal", spring, new Vector3(0f, 0.7f, 0f), 0.8f, 1.4f, StoneDark, true);
            Ball("Bowl", spring, new Vector3(0f, 1.45f, 0f), new Vector3(1.6f, 0.4f, 1.6f), StoneDark);

            // 石像が差し出していた水瓶
            GameObject jar = BuildPot(spring, new Vector3(1.5f, 0f, -3.9f), 0.8f);
            jar.transform.localRotation = Quaternion.Euler(0f, 0f, 80f);
            jar.transform.localPosition += Vector3.up * 0.2f;
        }

        private static void BuildProps(Transform parent)
        {
            for (int i = 0; i < PetrifiedVillageLayout.MarketStalls.Length; i++)
            {
                BuildStall(parent, PetrifiedVillageLayout.MarketStalls[i], ClothTones[i % ClothTones.Length]);
            }

            foreach (Vector3 position in PetrifiedVillageLayout.DeadPalms)
            {
                BuildDeadPalm(parent, position);
            }

            foreach (Vector3 position in PetrifiedVillageLayout.PotClusters)
            {
                Transform cluster = Group("Pots", parent, position);
                int count = rng.Next(2, 5);
                for (int i = 0; i < count; i++)
                {
                    BuildPot(cluster, new Vector3(Rand(-0.7f, 0.7f), 0f, Rand(-0.7f, 0.7f)), Rand(0.7f, 1.2f));
                }
            }

            foreach (Vector3 position in PetrifiedVillageLayout.CrateStacks)
            {
                Transform stack = Group("Crates", parent, position, Quaternion.Euler(0f, Rand(0f, 90f), 0f));
                Box("Crate", stack, new Vector3(0f, 0.35f, 0f), Vector3.one * 0.7f, Wood);
                Box("Crate", stack, new Vector3(0.8f, 0.35f, 0.1f), Vector3.one * 0.7f, Wood);
                GameObject top = Box("Crate", stack, new Vector3(0.35f, 1.02f, 0.05f), Vector3.one * 0.64f, Wood);
                top.transform.localRotation = Quaternion.Euler(0f, 18f, 0f);
            }

            BuildChest(parent, PetrifiedVillageLayout.HiddenChestPosition);
            BuildCart(parent, new Vector3(5.5f, 0f, -11f), new Vector3(-0.4f, 0f, 1f));
        }

        private static void BuildStall(Transform parent, PropSpec spec, Color clothColor)
        {
            Transform stall = Group("MarketStall", parent, spec.position, Face(spec.facing));

            for (int x = -1; x <= 1; x += 2)
            {
                for (int z = -1; z <= 1; z += 2)
                {
                    Cyl("Pole", stall, new Vector3(x * 1.5f, 1.1f, z * 1f), 0.1f, 2.2f, Wood);
                }
            }

            GameObject cloth = Box("Awning", stall, new Vector3(0f, 2.25f, 0f), new Vector3(3.3f, 0.06f, 2.4f), clothColor, false);
            cloth.transform.localRotation = Quaternion.Euler(8f, 0f, 0f);

            Box("Counter", stall, new Vector3(0f, 0.45f, 0.8f), new Vector3(2.6f, 0.9f, 0.7f), Wood);
            BuildPot(stall, new Vector3(-0.8f, 0.9f, 0.8f), 0.6f);
            BuildPot(stall, new Vector3(-0.2f, 0.9f, 0.85f), 0.5f);
            Box("Ledger", stall, new Vector3(0.7f, 0.93f, 0.8f), new Vector3(0.36f, 0.06f, 0.28f), ClothTones[2], false);
        }

        private static GameObject BuildPot(Transform parent, Vector3 position, float scale)
        {
            Transform pot = Group("Pot", parent, position);
            pot.localScale = Vector3.one * scale;
            Ball("Belly", pot, new Vector3(0f, 0.3f, 0f), new Vector3(0.5f, 0.6f, 0.5f), Terracotta);
            Cyl("Neck", pot, new Vector3(0f, 0.62f, 0f), 0.24f, 0.14f, Terracotta);
            return pot.gameObject;
        }

        private static void BuildDeadPalm(Transform parent, Vector3 position)
        {
            Transform palm = Group("DeadPalm", parent, position, Quaternion.Euler(0f, Rand(0f, 360f), 0f));

            // 幹は節ごとに少しずつ傾けて、枯れてしなった形にする
            const int segments = 5;
            const float segmentLength = 1.1f;
            Transform joint = palm;
            for (int i = 0; i < segments; i++)
            {
                Vector3 jointPosition = i == 0 ? Vector3.zero : new Vector3(0f, segmentLength - 0.05f, 0f);
                joint = Group("TrunkJoint", joint, jointPosition, Quaternion.Euler(Rand(3f, 9f), 0f, 0f));
                float thickness = Mathf.Lerp(0.36f, 0.22f, i / (float)(segments - 1));
                Cyl("Trunk", joint, new Vector3(0f, segmentLength * 0.5f, 0f), thickness, segmentLength, StoneDark, i == 0);
            }

            Transform crown = Group("Crown", joint, new Vector3(0f, segmentLength, 0f));
            const int fronds = 5;
            for (int i = 0; i < fronds; i++)
            {
                Transform frond = Group("Frond", crown, Vector3.zero, Quaternion.Euler(Rand(45f, 70f), i * 360f / fronds, 0f));
                Box("Leaf", frond, new Vector3(0f, 0f, 0.9f), new Vector3(0.28f, 0.05f, 1.8f), Stone, false);
            }
        }

        private static void BuildChest(Transform parent, Vector3 position)
        {
            Transform chest = Group("HiddenChest", parent, position, Quaternion.Euler(0f, 200f, 0f));
            Box("Base", chest, new Vector3(0f, 0.25f, 0f), new Vector3(0.9f, 0.5f, 0.6f), Wood);
            GameObject lid = Cyl("Lid", chest, new Vector3(0f, 0.5f, 0f), 0.6f, 0.9f, Wood);
            lid.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            Box("Lock", chest, new Vector3(0f, 0.45f, 0.31f), new Vector3(0.16f, 0.2f, 0.04f), Gold, false);
            Ball("SandCover", chest, new Vector3(-0.5f, 0f, -0.2f), new Vector3(1.6f, 0.7f, 1.4f), Sand);
        }

        private static void BuildCart(Transform parent, Vector3 position, Vector3 facing)
        {
            // 逃げる途中で置き去りにされた荷車。片輪が外れて傾いている
            Transform cart = Group("AbandonedCart", parent, position, Face(facing) * Quaternion.Euler(0f, 0f, 9f));
            Box("Bed", cart, new Vector3(0f, 0.7f, 0f), new Vector3(1.4f, 0.14f, 2.2f), Wood);
            Box("SideL", cart, new Vector3(-0.66f, 0.95f, 0f), new Vector3(0.08f, 0.4f, 2.2f), Wood, false);
            Box("SideR", cart, new Vector3(0.66f, 0.95f, 0f), new Vector3(0.08f, 0.4f, 2.2f), Wood, false);

            GameObject wheel = Cyl("Wheel", cart, new Vector3(0.8f, 0.5f, -0.3f), 1f, 0.1f, Wood);
            wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            GameObject lostWheel = Cyl("LostWheel", cart, new Vector3(-1.5f, 0.12f, 0.9f), 1f, 0.1f, Wood);
            lostWheel.transform.localRotation = Quaternion.Euler(0f, 0f, -9f);

            GameObject handle = Cyl("Handle", cart, new Vector3(0f, 0.6f, 1.7f), 0.08f, 1.4f, Wood);
            handle.transform.localRotation = Quaternion.Euler(80f, 0f, 0f);
            BuildPot(cart, new Vector3(0.2f, 0.77f, -0.5f), 0.9f);
        }

        private static void ScatterRubble(Transform parent, Vector3 center, float radius, int count, Color color)
        {
            for (int i = 0; i < count; i++)
            {
                float size = Rand(0.25f, 0.7f);
                Vector3 position = center + new Vector3(Rand(-radius, radius), size * 0.3f, Rand(-radius, radius));
                GameObject rubble = Box("Rubble", parent, position, new Vector3(size, size * 0.6f, size * Rand(0.7f, 1.3f)), color, false);
                rubble.transform.localRotation = Quaternion.Euler(Rand(-20f, 20f), Rand(0f, 180f), Rand(-20f, 20f));
            }
        }

        private static void BuildInspectPoint(Transform parent, InspectSpec spec)
        {
            GameObject point = new GameObject("Inspect_" + spec.title);
            point.transform.SetParent(parent, false);
            point.transform.localPosition = spec.position + Vector3.up;
            point.AddComponent<InspectPoint>().Setup(spec.title, spec.body, InspectRadius);
        }

        #endregion

        #region Environment

        private static void BuildEnvironment(Transform root)
        {
            Transform environment = Group("Environment", root);

            // 低めの西日。長い影で石像のシルエットを立たせる
            GameObject sun = new GameObject("Sun");
            sun.transform.SetParent(environment, false);
            sun.transform.localRotation = Quaternion.Euler(36f, -40f, 0f);
            Light light = sun.AddComponent<Light>();
            light.type = LightType.Directional;
            light.color = new Color(1f, 0.92f, 0.78f);
            light.intensity = 1.25f;
            light.shadows = LightShadows.Soft;

            GameObject cameraObject = new GameObject("FieldPreviewCamera");
            cameraObject.transform.SetParent(environment, false);
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 35f;
            camera.farClipPlane = 300f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.93f, 0.84f, 0.68f);
            cameraObject.AddComponent<FieldPreviewCamera>().ApplyTransform();

            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.Linear;
            RenderSettings.fogColor = new Color(0.93f, 0.84f, 0.68f);
            RenderSettings.fogStartDistance = 70f;
            RenderSettings.fogEndDistance = 160f;
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.62f, 0.55f, 0.48f);
        }

        #endregion

        #region Helpers

        private static Transform Group(string name, Transform parent)
        {
            return Group(name, parent, Vector3.zero, Quaternion.identity);
        }

        private static Transform Group(string name, Transform parent, Vector3 localPosition)
        {
            return Group(name, parent, localPosition, Quaternion.identity);
        }

        private static Transform Group(string name, Transform parent, Vector3 localPosition, Quaternion localRotation)
        {
            GameObject group = new GameObject(name);
            group.transform.SetParent(parent, false);
            group.transform.localPosition = localPosition;
            group.transform.localRotation = localRotation;
            return group.transform;
        }

        private static GameObject Box(string name, Transform parent, Vector3 center, Vector3 size, Color color, bool keepCollider = true)
        {
            return Primitive(PrimitiveType.Cube, name, parent, center, size, color, keepCollider);
        }

        // Cylinderプリミティブは高さ2m・直径1mなので、実寸で指定できるよう換算する
        private static GameObject Cyl(string name, Transform parent, Vector3 center, float diameter, float height, Color color, bool keepCollider = false)
        {
            Vector3 scale = new Vector3(diameter, height * 0.5f, diameter);
            return Primitive(PrimitiveType.Cylinder, name, parent, center, scale, color, keepCollider);
        }

        private static GameObject Ball(string name, Transform parent, Vector3 center, Vector3 size, Color color)
        {
            return Primitive(PrimitiveType.Sphere, name, parent, center, size, color, false);
        }

        private static GameObject Primitive(PrimitiveType type, string name, Transform parent, Vector3 center, Vector3 scale, Color color, bool keepCollider)
        {
            GameObject primitive = GameObject.CreatePrimitive(type);
            primitive.name = name;
            primitive.transform.SetParent(parent, false);
            primitive.transform.localPosition = center;
            primitive.transform.localScale = scale;
            Paint(primitive, color);

            if (!keepCollider)
            {
                DestroySafely(primitive.GetComponent<Collider>());
            }

            return primitive;
        }

        private static void Paint(GameObject target, Color color)
        {
            target.GetComponent<Renderer>().sharedMaterial = GetMaterial(color);
        }

        private static Material GetMaterial(Color color)
        {
            Material material;
            if (materialCache.TryGetValue(color, out material))
            {
                return material;
            }

            material = new Material(litShader);
            material.name = "PetrifiedVillage_" + ColorUtility.ToHtmlStringRGB(color);
            material.color = color;
            // URP/HDRPは_BaseColor、ビルトインは_Colorなので両方に入れておく
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }
            if (material.HasProperty("_Smoothness"))
            {
                material.SetFloat("_Smoothness", 0.05f);
            }
            if (material.HasProperty("_Glossiness"))
            {
                material.SetFloat("_Glossiness", 0.05f);
            }

            materialCache.Add(color, material);
            return material;
        }

        private static Shader FindLitShader()
        {
            string[] candidates = { "Universal Render Pipeline/Lit", "HDRP/Lit", "Standard" };
            foreach (string candidate in candidates)
            {
                Shader shader = Shader.Find(candidate);
                if (shader != null)
                {
                    return shader;
                }
            }

            return Shader.Find("Diffuse");
        }

        private static Quaternion Face(Vector3 facing)
        {
            return Quaternion.LookRotation(facing.normalized, Vector3.up);
        }

        private static float Rand(float min, float max)
        {
            return min + (float)rng.NextDouble() * (max - min);
        }

        private static void SetStaticRecursively(Transform target)
        {
            target.gameObject.isStatic = true;
            foreach (Transform child in target)
            {
                SetStaticRecursively(child);
            }
        }

        private static void DestroySafely(Object target)
        {
            if (target == null)
            {
                return;
            }

            if (Application.isPlaying)
            {
                Object.Destroy(target);
            }
            else
            {
                Object.DestroyImmediate(target);
            }
        }

        #endregion
    }
}
