using System.Collections.Generic;
using ScoutingServer.SQLDataObjects;
using RobotServer.ClientData;

namespace RobotServer.SQLDataObjects {
    public class Account  {

        public Account() {

        }

        public Account(string Id) : base() {
            this.Id = Id;
            Security = null;
            Role = new Role(Id, Role.ROLE_LEVEL_DEV);// TODO: change this to only give user level Role to new users.
        }

        public ClientAccount GetClientAccount() {
            return new ClientAccount {
                Id = Id,
                Username = Username,
                TeamNumber = TeamNumber,
                RealName = RealName
            };
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string RealName { get; set; }
        public string TeamNumber { get; set; }
        public virtual Role Role { get; set; }
        public virtual AccountSecurity Security { get; set; }

        public static bool operator ==(Account a, Account b) {
            if(a != null && b != null) {
                return a.Id == b.Id;
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
                return (obj as Account).Id == Id;
            return false;
        }

        public override int GetHashCode() {
            return Id.GetHashCode();
        }
    }
}