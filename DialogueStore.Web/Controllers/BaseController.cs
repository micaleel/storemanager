using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using DialogueStore.Web.Infrastructure;
using DialogueStore.Web.Models;
using System.Collections.Generic;

namespace DialogueStore.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        protected DialogueStoreContext Db { get; private set; }

        protected BaseController()
        {
            Db = new DialogueStoreContext();
            Mapper.AddProfile<DialogueStoreProfile>();
        }

        protected static FileResult DownloadCsvImpl<TEntity>(string[] columnHeaders,
            IEnumerable<TEntity> list, Func<TEntity, string> toCsv, string fileName)
        {

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.WriteLine(string.Join(", ", columnHeaders));

            foreach (var entity in list)
                writer.WriteLine(toCsv(entity));

            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            return new FileStreamResult(stream, System.Net.Mime.MediaTypeNames.Text.Plain)
            {
                FileDownloadName = fileName,
            };
        }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User == null) return;
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated) return;
            if (!(filterContext.HttpContext.User.Identity is FormsIdentity)) return;
            var id = filterContext.HttpContext.User.Identity as FormsIdentity;
            if (id == null) return;
            var ticket = id.Ticket;
            var userData = ticket.UserData;
            var userDateTokens = userData.Split('|');
            var roles = userDateTokens[0];

            filterContext.HttpContext.User = new GenericPrincipal(id, new[] { roles });
        }

        public string LoggedInUserName()
        {
            return User.Identity.Name;
        }

        public User LoggedInUser()
        {
            var userName = LoggedInUserName();
            return Db.Users.SingleOrDefault(u => u.UserName == userName);
        }

        public void LogActivity(TimelineActivity activity)
        {
            var currentUser = LoggedInUser();

            activity.Date = DateTime.UtcNow;
            activity.SubjectId = currentUser.UserId;
            activity.SubjectName = currentUser.UserName;
            activity.SubjectDescriptor = currentUser.DisplayName;

            Db.TimelineActivities.Add(activity);
            Db.SaveChanges();
        }

        public void LogActivity(string verb)
        {
            LogActivity(new TimelineActivity { Verb = verb });
        }

        public void LogActivity(string verb, TimelineActivityObjectType objectType, string objectDescriptor, int? objectId)
        {
            LogActivity(new TimelineActivity
            {
                Verb = verb,
                ObjectDescriptor = objectDescriptor,
                ObjectType = objectType,
                ObjectId = objectId
            });
        }

        public void FlashInfo(string message)
        {
            TempData["ToastInfo"] = message;
        }

        public void FlashSuccess(string message)
        {
            TempData["ToastSuccess"] = message;
        }

        public void FlashError(string message)
        {
            TempData["ToastError"] = message;
        }

        public void FlashWarning(string message)
        {
            TempData["ToastWarning"] = message;
        }

        public void FlashDeleted(string descriptor)
        {
            FlashInfo(string.Format("The record '{0}' has been deleted successfully", descriptor));
        }

        public void FlashUpdated(string descriptor)
        {
            FlashInfo(string.Format("The record '{0}' has been updated successfully", descriptor));
        }

        public void FlashAdded(string descriptor)
        {
            FlashInfo(string.Format("The record '{0}' has been added successfully", descriptor));
        }
        protected override void Dispose(bool disposing)
        {
            Db.Dispose();
            base.Dispose(disposing);
        }
    }
}
