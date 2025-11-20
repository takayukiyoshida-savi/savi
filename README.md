# Unity サンプルゲームプロジェクト

このリポジトリは Unity を使用した 2D サンプルゲーム開発プロジェクトです。
シンプルなゲームシステムの実装例として、プレイヤー操作、敵の生成・AI、UI表示などの基本的な機能を提供します。

## プロジェクト概要

- **ゲームタイプ**: 2D アクションゲーム
- **プレイヤー**: WASD / 方向キーで移動可能
- **敵システム**: 自動生成される敵がプレイヤーを追跡
- **UI**: 経過時間、倒した敵数、スコアを表示

## 開発環境

- **Unity バージョン**: Unity 2022.3 LTS 以降を推奨
- **対象プラットフォーム**: PC (Windows/Mac/Linux)
- **開発言語**: C# (.NET Standard 2.1)

## リポジトリ構成

```
savi/
├── Assets/
│   ├── Scripts/
│   │   ├── Core/                 # ゲーム全体の管理
│   │   │   └── GameBootstrap.cs
│   │   └── Systems/              # 各システムの実装
│   │       ├── Player/           # プレイヤー関連
│   │       │   └── PlayerController.cs
│   │       ├── Enemy/            # 敵関連
│   │       │   ├── EnemySpawner.cs
│   │       │   └── EnemyController.cs
│   │       └── UI/               # UI関連
│   │           └── UIController.cs
│   ├── Art/
│   │   └── Sprites/              # スプライト素材
│   ├── Prefabs/                  # プレハブファイル
│   └── Scenes/                   # シーンファイル
├── ProjectSettings/              # Unity プロジェクト設定
├── README.md                     # このファイル
├── LICENSE                       # ライセンス情報
└── .gitignore                    # Git 除外設定
```

## セットアップ手順

### 1. Unity プロジェクトの作成

1. Unity Hub を開き、「新規プロジェクト」を選択
2. **2D (Core)** テンプレートを選択
3. プロジェクト名を設定（例: "SaviSampleGame"）
4. プロジェクトを作成

### 2. リポジトリの取り込み

1. 作成した Unity プロジェクトのフォルダを開く
2. 既存の `Assets` フォルダをバックアップ（必要に応じて）
3. このリポジトリをクローン:
   ```bash
   git clone <このリポジトリのURL> temp_savi
   ```
4. クローンした内容を Unity プロジェクトに統合:
   ```bash
   # Assets フォルダを上書き
   cp -r temp_savi/Assets/* <UnityプロジェクトPath>/Assets/
   
   # .gitignore をコピー
   cp temp_savi/.gitignore <UnityプロジェクトPath>/
   
   # README.md をコピー（必要に応じて）
   cp temp_savi/README.md <UnityプロジェクトPath>/
   ```
5. 一時フォルダを削除:
   ```bash
   rm -rf temp_savi
   ```

### 3. Unity エディタでの設定

1. Unity エディタでプロジェクトを開く
2. `Assets/Scripts` 内のスクリプトがコンパイルされることを確認
3. 新しいシーンを作成（`File > New Scene`）
4. 以下のオブジェクトを配置:
   - **GameBootstrap**: 空の GameObject に `GameBootstrap.cs` をアタッチ
   - **Player**: 2D スプライト + `PlayerController.cs` + `Rigidbody2D` + "Player" タグ
   - **EnemySpawner**: 空の GameObject に `EnemySpawner.cs` をアタッチ
   - **UI Canvas**: UI Canvas + Text 要素 + `UIController.cs`

## スクリプト説明

### Core システム

- **GameBootstrap.cs**: ゲーム開始時の初期化処理を管理

### Player システム

- **PlayerController.cs**: プレイヤーの移動制御（WASD/方向キー対応）

### Enemy システム

- **EnemySpawner.cs**: 敵の自動生成システム
- **EnemyController.cs**: 敵の AI（プレイヤー追跡）

### UI システム

- **UIController.cs**: ゲーム情報の表示（時間、スコア、敵撃破数）

## 開発のポイント

### 名前空間の使用

各システムは適切な名前空間で分離されています:
- `TechSample.Core`
- `TechSample.Systems.Player`
- `TechSample.Systems.Enemy`
- `TechSample.Systems.UI`

### パフォーマンス考慮

- `Update()` 処理は最小限に抑制
- 物理演算は `FixedUpdate()` で実行
- 重い処理は `Coroutine` を活用

### デバッグ機能

各スクリプトにはデバッグ用の設定が含まれており、開発時の動作確認が容易です。

## 拡張のアイデア

- **武器システム**: プレイヤーが敵を攻撃できる機能
- **パワーアップ**: アイテム収集によるプレイヤー強化
- **ステージシステム**: 複数のレベル・難易度設定
- **サウンドシステム**: BGM・効果音の追加
- **セーブシステム**: ハイスコアの保存機能

## ライセンス

このプロジェクトは MIT ライセンスの下で公開されています。
詳細は [LICENSE](LICENSE) ファイルをご確認ください。

## 貢献

バグ報告や機能提案は Issue でお知らせください。
プルリクエストも歓迎します。

---

**注意**: このプロジェクトは学習・サンプル用途を想定しています。
商用利用の際は適切なライセンス確認を行ってください。