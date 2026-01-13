export interface PeoplePaginationRequest {
  userId: string;
  offset?: number;
  limit?: number;
  sort?: string;
  order?: "ASC" | "DESC";
  searchTerm?: string;
  isActive?: boolean;
  createdTime?: string;
}