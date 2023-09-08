import { Role } from "./Role";

export interface UserProfileModel {
  id?: string;
  firstName?: string;
  lastName?: string;
  dateOfBirth?: Date;
  phoneNumber?: string;
  address?: string;
  email?: string;
  role: Role;
  profileImageFile?: File;
  profileImageBase64?: string;
}
