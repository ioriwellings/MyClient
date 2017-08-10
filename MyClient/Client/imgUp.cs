using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class imgUp : Form
    {
        Label[] labels;
        String overtime;
        DataTable dt = null;
        PrivateFontCollection privateFonts = new PrivateFontCollection();
        service.houseTypeService hts = new service.houseTypeService();
        service.rtmonitoringService ms = new service.rtmonitoringService();
        private static string str = Application.StartupPath;//项目路径
        double a, b, c, d, e1, f;
        private bool flag = false;
        public imgUp()
        {
            InitializeComponent();
        }

        private void imgUp_Load(object sender, EventArgs e)
        {
            string str = Application.StartupPath;//项目路径
            this.button1.BackgroundImage = Image.FromFile(@str + "/images/xzwj.png");
            this.button2.BackgroundImage = Image.FromFile(@str + "/images/close.png");
            this.button3.BackgroundImage = Image.FromFile(@str + "/images/cdbj.png");
            this.button4.BackgroundImage = Image.FromFile(@str + "/images/cdbc.png");
            this.button5.BackgroundImage = Image.FromFile(@str + "/images/tpbc.png");
            privateFonts.AddFontFile(@str + @"/fonts/SIMYOU.TTF");//加载路径的字体
            overtime = Properties.Settings.Default.overTime;
            dt = hts.queryhouseType();
            this.comboBox1.DataSource =dt;//绑定数据源
            this.comboBox1.DisplayMember = "name";//显示给用户的数据集表项
            this.comboBox1.ValueMember = "id";//操作时获取的值 
            imgSwitch(this.comboBox1.SelectedValue.ToString());
            
        }
        private void imgSwitch(string houseCode) {
            this.pictureBox1.Controls.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["id"].ToString()== houseCode)
                {
                    string path=dr["imgPath"].ToString();
                    if (path != null && !"".Equals(path))
                    {
                        this.pictureBox1.Image = Image.FromFile(@str + "/images/" + path);
                        //实时数据
                        imgWenshidu(houseCode);
                    }
                    else {
                        this.timer1.Stop();
                        this.pictureBox1.Controls.Clear();
                        this.pictureBox1.Image = null;
                        MessageBox.Show("当前库房没有库房平面图，请上传。。。");
                        
                    }
                }
            }
        }
        private void imgWenshidu(string houseCode) {
            DataTable dat = ms.queryMonitoringByhousecode(houseCode);
            int num = dat.Rows.Count;
            int kk = 0;
            if (num > 0)
            {
                this.pictureBox1.Controls.Clear();
                Label[] label = new Label[num];//例如10个
                int p1 = 10;
                for (int x = 0; x < num; x++)
                {
                    label[x] = new Label();
                    //label[x].AutoSize = true;
                    label[x].Size = new Size(70, 45);//大小
                    string ss=dat.Rows[x]["terminalname"].ToString();
                    b = Double.Parse(dat.Rows[x]["t_high"].ToString());
                    c = Double.Parse(dat.Rows[x]["t_low"].ToString());
                    e1 = Double.Parse(dat.Rows[x]["h_high"].ToString());
                    f = Double.Parse(dat.Rows[x]["h_low"].ToString());
                    label[x].Tag = "测点名称：  "+ss+"\n温度上限：  "+b.ToString()+ "  ℃\n温度下限：  " + c.ToString()+ "  %RH\n湿度上限：  " + e1.ToString()+ "  ℃\n湿度下限：  " + f.ToString()+ "  %RH";
                    label[x].Click += new EventHandler(lable_Click);
                    string measureMeterCode = dat.Rows[x]["measureMeterCode"].ToString();
                    string pointx = dat.Rows[x]["pointX"].ToString();
                    string pointy = dat.Rows[x]["pointY"].ToString();
                    if ("" != measureMeterCode && !"".Equals(measureMeterCode))
                    {
                        label[x].Name = measureMeterCode;
                        label[x].BorderStyle = BorderStyle.None;
                        label[x].Font = new Font(privateFonts.Families[0], 10.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        label[x].ForeColor = Color.White;
                        label[x].TextAlign = ContentAlignment.MiddleCenter;
                        label[x].Image = Image.FromFile(@str + "/images/cd1.png");
                        string temperature = dat.Rows[x]["temperature"].ToString();
                        string humidity = dat.Rows[x]["humidity"].ToString();
                        if (temperature != "" && !"".Equals(temperature))
                        {
                            a = Double.Parse(temperature);
                        }
                        else
                        {
                            a = 0;
                        }
                        if (humidity != "" && !"".Equals(humidity))
                        {
                            d = Double.Parse(humidity);
                        }
                        else
                        {
                            d = 0;
                        }
                        string kkk = dat.Rows[x]["housetype"].ToString();
                        if (kkk != null && !"".Equals(kkk))
                        {
                            kk = Int32.Parse(kkk);
                        }
                        else
                        {
                            kk = 0;
                        }
                       
                        if (pointx != null && !"".Equals(pointx) && pointy != null && !"".Equals(pointy))
                        {
                            label[x].Location = new Point(Int32.Parse(pointx), Int32.Parse(pointy));
                        }
                        else
                        {
                            if (x < 20) {
                                label[x].Location = new Point(20, p1);
                                p1 += 45;
                            } else if (x>=20 && x < 40) {
                                if (x == 20) { p1 = 10; };
                                label[x].Location = new Point(100, p1);
                                p1 += 45;
                            }else if (x >= 40 && x < 60)
                            {
                                if (x == 40) { p1 = 10; };
                                label[x].Location = new Point(180, p1);
                                p1 += 45;
                            }else if (x >= 60 && x < 80)
                            {
                                if (x == 60) { p1 = 10; };
                                label[x].Location = new Point(260, p1);
                                p1 += 45;
                            }else if (x >= 80 && x < 100)
                            {
                                if (x == 80) { p1 = 10; };
                                label[x].Location = new Point(340, p1);
                                p1 += 45;
                            }
                            else if (x >= 100 && x < 120)
                            {
                                if (x == 100) { p1 = 10; };
                                label[x].Location = new Point(420, p1);
                                p1 += 45;
                            }
                            else if (x >= 120 && x < 140)
                            {
                                if (x == 120) { p1 = 10; };
                                label[x].Location = new Point(500, p1);
                                p1 += 45;
                            }
                            else if (x >= 140 && x < 160)
                            {
                                if (x == 140) { p1 = 10; };
                                label[x].Location = new Point(580, p1);
                                p1 += 45;
                            }
                            else if (x >= 160 && x < 180)
                            {
                                if (x == 160) { p1 = 10; };
                                label[x].Location = new Point(660, p1);
                                p1 += 45;
                            }
                            else if (x >= 180 && x < 200)
                            {
                                if (x == 180) { p1 = 10; };
                                label[x].Location = new Point(720, p1);
                                p1 += 45;
                            }
                        }
                        DateTime dt1 = Convert.ToDateTime(dat.Rows[x]["devtime"].ToString());
                        DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        TimeSpan ts = dt2.Subtract(dt1);
                        if (ts.TotalMinutes >= double.Parse(overtime))
                        {
                            label[x].Text = "- - ℃" + "\n" + "- -%RH";
                            label[x].Image = Image.FromFile(@str + "/images/cd2.png");
                            //label[x].BackColor = Color.LightGray;//控件的背景颜色
                        }
                        else
                        {
                            if (a != 0 && d != 0)
                            {
                                label[x].Text = a.ToString("0.0") + " ℃" + "\n" + d.ToString("0.0") + " %RH";
                                if (a > b || a < c || d > e1 || d < f)
                                {
                                    label[x].Image = Image.FromFile(@str + "/images/cd3.png");
                                }
                                else
                                {
                                    label[x].Image = Image.FromFile(@str + "/images/cd1.png");
                                }
                            }
                            else
                            {
                                if (a == 0 && d == 0)
                                {
                                    label[x].Text = "0.0 ℃" + "\n" + "0.0%RH";
                                    if (a > b || a < c || d > e1 || d < f)
                                    {
                                        label[x].Image = Image.FromFile(@str + "/images/cd3.png");
                                    }
                                    else {
                                        label[x].Image = Image.FromFile(@str + "/images/cd1.png");
                                    }
                                }
                                else {
                                if (a == 0)
                                {
                                    label[x].Text = "0.0 ℃" + "\n" + d.ToString("0.0") + " %RH";
                                    if (d > e1 || d < f)
                                    {
                                        label[x].Image = Image.FromFile(@str + "/images/cd3.png");
                                    }
                                }
                                else
                                {
                                    label[x].Image = Image.FromFile(@str + "/images/cd1.png");
                                }
                                if (d == 0)
                                {
                                    label[x].Text = a.ToString("0.0") + " ℃" + "\n" + "0.0%RH";
                                    if (a > b || a < c)
                                    {
                                        label[x].Image = Image.FromFile(@str + "/images/cd3.png");
                                    }
                                }
                                else
                                {
                                    label[x].Image = Image.FromFile(@str + "/images/cd1.png");
                                }
                            }
                            }
                        }
                        if (kk == 1)
                        {
                            label[x].Size = new Size(75, 45);//大小
                            label[x].Image = Image.FromFile(@str + "/images/cdkong.png");
                        }
                    }
                    else {
                        label[x].BorderStyle = BorderStyle.None;
                        label[x].Font = new Font(privateFonts.Families[0], 10.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                        label[x].ForeColor = Color.White;
                        label[x].TextAlign = ContentAlignment.MiddleCenter;
                        if (pointx != null && !"".Equals(pointx) && pointy != null && !"".Equals(pointy))
                        {
                            label[x].Location = new Point(Int32.Parse(pointx), Int32.Parse(pointy));
                        }
                        label[x].Text = "- - ℃" + "\n" + "- -%RH";
                        label[x].Image = Image.FromFile(@str + "/images/cd2.png");
                    }

                    this.pictureBox1.Controls.Add(label[x]);

                }
                this.timer1.Start();
            }
            
        }
        private void lable_Click(object sender, EventArgs e)
        {
            Label lable = (Label)sender;
            string tag = lable.Tag.ToString();
            MessageBox.Show(tag);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            MessageBox.Show("最佳库房平面图尺寸大小："+(rect.Width-10)+" * "+ (rect.Height-30)+" 像素， 否则可能图片显示比较模糊或显示不全");
            //创建一个对话框对象   
            OpenFileDialog ofd = new OpenFileDialog();
            //为对话框设置标题   
            ofd.Title = "请选择上传的图片";
            //设置筛选的图片格式     图片格式|*.jpg
            ofd.Filter = "Jpg图片|*.jpg|Gif图片|*.gif|BMP图片|*.bmp|Png图片|*.png";
            //设置是否允许多选   
            ofd.Multiselect = false;
            //如果你点了“确定”按钮   
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //获得文件的完整路径（包括名字后后缀）  
                string filePath = ofd.FileName;
                //将文件路径显示在文本框中  
                this.textBox1.Text = filePath;
                //找到文件名比如“1.jpg”前面的那个“\”的位置  
                int position = filePath.LastIndexOf("\\");
                //从完整路径中截取出来文件名“1.jpg”  
                string fileName = filePath.Substring(position + 1);
                //读取选择的文件，返回一个流  
                using (System.IO.Stream stream = ofd.OpenFile())
                {
                    this.pictureBox1.Controls.Clear();
                    //创建一个流，用来写入得到的文件流（注意：创建一个名为“Images”的文件夹，如果是用相对路径，必须在这个程序的Degug目录下创建    
                    //如果是绝对路径，放在那里都行，我用的是相对路径）  
                    //String file_Path = @str + "/Images/" + fileName;
                    //if (System.IO.File.Exists(file_Path))
                    //{ System.IO.File.Delete(file_Path) }

                    // Random random = new Random();
                    //fileName = random.Next(10000, 99999) + fileName;
                    try {
                    using (System.IO.FileStream fs = new System.IO.FileStream(@str + "/Images/" + fileName, System.IO.FileMode.Create))
                    {
                        //将得到的文件流复制到写入流中  
                        stream.CopyTo(fs);
                        //将写入流中的数据写入到文件中  
                        fs.Flush();
                        fs.Close();
                    }
                    stream.Close();
                    }
                    catch
                    {
                        DialogResult result = MessageBox.Show("程序发生未知错误，需重新退出重新登录！", "操作提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            System.Environment.Exit(0);
                        }
                    }
                    //PictrueBOx 显示该图片，此时这个图片已经被复制了一份在Images文件夹下，就相当于上传    
                    //至于上传到别的地方你再更改思路就行，这里只是演示过程    
                    //pbShow.ImageLocation = @"./Images/" + fileName;

                    //this.pictureBox1.Width = Image.FromFile(@str + "/images/" + fileName).Height;
                    //this.pictureBox1.Height = Image.FromFile(@str + "/images/" + fileName).Width; 

                    //把图片路径保存数据库
                    string id = this.comboBox1.SelectedValue.ToString();
                    bool bl= hts.updateHouseTypeById(fileName,id);
                    if (bl) {
                        this.pictureBox1.Controls.Clear();
                        this.timer1.Stop();
                        MessageBox.Show("恭喜，库房平面图上传成功！");
                        this.pictureBox1.Image = Image.FromFile(@str + "/images/" + fileName);
                        dt = hts.queryhouseType();
                    }
                }
            }
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.Close();
        }
        private Point movePoint;

        private void button3_Click(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.pictureBox1.Controls.Clear();
            string code = this.comboBox1.SelectedValue.ToString();
            service.deviceInformationService dis = new service.deviceInformationService();
            DataTable dt = dis.queryDeviceByHouseTypeCode(code);
            int count = dt.Rows.Count;
            if (count > 0)
            {
                labels = new Label[count];//例如10个
                int p = 10;
                for (int x = 0; x < count; x++)
                {
                    string pointx = dt.Rows[x]["pointX"].ToString();
                    string pointy = dt.Rows[x]["pointY"].ToString();
                    labels[x] = new Label();
                    labels[x].AutoSize = true;
                    labels[x].Name = dt.Rows[x]["measureCode"].ToString()+"_"+ dt.Rows[x]["meterNo"].ToString();
                    labels[x].Text = dt.Rows[x]["terminalname"].ToString();
                    labels[x].BorderStyle = BorderStyle.None;
                    //labels[x].BackColor = Color.White;//控件的背景颜色
                    labels[x].Font = new Font(privateFonts.Families[0], 10.0F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    if (pointx != null && !"".Equals(pointx) && pointy != null && !"".Equals(pointy))
                    {
                        labels[x].Location = new Point(Int32.Parse(pointx), Int32.Parse(pointy));
                    }
                    else
                    {
                        //labels[x].Location = new Point(10, p);
                        //p += 45;
                        if (x < 20)
                        {
                            labels[x].Location = new Point(10, p);
                            p += 45;
                        }
                        else if (x >= 20 && x < 40)
                        {
                            if (x == 20) { p = 10; };
                            labels[x].Location = new Point(160, p);
                            p += 45;
                        }
                        else if (x >= 40 && x < 60)
                        {
                            if (x == 40) { p = 10; };
                            labels[x].Location = new Point(310, p);
                            p += 45;
                        }
                        else if (x >= 60 && x < 80)
                        {
                            if (x == 60) { p = 10; };
                            labels[x].Location = new Point(460, p);
                            p += 45;
                        }
                        else if (x >= 80 && x < 100)
                        {
                            if (x == 80) { p = 10; };
                            labels[x].Location = new Point(610, p);
                            p += 45;
                        }
                        else if (x >= 100 && x < 120)
                        {
                            if (x == 100) { p = 10; };
                            labels[x].Location = new Point(760, p);
                            p += 45;
                        }
                        else if (x >= 120 && x < 140)
                        {
                            if (x == 120) { p = 10; };
                            labels[x].Location = new Point(910, p);
                            p += 45;
                        }
                        else if (x >= 140 && x < 160)
                        {
                            if (x == 140) { p = 10; };
                            labels[x].Location = new Point(1060, p);
                            p += 45;
                        }
                        else if (x >= 160 && x < 180)
                        {
                            if (x == 160) { p = 10; };
                            labels[x].Location = new Point(1210, p);
                            p+= 45;
                        }
                        else if (x >= 180 && x < 200)
                        {
                            if (x == 180) { p = 10; };
                            labels[x].Location = new Point(1360, p);
                            p += 45;
                        }
                    }
                    labels[x].MouseMove += new MouseEventHandler(this.label_MouseMove);
                    labels[x].MouseDown += new MouseEventHandler(this.label_MouseDown);
                    this.pictureBox1.Controls.Add(labels[x]);
                   
                }
                flag = true;
            }
            else
            {
                MessageBox.Show("此库房没有对应的测点(仪表)，请去测点管理功能中添加或修改测点(仪表)数据！");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string localFilePath = String.Empty;
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.InitialDirectory = "C://";

            fileDialog.Filter = "txt files (*.db)|*.db|All files (*.*)|*.*";

            //设置文件名称：
            fileDialog.FileName = this.comboBox1.Text+"库房图.jpg";

            fileDialog.FilterIndex = 2;

            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {   //获得文件路径
                localFilePath = fileDialog.FileName.ToString();
                try
                {
                    //保存窗体到图片
                    Bitmap formBitmap = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
                    this.DrawToBitmap(formBitmap, new Rectangle(0, 0, this.pictureBox1.Width, this.pictureBox1.Height));
                    formBitmap.Save(localFilePath, System.Drawing.Imaging.ImageFormat.Bmp);
                    MessageBox.Show("恭喜，图片保存成功！");
                }
                catch
                {
                    MessageBox.Show("图片保存失败，请重新保存！");
                }
            }
            //保存控件DataGridView到图片搜索
            //Bitmap controlBitmap = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            //this.dataGridView1.DrawToBitmap(controlBitmap, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            //controlBitmap.Save(@"D:\form.jpg", System.Drawing.Imaging.ImageFormat.Bmp);
        }

        private void imgUp_FormClosed(object sender, FormClosedEventArgs e)
        {
            button2_Click(null,null);
        }

        private void label_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                movePoint = e.Location;
            }
        }
        private void label_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ((Label)sender).Location = new Point(((Label)sender).Location.X + e.X - movePoint.X, ((Label)sender).Location.Y + e.Y - movePoint.Y);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (flag)
            {
                foreach (Control ctr in this.pictureBox1.Controls)
                {
                    //判断该控件是不是Label
                    if (ctr is Label)
                    { //将ctr转换成Label并赋值给ck
                        Label lb = ctr as Label;
                        string[] code1 = lb.Name.Split('_');

                        Point tt = lb.Location;
                        bean.deviceInformation di = new bean.deviceInformation();
                        di.measureCode = code1[0].ToString();
                        di.meterNo = code1[1].ToString();
                        di.pointX = tt.X;
                        di.pointY = tt.Y;
                        service.deviceInformationService dis = new service.deviceInformationService();
                        dis.updateIformationByPoint(di);

                    }
                }
                MessageBox.Show("平面图仪表位置信息保存成功！");
                string code = this.comboBox1.SelectedValue.ToString();
                imgWenshidu(code);
                flag = false;
            }
            else {
                MessageBox.Show("请先编辑测点位置，再点击保存！");
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            this.timer1.Stop();
            string code = this.comboBox1.SelectedValue.ToString();
            imgSwitch(code);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int kk = 0;
            string code = this.comboBox1.SelectedValue.ToString();
            DataTable dat = ms.queryMonitoringByhousecode(code);
            int num = dat.Rows.Count;
            if (num < 1) { return; }
            String codemeter;
            String temperature;
            String humidity;
            for (int i = 0; i < num; i++)
            {
                codemeter = dat.Rows[i]["measureMeterCode"].ToString();
                if (codemeter.Length != 0)
                {
                    temperature = dat.Rows[i]["temperature"].ToString();
                    humidity = dat.Rows[i]["humidity"].ToString();
                    if (temperature != "" && !"".Equals(temperature))
                    {
                        a = Double.Parse(temperature);
                    }
                    else
                    {
                        a = 0;
                    }
                    if (humidity != "" && !"".Equals(humidity))
                    {
                        d = Double.Parse(humidity);
                    }
                    else
                    {
                        d = 0;
                    }
                    //a = Double.Parse(temperature);
                    b = Double.Parse(dat.Rows[i]["t_high"].ToString());
                    c = Double.Parse(dat.Rows[i]["t_low"].ToString());
                    //d = Double.Parse(humidity);
                    e1 = Double.Parse(dat.Rows[i]["h_high"].ToString());
                    f = Double.Parse(dat.Rows[i]["h_low"].ToString());
                    string kkk = dat.Rows[i]["housetype"].ToString();
                    if (kkk != null && !"".Equals(kkk))
                    {
                        kk = Int32.Parse(kkk);
                    }
                    else
                    {
                        kk = 0;
                    }
                    if (a > b || a < c || d > e1 || d < f)
                    {
                        // ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).BackColor = Color.OrangeRed;//控件的背景颜色
                        ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Image = Image.FromFile(@str + "/images/cd3.png");
                    }
                    DateTime dt1 = Convert.ToDateTime(dat.Rows[i]["devtime"].ToString());
                    DateTime dt2 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    TimeSpan ts = dt2.Subtract(dt1);
                    if (ts.TotalMinutes >= double.Parse(overtime))
                    {
                        ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Text = "- - ℃" + "\n" + "- -%RH";
                        //((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).BackColor = Color.LightGray;//控件的背景颜色
                        ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Image = Image.FromFile(@str + "/images/cd2.png");
                    }
                    else
                    {
                        if (a == 0 && d == 0)
                        {
                            ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Text = "0.0 ℃" + "\n" + "0.0%RH";
                        }
                        else
                        {
                            ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Text = a.ToString("0.0") + " ℃" + "\n" + d.ToString("0.0") + " %RH";
                        }
                    }
                    if (kk == 1)
                    {
                        ((Label)this.pictureBox1.Controls.Find(codemeter, false)[0]).Image = Image.FromFile(@str + "/images/cdkong.png");
                    }
                }
            }
        }
    }
}
