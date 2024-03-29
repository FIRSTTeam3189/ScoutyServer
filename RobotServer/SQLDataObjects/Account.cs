﻿using RobotServer.ClientData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace RobotServer.SQLDataObjects {
    public class Account : IdentityUser{

        public Account() {

        }

        /*public Account(string Id) : base() {
            this.Id = Id;
        }*/

        public ClientAccount GetClientAccount() {
            return new ClientAccount {
                //Id = Id,
                Username = UserName,
                TeamNumber = TeamNumber,
                RealName = RealName
            };
        }

        //public string Id { get; set; }
        public string RealName { get; set; }
        public int TeamNumber { get; set; }
        //public List<RobotEvent> RobotEvents { get; set; }
        //public byte[] SaltedAndHashedPassword { get; set; }

        public static bool operator ==(Account a, Account b) {
            if(a != null && b != null) {
                return a.UserName == b.UserName;
            } else if(a == null && b == null) {
                return true;
            }
            return false;
        }

        public static bool operator !=(Account a, Account b) {
            return !(a == b);
        }

        public override bool Equals(object obj) {
            if(obj is Account)
                return (obj as Account).UserName == UserName;
            return false;
        }

        public override int GetHashCode() {
            return UserName.GetHashCode();
        }
    }
}