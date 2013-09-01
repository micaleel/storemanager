using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreManager.Models {
    public enum ResetPasswordResult {
        Success,
        InvalidUserName,
        InvalidAnswer
    }
    public enum Role { User, Admin }

}