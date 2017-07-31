using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LBKJClient.bean
{
    class UserInfo
    {
        private String id;
        /// <summary>
        /// 用户名
        /// </summary>
        private String userName;

        /// <summary>
        /// 密码
        /// </summary>
        private String pwd;

        /// <summary>
        /// 是否停用
        /// </summary>
        private int enable;

        /// <summary>
        /// 角色权限
        /// </summary>
        private int auth;

        /// <summary>
        /// 权限
        /// </summary>
        private string power;

        public string Power
        {
            get
            {
                return power;
            }

            set
            {
                power = value;
            }
        }
        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                userName = value;
            }
        }

        public string Pwd
        {
            get
            {
                return pwd;
            }

            set
            {
                pwd = value;
            }
        }

        public int Enable
        {
            get
            {
                return enable;
            }

            set
            {
                enable = value;
            }
        }

        public int Auth
        {
            get
            {
                return auth;
            }

            set
            {
                auth = value;
            }
        }
    }
}
