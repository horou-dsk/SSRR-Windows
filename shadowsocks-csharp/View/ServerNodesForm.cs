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
using Shadowsocks.Controller;
using Shadowsocks.Model;

namespace Shadowsocks.View
{
    public partial class ServerNodesForm : Form
    {

        private readonly ShadowsocksController _controller;
        private int selectIndex;
        public ServerNodesForm(ShadowsocksController controller)
        {
            _controller = controller;
            selectIndex = controller.GetCurrentConfiguration().index;
            InitializeComponent();

            /*var selected = new ColumnHeader
            {
                Width = 120,
                Text = "  "
            };
            this.listView1.Columns.Add(selected);
            var delayHeader = new ColumnHeader
            {
                Text = "延迟",
                Width = 120,
                TextAlign = HorizontalAlignment.Center
            };
            this.listView1.Columns.Add(delayHeader);*/
            var nodeName = new ColumnHeader
            {
                Width = 400,
                Text = "节点名称",
                TextAlign = HorizontalAlignment.Center
            };
            this.listView1.Columns.Add(nodeName);
            this.listView1.MouseClick += ListView_MouseDoubleClick;
            UpdateList();
        }

        private void UpdateList()
        {
            this.listView1.Items.Clear();
            this.listView1.BeginUpdate();
            var configuration = _controller.GetCurrentConfiguration();
            const string def_group = "!(no group)";
            var index = 0;
            foreach (var server in configuration.configs)
            {
                /*if (groupName != server.@group)
                {
                    continue;
                }*/

                string latency;
                if (server.latency == Server.LATENCY_TESTING)
                {
                    latency = "[testing]";
                }
                else if (server.latency == Server.LATENCY_ERROR)
                {
                    latency = "[error]";
                }
                else if (server.latency == Server.LATENCY_PENDING)
                {
                    latency = "[pending]";
                }
                else
                {
                    latency = "[" + server.latency.ToString() + "ms]";
                }

                var lvi = new ListViewItem
                {
                    BackColor = index == selectIndex ? Color.Cyan : Color.White,
                    Text = (index == configuration.index ? "√ " : "  ") + latency + "  " + server.FriendlyName(),
                    Tag = index
                };
                // lvi.SubItems.Add(latency);
                // lvi.SubItems.Add(server.FriendlyName());
                this.listView1.Items.Add(lvi);
                index++;

            }
            this.listView1.EndUpdate();
        }

        private void NodesForm_Load(object sender, EventArgs e)
        {

        }

        private void ListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var lv = sender as ListView;
            if (lv == null)
                return;
            var info = lv.HitTest(e.X, e.Y);
            var curItem = info.Item;
            selectIndex = (int) curItem.Tag;
            _controller.SelectServerIndex(selectIndex);
            var config = _controller.GetCurrentConfiguration();
            foreach (var server in config.configs)
            {
                server.GetConnections().CloseAll();
            }
            Close();
            // UpdateList();
        }
    }
}
