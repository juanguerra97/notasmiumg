
export default interface ServerResponse {
  status: number;
  message: string;
  data?: any | any[];
  error?: string;
}
