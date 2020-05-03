# Previewed Reality Service Caller


# 概要
Previewed Realityで使うタスクサービスをスマートフォンから呼び出すためのアプリケーション


## 関連ソフトのリンク
[ROS-TMS for Smart Previewed Reality](https://github.com/SigmaHayashi/ros_tms_for_smart_previewed_reality)

[Smart Previewed Reality](https://github.com/SigmaHayashi/Smart-Previewed-Reality)


# 必要な環境
PC1 : Windows10 64bit（アプリケーションビルド用）  
PC2 : Ubuntu 16（Smart Previewed Reality実行用）  
※PC1とPC2は同時に起動する必要なし，デュアルブートでOK

Androidスマートフォン : Smart Previewed Realityアプリケーションを実行するスマートフォンとは別のもの

ROS kinetic (Ubuntuにインストールしておく)


# 開発環境
PC : Windows 10 64bit  
* Unity 2018.4.1f1  
* Visual Studio 2017  
* Android Studio 3.5.1  

Android（動作確認済み） : Pixel 3 XL, Pixel 4 XL


# アプリケーションをビルドするためのPCの準備
1. Unityのインストール  
    URL : https://unity3d.com/jp/get-unity/download

1. Visual Studioのインストール  
    ※VS Codeではない  
    ※Unityのインストール中にインストールされるものでOK  
    URL : https://visualstudio.microsoft.com/ja/downloads/

1. Android Studioのインストール  
    ※Android SDKが必要  
    URL : https://developer.android.com/studio


# アプリケーションのインストール方法
1. GitHubから任意の場所にダウンロード

1. Unityでプロジェクトを開く
1. "MainScene"のSceneを開く
1. File > Build Settingsからビルド環境の設定を開く
1. Androidを選択し，Switch Platformを選択
1. Android端末をPCに接続し，Build & Run


# 使い方
1. [Smart Previewed Reality](https://github.com/SigmaHayashi/Smart-Previewed-Reality)の使い方を参考に，ROS-TMS，Smart Previewed Realityアプリケーションを起動する

1. Previewed Reality Service Caller（このアプリ）を起動する

    ※初回起動時は，Settingsボタンを押してROS-TMSを実行しているUbuntu PCのIPアドレスを指定する必要あり（うまく起動しない場合はWi-Fiを一度オフにしてからアプリを起動するとスムーズに起動するかも）

1. アプリケーションを通して，
    * 移動サービス
    * 物品取り寄せサービス
    
    を実行できる
