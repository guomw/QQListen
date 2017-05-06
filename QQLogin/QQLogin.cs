using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Log;
using iQQ.Net.WebQQCore.Util;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQLogin
{
    public partial class QQLogin : Form
    {
        #region 移动窗口
        /*
         * 首先将窗体的边框样式修改为None，让窗体没有标题栏
         * 实现这个效果使用了三个事件：鼠标按下、鼠标弹起、鼠标移动
         * 鼠标按下时更改变量isMouseDown标记窗体可以随鼠标的移动而移动
         * 鼠标移动时根据鼠标的移动量更改窗体的location属性，实现窗体移动
         * 鼠标弹起时更改变量isMouseDown标记窗体不可以随鼠标的移动而移动
         */
        private bool isMouseDown = false;
        private Point FormLocation;     //form的location
        private Point mouseOffset;      //鼠标的按下位置
        private void WinForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isMouseDown = true;
                FormLocation = this.Location;
                mouseOffset = Control.MousePosition;
            }
        }

        private void WinForm_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }
        private void WinForm_MouseMove(object sender, MouseEventArgs e)
        {
            int _x = 0;
            int _y = 0;
            if (isMouseDown)
            {
                Point pt = Control.MousePosition;
                _x = mouseOffset.X - pt.X;
                _y = mouseOffset.Y - pt.Y;

                this.Location = new Point(FormLocation.X - _x, FormLocation.Y - _y);
            }

        }
        #endregion

        public QQLogin()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 登录成功处理
        /// </summary>
        public QQNotifyLoginSuccessEventHandler loginSuccessHandler { get; set; }
        /// <summary>
        /// 群消息处理
        /// </summary>
        public QQNotifyGroupMsgEventHandler GroupMsgHandler { get; set; }
        /// <summary>
        /// 二维码是否失效
        /// </summary>
        public bool QrCodeInvalid { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取二维码
            QQGlobal.client = new WebQQClient((client, notifyEvent) =>
            {
                switch (notifyEvent.Type)
                {
                    case QQNotifyEventType.LoginSuccess:
                        {
                            loginSuccessHandler?.Invoke();
                            OpenMainForm();
                            break;
                        }
                    case QQNotifyEventType.GroupMsg:
                        {
                            var revMsg = (QQMsg)notifyEvent.Target;
                            //判断当前群是否已开启监控
                            bool isListen = QQGlobal.listenGroups.Exists((g) => { return g.Gid == revMsg.Group.Gid; });
                            if (isListen)
                            {
                                string msg = revMsg.GetText();
                                List<string> urls = new List<string>();
                                urls = UrlUtils.GetUrls(msg);
                                GroupMsgHandler?.Invoke(msg, urls);                                
                            }
                            break;
                        }
                    case QQNotifyEventType.QrcodeReady:
                        {
                            var verify = (Image)notifyEvent.Target;
                            const string path = "verify.png";
                            verify.Save(path);
                            setQrCode(verify);
                            SetMsg("请使用QQ手机版扫描二维码");
                            break;
                        }
                    case QQNotifyEventType.QrcodeAuth:
                        {
                            SetMsg("扫描成功，请在手机上确认是否授权登录");
                            break;
                        }
                    case QQNotifyEventType.QrcodeSuccess:
                        {
                            SetMsg("二维码验证成功！正在跳转...");
                            break;
                        }
                    case QQNotifyEventType.LoadGroupSuccess:
                        {
                            QQGlobal.QQGroupLoadSuccess = true;
                            break;
                        }
                    case QQNotifyEventType.QrcodeInvalid:
                        {
                            QrCodeInvalid = true;
                            setQrCode(Properties.Resources.QQBG);
                            break;
                        }
                    case QQNotifyEventType.KickOffline:
                    case QQNotifyEventType.NetError:
                        {
                            //重新登录
                            client.Relogin(QQStatus.ONLINE);
                            break;
                        }
                    default:
                        {
                            //MessageBox.Show(notifyEvent.Target.ToString());
                            break;
                        }
                }
            });
            QQGlobal.client.LoginWithQRCode(); // 登录之后自动开始轮训
        }



        /// <summary>
        /// 设置二维码
        /// </summary>
        /// <param name="verify"></param>
        private void setQrCode(Image verify)
        {
            if (this.picQrcode.InvokeRequired)
            {
                this.picQrcode.Invoke(new Action<Image>(setQrCode), new object[] { verify });
            }
            else
            {
                picQrcode.SizeMode = PictureBoxSizeMode.StretchImage;
                picQrcode.Image = verify;
                if (QrCodeInvalid)
                {
                    picQrcode.Cursor = Cursors.Hand;
                    picQQ.Visible = false;
                }
                else
                {
                    picQrcode.Cursor = Cursors.Default;
                    picQQ.Visible = true;
                }
            }
        }
        /// <summary>
        /// 打开主窗口
        /// </summary>
        private void OpenMainForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(OpenMainForm), new object[] { });
            }
            else
            {
                this.Hide();
                QQGroupList f2 = new QQGroupList();
                f2.Show();
            }
        }

        /// <summary>
        /// 设置二维码扫描状态
        /// </summary>
        /// <param name="text"></param>
        private void SetMsg(string text)
        {
            if (this.lbMsg.InvokeRequired)
            {
                this.lbMsg.Invoke(new Action<string>(SetMsg), new object[] { text });
            }
            else
            {
                lbMsg.Text = text;
            }
        }


        /// <summary>
        /// 刷新二维码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbRefreshQrCode_Click(object sender, EventArgs e)
        {
            if (QrCodeInvalid)
            {
                QrCodeInvalid = false;
                if (QQGlobal.client != null)
                    QQGlobal.client.LoginWithQRCode();
                picQrcode.SizeMode = PictureBoxSizeMode.CenterImage;
                picQrcode.Image = Properties.Resources.loading;
                picQQ.Visible = false;
            }
        }


        /// <summary>
        /// 关闭所以线程窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Process.GetCurrentProcess().Kill();
        }
    }
}
