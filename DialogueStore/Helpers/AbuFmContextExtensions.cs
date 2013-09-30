using System;
using System.Linq;
using DialogueStore.Models;

namespace DialogueStore.Helpers {
    public static class DialogueStoreContextExtensions {

        public static User Get(this DialogueStoreContext db, int userId) {
            return db.Users.SingleOrDefault(u => u.UserId == userId);
        }

        public static User Get(this DialogueStoreContext db, string userName) {
            return db.Users.SingleOrDefault(x => x.UserName == userName);
        }

        public static bool Authenticate(this DialogueStoreContext db, string userName, string password) {
            return db.Users.Any(x => x.UserName == userName && x.Password == password);
        }

        public static void Update(this DialogueStoreContext db, User row) {
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

        public static void ApproveRequest(this DialogueStoreContext db, int userId) {

            var user = db.Get(userId);
            if (user == null) return;

            user.Approved = true;

            db.SaveChanges();
        }

        public static void RejectRequest(this DialogueStoreContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Approved = false;

            db.SaveChanges();
        }

        public static void LockAccount(this DialogueStoreContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Locked = true;

            db.SaveChanges();
        }

        public static void UnlockAccount(this DialogueStoreContext db, int userId) {
            var user = db.Get(userId);
            if (user == null) return;

            user.Locked = false;

            db.SaveChanges();
        }

        public static void UpdateLastLoginDate(this DialogueStoreContext db, string userName) {
            var user = db.Get(userName);
            if (user == null) return;

            user.LastLogin = DateTime.UtcNow;

            db.SaveChanges();
        }

        public static ResetPasswordResult ResetPassword(this DialogueStoreContext db, string userName, string question, string answer, string password) {

            var user = db.Get(userName);
            if (user == null) return ResetPasswordResult.InvalidUserName;
            if (user.SecretAnswer != answer) return ResetPasswordResult.InvalidAnswer;

            user.Password = password;
            db.SaveChanges();

            return ResetPasswordResult.Success;
        }

        public static int CreateUser(this DialogueStoreContext db, User u) {
            db.Users.Add(u);
            db.SaveChanges();
            return u.UserId;
        }

    }
}