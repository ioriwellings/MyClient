﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class userManage : Form
    {
        service.UserService us = new service.UserService();
        public userManage()
        {
            InitializeComponent();
        }

        private void userManage_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/insert.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/update.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/close.png");

            queryUser();
        }
        private void queryUser()
        {   //查询所有用户信息
            DataTable dd= us.listUser();
            dd.Columns.Add("neir", typeof(string));
            foreach (DataRow rows in dd.Rows)
            {
                if (rows[1].ToString() == "user") 
                {
                    rows["neir"] = "管理员";
                }
                else {
                    rows["neir"] = "普通用户";
                }
                rows[2] = "******";
            }
            this.dataGridView1.DataSource = dd;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[1].HeaderCell.Value = "用户名称(登录用)";
            this.dataGridView1.Columns[2].HeaderCell.Value = "密码";
            this.dataGridView1.Columns[3].HeaderCell.Value = "用户状态";
            this.dataGridView1.Columns[4].HeaderCell.Value = "创建时间";
            this.dataGridView1.Columns[5].Visible = false;
            this.dataGridView1.Columns[6].HeaderCell.Value = "用户说明";
            this.dataGridView1.Columns[1].Width = 110;
            this.dataGridView1.Columns[4].Width = 130;
            this.dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            this.dataGridView1.AllowUserToAddRows = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addUser au = new addUser();
            if (au.ShowDialog() == DialogResult.OK) {
                queryUser();
                MessageBox.Show("添加成功！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string id = this.dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            string name = this.dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            string enable=this.dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            string power = this.dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            if (name == "admin" || name == "user")
            {
                MessageBox.Show("管理员账号不能被修改！");
                return;
            }
            if (name != null && !"".Equals(name))
            {
                updateUser up = new updateUser();
                up.textBox1.Text = name;
                up.textBox2.Text = id;
                if (enable == "启用") { up.comboBox1.Text = "1"; } else { up.comboBox1.Text = "0"; }
                //up.comboBox1.Text = enable;
                if (name == "admin")
                {
                    foreach (Control ctr in up.groupBox2.Controls)
                    {
                        //判断该控件是不是CheckBox
                        if (ctr is CheckBox)
                        {
                            //将ctr转换成CheckBox并赋值给ck
                            CheckBox ck = ctr as CheckBox;
                            ck.Checked = true;
                        }
                    }
                }
                else { 
                List<string> list = power.Split(',').ToList();
                foreach (Control ctr in up.groupBox2.Controls)
                {
                    //判断该控件是不是CheckBox
                    if (ctr is CheckBox)
                    {
                        //将ctr转换成CheckBox并赋值给ck
                        CheckBox ck = ctr as CheckBox;
                        if (list.Contains(ck.Text))
                        {
                            ck.Checked = true;
                        }
                    }
                }
            }
                if (up.ShowDialog() == DialogResult.OK)
                {
                    queryUser();
                    MessageBox.Show("修改成功！");
                }
            }
           
           
        }
    }
}
