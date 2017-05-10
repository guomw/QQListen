﻿using System;
using System.Drawing;
using System.IO;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Http;
using iQQ.Net.WebQQCore.Util;

namespace iQQ.Net.WebQQCore.Im.Action
{
    /// <summary>
    /// <para>获取用户头像</para>
    /// <para>@author ChenZhiHui</para>
    /// <para>@since 2013-2-23</para>
    /// </summary>
    public class GetFriendFaceAction : AbstractHttpAction
    {

        private readonly QQUser _user;
        
        public GetFriendFaceAction(IQQContext context, QQActionListener listener,QQUser user)
            : base(context, listener)
        {
            this._user = user;
        }

        public override QQHttpRequest OnBuildRequest()
        {
            var session = Context.Session;
            var req = CreateHttpRequest(HttpConstants.Get, QQConstants.URL_GET_USER_FACE);
            req.AddGetValue("b", "qq");
            req.AddGetValue("nk", _user.QQ);
            req.AddGetValue("s", 100);
            req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());

            //req.AddGetValue("uin", _user.Uin);
            //req.AddGetValue("vfwebqq", session.Vfwebqq);
            //req.AddGetValue("t", DateTime.Now.CurrentTimeMillis());
            //req.AddGetValue("cache", 0); // ??
            //req.AddGetValue("type", 1); // ??
            //req.AddGetValue("fid", 0); // ??
            req.AddHeader("Referer", "http://w.qq.com");
            req.ResultType = ResponseResultType.Stream;
            return req;
        }

        public override void OnHttpStatusOK(QQHttpResponse response)
        {
            var image = Image.FromStream(response.ResponseStream);
            _user.Face = image;
            NotifyActionEvent(QQActionEventType.EvtOK, _user);
        }
    }
}
