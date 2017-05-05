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

        public WebQQClient qq { get; set; }

        /// <summary>
        /// 登录成功处理
        /// </summary>
        public QQNotifyLoginSuccessEventHandler loginSuccessHandler { get; set; }
        /// <summary>
        /// 接收群活讨论组消息处理
        /// </summary>
        public QQNotifyGroupMsgEventHandler GroupMsgHandler { get; set; }

        public bool QrCodeInvalid { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取二维码
            qq = new WebQQClient((client, notifyEvent) =>
            {
                switch (notifyEvent.Type)
                {
                    case QQNotifyEventType.LoginSuccess:
                        {
                            loginSuccessHandler?.Invoke(client);
                            OpenMainForm();
                            break;
                        }
                    case QQNotifyEventType.GroupMsg:
                        {
                            var revMsg = (QQMsg)notifyEvent.Target;
                            string msg = revMsg.GetText();
                            List<string> urls = new List<string>();
                            urls = UrlUtils.GetUrls(msg);
                            GroupMsgHandler?.Invoke(client, revMsg);
                            break;
                        }
                    case QQNotifyEventType.QrcodeReady:
                        {
                            var verify = (Image)notifyEvent.Target;
                            const string path = "verify.png";
                            verify.Save(path);
                            setQrCode(verify);
                            break;
                        }
                    case QQNotifyEventType.LoadBuddySuccess:
                        {
                            QQGlobal.QQBuddyLoadSuccess = true;
                            QQGlobal.client = client;
                            break;
                        }
                    case QQNotifyEventType.LoadGroupSuccess:
                        {
                            QQGlobal.QQGroupLoadSuccess = true;
                            QQGlobal.client = client;
                            break;
                        }

                    case QQNotifyEventType.QrcodeInvalid:
                        {
                            QrCodeInvalid = true;
                            setQrCode(Properties.Resources.QQBG);                            
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            });
            qq.LoginWithQRCode(); // 登录之后自动开始轮训

        }




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
                if(QrCodeInvalid)
                    picQQ.Visible = false;
                else
                    picQQ.Visible = true;
            }
        }

        private void OpenMainForm()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(OpenMainForm), new object[] { });
            }
            else
            {
                this.Hide();
                QQGroupList f2 = new QQGroupList(this);
                f2.Show();
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
                if (qq != null)
                    qq.LoginWithQRCode();
                picQrcode.SizeMode = PictureBoxSizeMode.CenterImage;
                picQrcode.Image = Properties.Resources.loading;
                picQQ.Visible = false;
            }
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Process.GetCurrentProcess().Kill();
        }
    }
}
