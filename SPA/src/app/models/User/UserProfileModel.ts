export interface UserProfileModel {
  id: string;
  firstName: string;
  lastName: string;
  dateOfBirth: Date;
  phoneNumber: string;
  address?: string;
  email: string;
  role: string;
  profileImageFile?: File;
  profileImageBase64?: string;
}
