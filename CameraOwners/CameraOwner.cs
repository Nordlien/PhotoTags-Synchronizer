using System;
using System.Collections.Generic;

namespace CameraOwners
{
    public class CameraOwner
    {
        private string make;
        private string model;
        private string owner;
        public static readonly string UnknownMake = "(Unknown)";
        public static readonly string UnknownModel = "(Unknown)";
        

        public CameraOwner(string make, string model, string owner)
        {
            this.make = make;
            this.model = model;
            this.owner = owner;
        }

        public string Make { get => make; set => make = string.IsNullOrWhiteSpace(value) ? UnknownMake : value; }
        public string Model { get => model; set => model = string.IsNullOrWhiteSpace(value) ? UnknownModel : value; }
        public string Owner { get => owner; set => owner = value; }

        public override string ToString()
        {
            return make + System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator + " " + model;
        }

        public override bool Equals(object obj)
        {
            return obj is CameraOwner owner &&
                   make == owner.make &&
                   model == owner.model &&
                   this.owner == owner.owner;
        }

        public override int GetHashCode()
        {
            int hashCode = -86807459;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(make);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(model);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(owner);
            return hashCode;
        }

        public static bool operator ==(CameraOwner left, CameraOwner right)
        {
            return EqualityComparer<CameraOwner>.Default.Equals(left, right);
        }

        public static bool operator !=(CameraOwner left, CameraOwner right)
        {
            return !(left == right);
        }

        public static bool CameraMakeModelExistInList(List<CameraOwner> cameraOwners, CameraOwner cameraOwner)
        {
            foreach (CameraOwner cameraOwnerToCheck in cameraOwners)
            {
                if (cameraOwner.Make == cameraOwnerToCheck.Make &&
                    cameraOwner.Model == cameraOwnerToCheck.Model) return true;
            }
            return false;
        }
    }
}
