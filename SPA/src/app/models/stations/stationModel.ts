export interface StationModel {
  id: string;
  name: string;
}

export interface StationListModel extends StationModel {
  isEditing: boolean;
  oldNameValue?: string;
}
