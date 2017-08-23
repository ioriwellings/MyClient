using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LBKJClient
{
    public partial class PagerControl : UserControl
    {
        //定义一个事件，用于在引发分页时，方便外部重新获取数据绑定：
        public event EventHandler OnPageChanged;
        public PagerControl()
        {
            InitializeComponent();
        }

        private void PagerControl_Load(object sender, EventArgs e)
        {

        }
        #region 分页字段和属性

        private int pageIndex = 1;
        /// <summary>
        /// 当前页面
        /// </summary>
        public virtual int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageSize = 1000;
        /// <summary>
        /// 每页记录数
        /// </summary>
        public virtual int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }

        private int recordCount = 0;
        /// <summary>
        /// 总记录数
        /// </summary>
        public virtual int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }

        private int pageCount = 0;
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                if (pageSize != 0)
                {
                    pageCount = GetPageCount();
                }
                return pageCount;
            }
        }
        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <returns></returns>
        private int GetPageCount()
        {
            if (PageSize == 0)
            {
                return 0;
            }
            //pageCount = RecordCount / PageSize;
            if (RecordCount % PageSize == 0)
            {
                pageCount = RecordCount / PageSize;
            }
            else
            {
                pageCount = RecordCount / PageSize + 1;
            }
            return pageCount;
        }

        #endregion  点击事件
        private void lnkFirst_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = 1;
            DrawControl(true);
        }

        private void lnkPrev_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Max(1, PageIndex - 1);
            DrawControl(true);
        }

        private void lnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = Math.Min(PageCount, PageIndex + 1);
            DrawControl(true);
        }

        private void lnkLast_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PageIndex = PageCount;
            DrawControl(true);
        }

        /// <summary>
        /// 外部调用
        /// </summary>
        public void DrawControl(int count)
        {
            recordCount = count;
            DrawControl(false);
        }
        /// <summary>
        /// 页面控件呈现
        /// </summary>
        private void DrawControl(bool callEvent)
        {
            tbxGo.Text = PageIndex.ToString();
            lblCurrentPage.Text = PageIndex.ToString();
            lblPageCount.Text = PageCount.ToString();
            lblTotalCount.Text = RecordCount.ToString();
            txtPageSize.Text = PageSize.ToString();

            if (callEvent && OnPageChanged != null)
            {
                OnPageChanged(this, null);//当前分页数字改变时，触发委托事件
            }
            SetFormCtrEnabled();
            if (PageCount == 1)//有且仅有一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
                btnGO.Enabled = false;
            }
            else if (PageIndex == 1)//第一页
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else if (PageIndex == PageCount)//最后一页
            {
                lnkNext.Enabled = false;
                lnkLast.Enabled = false;
            }
        }
        private void SetFormCtrEnabled()
        {
            lnkFirst.Enabled = true;
            lnkPrev.Enabled = true;
            lnkNext.Enabled = true;
            lnkLast.Enabled = true;
            btnGO.Enabled = true;
        }

        /// <summary>
        /// enter键功能
        /// </summary>
        private void tbxGo_KeyPress(object sender, KeyPressEventArgs e)
        {
            btnGo_Click(null, null);
        }

        /// <summary>
        /// 跳转页数限制
        /// </summary>
        private void tbxGo_TextChanged(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(tbxGo.Text.Trim(), out num) && num > 0)
            {
                if (num > PageCount)
                {
                    tbxGo.Text = PageCount.ToString();
                }
            }
        }

        /// <summary>
        /// 跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (int.TryParse(tbxGo.Text.Trim(), out num) && num > 0)
            {
                PageIndex = num;
                DrawControl(true);
            }
        }


        bool isTextChanged = false;
        /// <summary>
        /// 分页属性改变了。
        /// </summary>
        private void txtPageSize_TextChanged_1(object sender, EventArgs e)
        {
            int num = 0;
            if (!int.TryParse(txtPageSize.Text.Trim(), out num) || num <= 0)
            {
                num = 1000;
                txtPageSize.Text = "1000";
            }
            else
            {
                isTextChanged = true;
            }
            pageSize = num;
        }
        /// <summary>
        /// 光标离开分页属性
        private void txtPageSize_Leave(object sender, EventArgs e)
        {
            if (isTextChanged)
            {
                isTextChanged = false;
                lnkFirst_LinkClicked_1(null, null);
            }
        }
    }
}
