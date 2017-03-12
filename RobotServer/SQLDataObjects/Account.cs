using RobotServer.ClientData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RobotServer.SQLDataObjects {
    public class Account  {

        public Account() {

        }

        /*public Account(string Id) : base() {
            this.Id = Id;
        }*/

        public ClientAccount GetClientAccount() {
            return new ClientAccount {
                //Id = Id,
                Username = Username,
                TeamNumber = TeamNumber,
                RealName = RealName
            };
        }

        //public string Id { get; set; }
        [Key]
        public string Username { get; set; }
        public string RealName { get; set; }
        public string TeamNumber { get; set; }
        //public List<RobotEvent> RobotEvents { get; set; }
        public byte[] Salt { get; set; }
        public byte[] SaltedAndHashedPassword { get; set; }

        public static bool operator ==(Account a, Account b) {
            if(a != null && b != null) {
                return a.Username == b.Username;
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
                return (obj as Account).Username == Username;
            return false;
        }

        public override int GetHashCode() {
            return Username.GetHashCode();
        }
    }
}