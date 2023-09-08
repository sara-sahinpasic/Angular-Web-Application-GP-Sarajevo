export interface UserEditProfileModel {
  id?: string;
  firstName?: string;
  lastName?: string;
  phoneNumber?: string;
  address?: string;
  profileImageFile?: File;
  password?: string | null;
  profileImageBase64?: string;
}
