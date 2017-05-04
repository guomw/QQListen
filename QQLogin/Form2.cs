using iQQ.Net.WebQQCore.Im.Bean;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QQLogin
{
    public partial class Form2 : Form
    {
        private Form1 f1 { get; set; }

        public Form2(Form1 f)
        {
            InitializeComponent();

            f1 = f;

        }

        private void Form2_Load(object sender, EventArgs e)
        {




            new System.Threading.Thread(() => {

                while (!Form1.QQGroupLoadSuccess) { }
                SetContactsView(Form1.QQClient.GetGroupList());
            }) { IsBackground = true }.Start();


        }

        /// <summary>
        /// 加载微信通讯录
        /// </summary>
        /// <param name="contact_all">The contact_all.</param>
        public void SetContactsView(List<QQGroup> contact_all)
        {
            if (dgvContact.InvokeRequired)
            {
                this.dgvContact.Invoke(new Action<List<QQGroup>>(SetContactsView), new object[] { contact_all });
            }
            else
            {
                this.dgvContact.Rows.Clear();
                int i = dgvContact.Rows.Count;
                foreach (var user in contact_all)
                {
                    
                    dgvContact.Rows.Add();
                    ++i;
                    //dgvContact.Rows[i - 1].Cells["GroupFace"].Value =user.Face;
                    dgvContact.Rows[i - 1].Cells["GroupTitle"].Value = user.Name;
                    dgvContact.Rows[i - 1].Cells["GroupGid"].Value = user.Gid;
                    dgvContact.Rows[i - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(236, 232, 231);// ConstConfig.DataGridViewOddRowBackColor;                    

                    if (i % 2 == 0)
                    {
                        dgvContact.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);
                        dgvContact.Rows[i - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(248, 248, 248);
                    }
                    else
                    {
                        dgvContact.Rows[i - 1].DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 255);
                        dgvContact.Rows[i - 1].DefaultCellStyle.SelectionBackColor = Color.FromArgb(255, 255, 255);
                    }
                }
            }
        }



        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.ExitThread();
            Process.GetCurrentProcess().Kill();
        }
    }
}
