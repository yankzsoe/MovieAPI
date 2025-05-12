namespace MovieAPI.Application.Common.Models {
    public class PolicyModel {
        public string Name { get; set; }
        public string Type { get; set; }
        public PolicyModel(string Name, string type) {
            this.Name = Name;
            Type = type;
        }

        public static PolicyModel GetMovie {
            get {
                return new PolicyModel("Get Movie", "Permission");
            }
        }

        public static PolicyModel CreateMovie {
            get {
                return new PolicyModel("Create Movie", "Permission");
            }
        }

        public static PolicyModel UpdateMovie {
            get {
                return new PolicyModel("Update Movie", "Permission");
            }
        }

        public static PolicyModel DeleteMovie {
            get {
                return new PolicyModel("Delete Movie", "Permission");
            }
        }

    }
}
