using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shadowsocks.View
{
    public partial class ServerNodes : Form
    {
        public ServerNodes()
        {
            InitializeComponent();
            for (int i = 0; i < 20; i++)
            {
                listView1.Items.Add(new ListViewItem(new string[] {"行" + i, ""}));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
