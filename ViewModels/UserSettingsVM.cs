using AphidBT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AphidBT.ViewModels
{
    public class UserSettingsVM
    {
        public IndexViewModel IndexVM { get; set; }

        public ChangePasswordViewModel ChangePasswordVM { get; set; }

        public ExtChangeProfileViewModel ExtChangeProfileVM { get; set; }

        public ManageLoginsViewModel ManageLoginsVM { get; set; }
    }
}