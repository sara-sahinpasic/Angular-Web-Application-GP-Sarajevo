interface BaseNewsDto {
  id?: string
  title?: string;
  content?: string;
  date?: string;
}

export interface NewsResponseDto extends BaseNewsDto {
  createdBy?: string;
}

export interface NewsRequestDto extends BaseNewsDto {
  userId?: string;
}
