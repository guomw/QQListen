using iQQ.Net.WebQQCore.Im;
using iQQ.Net.WebQQCore.Im.Actor;
using iQQ.Net.WebQQCore.Im.Bean;
using iQQ.Net.WebQQCore.Im.Bean.Content;
using iQQ.Net.WebQQCore.Im.Event;
using iQQ.Net.WebQQCore.Im.Log;
using iQQ.Net.WebQQCore.Util.Extensions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQLogin
{
    public partial class Form1 : Form
    {

        private static Image qrCodeImage { get; set; }

        private static bool LoginSuccess { get; set; }
        /// <summary>
        /// 群加载完成
        /// </summary>
        public static bool QQGroupLoadSuccess { get; set; }
        /// <summary>
        /// 好友加载完成
        /// </summary>
        public static bool QQBuddyLoadSuccess { get; set; }
        public static IQQClient QQClient { get; set; }

        private static readonly QQNotifyListener Listener = (client, notifyEvent) =>
       {
           switch (notifyEvent.Type)
           {
               case QQNotifyEventType.LoginSuccess:
                   {
                       //client.Logger.LogInformation("登录成功");
                       LoginSuccess = true;                       
                       break;
                   }

               case QQNotifyEventType.GroupMsg:
                   {
                       var revMsg = (QQMsg)notifyEvent.Target;                       
                       //client.Logger.LogInformation($"群[{revMsg.Group.Name}]-好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");
                       break;
                   }

               case QQNotifyEventType.ChatMsg:
                   {
                       var revMsg = (QQMsg)notifyEvent.Target;
                       //client.Logger.LogInformation($"好友[{revMsg.From.Nickname}]：{revMsg.GetText()}");

                       var msgReply = new QQMsg()
                       {
                           Type = QQMsgType.BUDDY_MSG,
                           To = revMsg.From,
                           From = client.Account,
                           Date = DateTime.Now,
                       };
                       msgReply.AddContentItem(new TextItem("自动回复")); // 添加文本内容
                       msgReply.AddContentItem(new FaceItem(0));            // QQ id为0的表情
                       msgReply.AddContentItem(new FontItem());             // 使用默认字体

                       client.SendMsg(msgReply);
                       break;
                   }

               case QQNotifyEventType.QrcodeReady:
                   {
                       var verify = (Image)notifyEvent.Target;
                       const string path = "verify.png";
                       verify.Save(path);
                       //client.Logger.LogInformation("请扫描在项目根目录下qrcode.png图片");
                       qrCodeImage = verify;
                       break;
                   }

               case QQNotifyEventType.QrcodeSuccess:
                   {
                       
                       break;
                   }

               case QQNotifyEventType.LoadBuddySuccess:
                   {
                       QQBuddyLoadSuccess = true;
                       QQClient = client;
                       break;
                   }

               case QQNotifyEventType.LoadGroupSuccess:
                   {
                       QQGroupLoadSuccess = true;
                       QQClient = client;
                       break;
                   }

               case QQNotifyEventType.QrcodeInvalid:
                   {
                       //client.Logger.LogWarning("二维码已失效");
                       break;
                   }               
               default:
                   {
                       // client.Logger.LogInformation(notifyEvent.Type.GetFullDescription());
                       break;
                   }
           }
       };


        public Form1()
        {
            InitializeComponent();
        }

        public WebQQClient qq { get; set; }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 获取二维码
            qq = new WebQQClient("", "", Listener, new SimpleActorDispatcher(), new QQConsoleLogger());
            qq.LoginWithQRCode(); // 登录之后自动开始轮训


            

            new Thread(() => {

                while (qrCodeImage == null) { }

                setQrCode(qrCodeImage);


                while (!LoginSuccess) { }

                OpenMainForm();



            }) { IsBackground = true }.Start();

        }

        private void setQrCode(Image verify)
        {
            if (this.picQrcode.InvokeRequired)
            {
                this.picQrcode.Invoke(new Action<Image>(setQrCode), new object[] { verify });
            }
            else
            {
                picQrcode.Image = verify;
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
                Form2 f2 = new Form2(this);
                f2.Show();
            }

        }

    }
}
