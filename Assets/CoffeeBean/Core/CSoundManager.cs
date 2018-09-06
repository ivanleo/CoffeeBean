﻿/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:31
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CSoundManager.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CSoundManager
    file ext:   cs
    author:     Leo

    purpose:    音效库
*********************************************************************/

using UnityEngine;
namespace CoffeeBean
{
    /// <summary>
    /// 音效库
    /// </summary>
    public class CSoundManager : CSingleton<CSoundManager>, IMsgSender
    {
        // 是否允许音乐
        public bool IsEnableMusic { get; set; }

        // 是否允许音效
        public bool IsEnableEffect { get; set; }

        // 音乐播放组件
        private AudioSource m_MusicComp = null;

        // 默认背景音乐
        private const string m_DefaultBackground = "";

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            IsEnableEffect = CLocalizeData.LoadData<int> ( "DISABLE_MUSIC" ) == 0;
            IsEnableMusic = IsEnableEffect;

            return;
        }

        /// <summary>
        /// 安全的得到音乐组件
        /// </summary>
        private AudioSource GetMusicComponent()
        {
            if ( m_MusicComp != null )
            {
                return m_MusicComp;
            }

            GameObject MusicOB = new GameObject ( "MusicNode" );
            //记录音源组件
            m_MusicComp = MusicOB.AddComponent<AudioSource>();

            GameObject.DontDestroyOnLoad ( MusicOB );

            return m_MusicComp;
        }

        /// <summary>
        /// 安全的得到音效组件
        /// </summary>
        private AudioSource GetEffectComponent()
        {
            GameObject EffectOB = new GameObject ( "EffectNode" );
            //记录音源组件
            AudioSource AS = EffectOB.AddComponent<AudioSource>();

            return AS;
        }


        /// <summary>
        /// 设置当前音乐音量
        /// </summary>
        /// <param name="Volume">音量 0.0f-1.0f </param>
        public void SetMusicVolume ( float Volume )
        {
            GetMusicComponent().volume = Volume;
        }


        /// <summary>
        /// 停止音乐播放
        /// 播放头回到开始
        /// </summary>
        public void StopMusic()
        {
            GetMusicComponent().Stop();
        }

        /// <summary>
        /// 暂停音乐播放
        /// </summary>
        public void PauseMusic()
        {
            GetMusicComponent().Pause();
        }

        /// <summary>
        /// 恢复音乐播放
        /// </summary>
        public void ResumeMusic()
        {
            AudioSource AS  = GetMusicComponent();
            if ( AS.clip == null )
            {
                AS.clip = CResourcesManager.LoadAudio ( m_DefaultBackground );
            }

            AS.loop = true;
            AS.Play();
        }

        /// <summary>
        /// 播放音乐
        /// </summary>
        /// <param name="FileName">文件名</param>
        /// <param name="IsLoop">是否循环</param>
        public void PlayMusic ( string FilePath, bool IsLoop = true )
        {
            if ( !IsEnableMusic )
            {
                return;
            }

            AudioSource AS = GetMusicComponent();
            AS.clip = CResourcesManager.LoadAudio ( FilePath );
            AS.loop = IsLoop;
            AS.Play();
        }

        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="FileName">文件名</param>
        public void PlayEffect ( string FilePath )
        {
            //CLOG.I ( "play effect {0}", FilePath );
            if ( !IsEnableEffect )
            {
                return;
            }

            AudioSource AS = GetEffectComponent();
            AS.clip = CResourcesManager.LoadAudio ( FilePath, true );
            AS.Play();
            GameObject.Destroy ( AS.gameObject, AS.clip.length );
        }

        /// <summary>
        /// 禁止音乐播放
        /// </summary>
        public void DisableSound()
        {
            StopMusic();

            IsEnableEffect = false;
            IsEnableMusic = false;

            //记录禁止音乐播放了
            CLocalizeData.SaveData<int> ( "DISABLE_MUSIC", 1 );

            this.DispatchMessage ( ECustomMessageType.DISABLE_SOUND );
        }

        /// <summary>
        /// 允许声音播放
        /// </summary>
        public void AbleSound()
        {
            ResumeMusic();
            IsEnableEffect = true;
            IsEnableMusic = true;

            //记录静音了
            CLocalizeData.SaveData<int> ( "DISABLE_MUSIC", 0 );
            this.DispatchMessage ( ECustomMessageType.ABLE_SOUND );
        }

    }

}