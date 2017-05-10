﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Core;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Event.Future;
using iQQ.Net.WebQQCore.Util;
using Microsoft.Extensions.Logging;

namespace iQQ.Net.WebQQCore.Im.Module
{
    /// <summary>
    /// 处理整体登陆逻辑
    /// 表示一系列连续的action
    /// </summary>
    public class ProcModule : AbstractModule
    {
        private IQQActionFuture _pollFuture;

        public IQQActionFuture LoginWithQRCode(QQActionListener listener)
        {
            var future = new ProcActionFuture(listener, true);
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetQRCode((sender, @event) =>
            {
                if (@event.Type == QQActionEventType.EvtOK)
                {
                    var verify = (Image)@event.Target;
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeReady, verify));
                    DoCheckQRCode(future);
                    // future.NotifyActionEvent(QQActionEventType.EvtOK, @event.Target);
                }
                else if (@event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, @event.Target);
                }
            });
            return future;
        }

        private void DoCheckQRCode(ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            QQActionListener handler = null;
            handler = (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    var eventArgs = (CheckQRCodeArgs)Event.Target;
                    switch (eventArgs.Status)
                    {
                        case QRCodeStatus.QRCODE_OK:
                            Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeSuccess));
                            DoCheckLoginSig(eventArgs.Msg, future);
                            break;

                        case QRCodeStatus.QRCODE_VALID:
                            Thread.Sleep(3000);
                            login.CheckQRCode(handler);
                            break;
                        case QRCodeStatus.QRCODE_AUTH:
                            Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeAuth, eventArgs.Msg));
                            login.CheckQRCode(handler);
                            break;

                        case QRCodeStatus.QRCODE_INVALID:
                            Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.QrcodeInvalid, eventArgs.Msg));
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            };
            login.CheckQRCode(handler);
        }

        public IQQActionFuture Login(QQActionListener listener)
        {
            var future = new ProcActionFuture(listener, true);
            // DoGetLoginSig(future); // 这里可以直接替换成 DoCheckVerify(future);
            DoCheckVerify(future);
            return future;
        }

        public IQQActionFuture LoginWithVerify(string verifyCode, ProcActionFuture future)
        {
            DoWebLogin(verifyCode, future);
            return future;
        }

        private void DoGetLoginSig(ProcActionFuture future)
        {
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetLoginSig((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    DoCheckVerify(future);
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        private void DoGetVerify(string reason, ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var account = Context.Account;
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetCaptcha(account.Uin, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    var verify = new ImageVerify()
                    {
                        Type = ImageVerify.VerifyType.LOGIN,
                        Image = (Image)Event.Target,
                        Reason = reason,
                        Future = future
                    };
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.CapachaVerify, verify));
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        private void DoCheckVerify(ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            var account = Context.Account;
            login.CheckVerify(account.Username, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    var args = (CheckVerifyArgs)(Event.Target);
                    Context.Account.Uin = args.Uin;
                    if (args.Result == 0)
                    {
                        DoWebLogin(args.Code, future);
                    }
                    else
                    {
                        Context.Session.CapCd = args.Code;
                        DoGetVerify("为了保证您账号的安全，请输入验证码中字符继续登录。", future);
                    }
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        private void DoWebLogin(string verifyCode, ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.WebLogin(Context.Account.Username, Context.Account.Password, Context.Account.Uin,
                verifyCode, ((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    DoCheckLoginSig((string)Event.Target, future);
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    var ex = (QQException)(Event.Target);
                    if (ex.ErrorCode == QQErrorCode.WrongCaptcha)
                    {
                        DoGetVerify(ex.Message, future);
                    }
                    else
                    {
                        future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                    }
                }
            }));
        }

        private void DoCheckLoginSig(string checkSigUrl, ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.CheckLoginSig(checkSigUrl, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    DoGetVFWebqq(future);
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        private void DoGetVFWebqq(ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            login.GetVFWebqq((sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    DoChannelLogin(future);
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        private void DoChannelLogin(ProcActionFuture future)
        {
            if (future.IsCanceled) return;

            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            if (Context.Account.Status == QQStatus.OFFLINE) Context.Account.Status = QQStatus.ONLINE;

            login.ChannelLogin(Context.Account.Status, async (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtOK, null);
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.LoginSuccess));
                    var task1 = Context.GetModule<GroupModule>(QQModuleType.GROUP).GetGroupList((s, e) =>
                    {
                        if (e.Type == QQActionEventType.EvtOK)
                        {
                            Context.Logger.LogInformation($"获取群列表成功，共{Context.Store.GroupCount}个群");
                        }
                    }).WhenFinalEvent();
                    var task2 = Context.GetModule<CategoryModule>(QQModuleType.CATEGORY).GetBuddyList((s, e) =>
                    {
                        if (e.Type == QQActionEventType.EvtOK)
                        {
                            Context.Logger.LogInformation($"获取好友列表成功，共{Context.Store.BuddyCount}个好友");
                        }
                    }).WhenFinalEvent();
                    var task3 = Context.GetModule<LoginModule>(QQModuleType.LOGIN).GetSelfInfo((s, e) =>
                    {
                        if (e.Type == QQActionEventType.EvtOK) Context.Logger.LogInformation("获取个人信息成功");
                    }).WhenFinalEvent();
                    var task4 = Context.GetModule<BuddyModule>(QQModuleType.BUDDY).GetOnlineBuddy((s, e) =>
                    {
                        if (e.Type == QQActionEventType.EvtOK) Context.Logger.LogInformation($"获取在线好友信息成功，共{Context.Store.GetOnlineBuddyList().Count()}个在线好友");
                    }).WhenFinalEvent();

                    await Task.WhenAll(task1, task2, task3, task4).ConfigureAwait(false);
                    DoPollMsg();
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    future.NotifyActionEvent(QQActionEventType.EvtError, Event.Target);
                }
            });
        }

        public IQQActionFuture Relogin(QQStatus status, QQActionListener listener)
        {
            Context.Account.Status = status;
            Context.Session.State = QQSessionState.Logining;
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            var future = login.ChannelLogin(status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtError)
                {
                    Context.Logger.LogInformation("iqq client ReloginChannel fail!!! use Login.");
                    Login(listener);
                }
                else
                {
                    listener?.Invoke(this, Event);
                }
            });
            return future;
        }

        public void Relogin()
        {
            Context.Logger.LogInformation("Relogin...");
            var session = Context.Session;
            if (session.State == QQSessionState.Logining) return;
            // 登录失效，重新登录
            Relogin(Context.Account.Status, (sender, Event) =>
            {
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    // 重新登录成功重新POLL
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.ReloginSuccess));
                    DoPollMsg();
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.NetError, Event.Target));
                }
            });
        }

        /// <summary>
        /// 轮询新消息，发送心跳包，也就是挂qq
        /// </summary>
        public void DoPollMsg()
        {
            Context.Logger.LogInformation("begin to poll...");
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            _pollFuture = login.PollMsg((sender, Event) =>
            {
                // 回调通知事件函数
                if (Event.Type == QQActionEventType.EvtOK)
                {
                    var events = (List<QQNotifyEvent>)Event.Target;
                    foreach (var evt in events)
                    {
                        Context.FireNotify(evt);
                    }

                    // 准备提交下次poll请求
                    var session = Context.Session;
                    if (session.State == QQSessionState.Online)
                    {
                        DoPollMsg();
                        return;
                    }
                    else if (session.State != QQSessionState.Kicked)
                    {
                        Relogin();
                        return;
                    }
                }
                else if (Event.Type == QQActionEventType.EvtError)
                {
                    var ex = (QQException)Event.Target;
                    var code = ex.ErrorCode;
                    if (code == QQErrorCode.IOTimeout)
                    {
                        DoPollMsg();
                        return; // 心跳超时是正常的
                    }

                    //因为自带了错误重试机制，如果出现了错误回调，表明已经超时多次均失败，这里直接返回网络错误的异常
                    var session = Context.Session;
                    var account = Context.Account;
                    session.State = QQSessionState.Offline;

                    switch (code)
                    {
                        case QQErrorCode.NeedToLogin:
                            {
                                DoGetVFWebqq(new ProcActionFuture(null, true));
                                return;
                            }
                        case QQErrorCode.InvalidLoginAuth:
                            {
                                Relogin();
                                return;
                            }
                        case QQErrorCode.IOError:
                        case QQErrorCode.IOTimeout:
                        case QQErrorCode.InvalidResponse:
                            {
                                account.Status = QQStatus.OFFLINE;
                                //粗线了IO异常，直接报网络错误
                                Context.FireNotify(new QQNotifyEvent(QQNotifyEventType.NetError, ex));
                                return;
                            }
                        default:
                            {
                                Context.Logger.LogInformation("poll msg unexpected error, ignore it ...", ex);
                                Relogin();
                                return;
                            }
                    }
                }
                else if (Event.Type == QQActionEventType.EvtRetry)
                {
                    // System.err.println("Poll Retry:" + this);
                    Context.Logger.LogInformation("poll msg error, retrying....", (QQException)Event.Target);
                }
            });
        }

        public void CancelPoll()
        {
            _pollFuture?.Cancel();
        }

        /// <summary>
        /// 退出，下线
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public IQQActionFuture DoLogout(QQActionListener listener)
        {
            CancelPoll();
            var login = Context.GetModule<LoginModule>(QQModuleType.LOGIN);
            return login.Logout(listener);
        }

        public override void Destroy()
        {
            CancelPoll();
            base.Destroy();
        }
    }

}
