using System;
using System.Linq;
using StoreManager.Models;

namespace StoreManager.Helpers {
    public static class StoreManagerContextExtensions {

        public static User Get(this StoreManagerContext db, int userId) {
            return db.Users.SingleOrDefault(u => u.UserId == userId);
        }

        public static User Get(this StoreManagerContext db, string userName) {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public static bool Authenticate(this StoreManagerContext db, string userName, string password) {
            return db.Users.Any(x => x.UserName == userName && x.Password == password);
        }

        public static void Update(this StoreManagerContext db, User row) {
            var user = db.Get(row.UserName);
            if (user == null) return;

            user.Approved = row.Approved;
            user.DisplayName = row.DisplayName;
            user.Locked = row.Locked;
            user.Role = row.Role;
            user.SecretAnswer = row.SecretAnswer;
            user.SecretQuestion = row.SecretQuestion;

            db.SaveChanges();
        }

        public static void ApproveRequest(this StoreManagerContext db, int userId) {

            var user = db.Get(userId);
            if (user == null) return;

            user.Approved = true;

            db.SaveChanges();
        }

        public static void RejectRequest(this StoreManagerContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Approved = false;

            db.SaveChanges();
        }

        public static void LockAccount(this StoreManagerContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Locked = true;

            db.SaveChanges();
        }

        public static void UnlockAccount(this StoreManagerContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Locked = false;

            db.SaveChanges();
        }

        public static void UpdateLastLoginDate(this StoreManagerContext db, string userName) {
            var user = db.Get(userName);
            if (user == null) return;

            user.LastLogin = DateTime.UtcNow;

            db.SaveChanges();
        }

        public static ResetPasswordResult ResetPassword(this StoreManagerContext db, string userName, string question, string answer, string password) {

            var user = db.Get(userName);
            if (user == null) return ResetPasswordResult.InvalidUserName;
            if (user.SecretAnswer != answer) return ResetPasswordResult.InvalidAnswer;

            user.Password = password;
            db.SaveChanges();

            return ResetPasswordResult.Success;
        }

        public static int CreateUser(this StoreManagerContext db, User u) {
            db.Users.Add(u);
            db.SaveChanges();
            return u.UserId;
        }

    }
}