import { ToastType } from "../enums/toastType";

export interface ToastMessage {
  message?: string,
  type: ToastType
}
