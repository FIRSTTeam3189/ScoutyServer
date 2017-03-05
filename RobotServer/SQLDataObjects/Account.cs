using RobotServer.ClientData;

namespace RobotServer.SQLDataObjects {
    public class Account  {
        public const int ROLE_LEVEL_USER = 0;
        public const int ROLE_LEVEL_MOD = 1;
        public const int ROLE_LEVEL_ADMIN = 2;
        public const int ROLE_LEVEL_DEV = 3;

        public Account() {

        }

        public Account(string Id) : base() {
            this.Id = Id;
            Security = null;
            AuthLevel = Account.ROLE_LEVEL_DEV;
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
        public int AuthLevel { get; set; }
        public virtual AccountSecurity Security { get; set; }
        public bool IsModLevel() {
            return AuthLevel >= ROLE_LEVEL_MOD;
        }

        public bool IsAdminLevel() {
            return AuthLevel >= ROLE_LEVEL_ADMIN;
        }

        public bool IsDevLevel() {
            return AuthLevel >= ROLE_LEVEL_DEV;
        }

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