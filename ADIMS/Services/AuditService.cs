using ADIMS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;

namespace ADIMS.Services
{
    public class AuditService
    {
        private static readonly adimsEntities db = new adimsEntities();
        //Class to handle Audit logs

        public static void Login(string username)
        {
            auditlog _log = new auditlog()
            {
                activity = "login",
                email = username,
                description = $"User Logged in",
                date_created = DateTime.Now
            };
            db.auditlogs.Add(_log);
            db.SaveChanges();

        }
        
        public static void AddEntry<T>(T _record, string username)
        {
            auditlog _log = new auditlog()
            {
                activity = "add",
                email = username,
                description = $"Added new {ResolveName(typeof(T))}",
                date_created = DateTime.Now
            };
            
            db.auditlogs.Add(_log);
            db.SaveChanges();
        }

        public static void View(string username,string comments)
        {
            auditlog _log = new auditlog()
            {
                activity = "view",
                email = username,
                description = $"{comments}",
                date_created = DateTime.Now
            };

            db.auditlogs.Add(_log);
            db.SaveChanges();
        }

        public static void Search(string username, string comments)
        {
            auditlog _log = new auditlog()
            {
                activity = "search",
                email = username,
                description = $"{comments}",
                date_created = DateTime.Now
            };

            db.auditlogs.Add(_log);
            db.SaveChanges();
        }

        public static void AddEntry<T>(T _record, string username, string _comments)
        {
            auditlog _log = new auditlog()
            {
                activity = "add",
                email = username,
                description = $"Added new {ResolveName(typeof(T))}. Details: {_comments }",
                date_created = DateTime.Now
            };

            db.auditlogs.Add(_log);
            db.SaveChanges();
        }

        public static void EditEntry<T>(T _record, string username)
        {
            auditlog _log = new auditlog()
            {
                activity = "edit",
                email = username,
                description = $"Edited item {ResolveName(typeof(T))}",
                date_created = DateTime.Now
            };

            db.auditlogs.Add(_log);
            db.SaveChanges();
        }
        public static void EditEntry<T>(T _record, string username, string _comments)
        {
            auditlog _log = new auditlog()
            {
                activity = "edit",
                email = username,
                description = $"Edited item {ResolveName(typeof(T))}. Details: {_comments }",
                date_created = DateTime.Now
            };
            db.auditlogs.Add(_log);
            db.SaveChanges();
        }

        private static string ResolveName(Type _type)
        {
            try
            {
                string result = "";
                Dictionary<Type, string> _typeList = new Dictionary<Type, string>();

                _typeList.Add(typeof(ApplicationUser), "User");
                _typeList.Add(typeof(IdentityRole), "Role");
                _typeList.Add(typeof(cropacreage), "Crop Acreage");

                result = _typeList[_type] ?? _type.Name.ToUpper();

                return result;
            }
            catch (Exception)
            {
                return $"{_type.Name} Record";
            }
        }


    }
}