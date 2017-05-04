﻿using System;

namespace iQQ.Net.WebQQCore.Im.Core
{

    public enum QQSessionState
    {
        Offline,
        Online,
        Kicked,
        Logining,
        Error
    }

    /// <summary>
    /// QQSession保存了每次登陆时候的状态信息
    /// </summary>
    public class QQSession
    {
        private volatile QQSessionState _state; // 作为指令关键字，确保本条指令不会因编译器的优化而省略，且要求每次直接读值.
        public QQSessionState State
        {
            get { return _state; }
            set { _state = value; }
        }

        private long _clientId;
        public long ClientId
        {
            get
            {
                if (_clientId == 0)
                {
                   // _clientId = new Random().Next(); //random??
                    _clientId = 53999199;
                }
                return _clientId;
            }
            set { _clientId = value; }
        }

        public string SessionId { get; set; }
        public string Vfwebqq { get; set; }
        public string Ptwebqq { get; set; }
        public string LoginSig { get; set; }
        public string CfaceKey { get; set; }
        public string CfaceSig { get; set; }
        public string EmailAuthKey { get; set; }
        public int Index { get; set; }
        public int Port { get; set; }
        public int PollErrorCnt { get; set; }
        public string CapCd { get; set; }
        public string Psessionid { get; set; }
    }

}
